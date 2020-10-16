using ArgentPonyWarcraftClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PCDataDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Database;
using Tools.Database.Models;

namespace Tools.Services
{
    public class WowToolsService : IWowToolsService
    {
        private const int characterCachetime = 10;
        private WarcraftClient warcraftClient;
        private ToolContext dbContext;

        public WowToolsService(ToolContext dbContext)
        {
            this.dbContext = dbContext;

            var clientId = SecureData.BlizzardApiClientId;
            var clientSecret = SecureData.BlizzardApiClientSecret;

            warcraftClient = new WarcraftClient(clientId, clientSecret, Region.Europe, Locale.en_GB);
        }

        public IList<CharacterEquipmentCache> GetAllCharacterItems(IList<Character> characters)
        {
            throw new NotImplementedException();
        }

        public string AddCharacter(string playerName, string name, string realm)
        {
            var result = warcraftClient.GetCharacterEquipmentSummaryAsync(realm.ToLower().Replace(" ", "-"), name.ToLower(), "profile-eu").Result;

            if (result.Success)
            {
                var character = result.Value.Character;

                var existingCharacter = dbContext.Characters.FirstOrDefault(x => x.Name == character.Name && x.Realm == character.Realm.Slug && !x.Removed);
                if (existingCharacter != null)
                {
                    if (!String.IsNullOrWhiteSpace(playerName) &&
                        !String.Equals(playerName, existingCharacter.PlayerName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        existingCharacter.PlayerName = playerName;
                        dbContext.SaveChanges();
                        return "Character Edited";
                    }
                    return "Character already exists";
                }

                dbContext.Characters.Add(new Character
                {
                    PlayerName = playerName,
                    Name = character.Name,
                    Realm = character.Realm.Slug,
                    AddedTime = DateTime.Now
                });
                dbContext.SaveChanges();

                return "";
            }

            return result.Error.Detail;
        }

        public IList<Character> GetAllCharacters()
        {
            return dbContext.Characters.Where(x => !x.Removed).ToList();
        }

        public CharacterEquipmentCache GetCharacterItems(int characterId)
        {
            Character character = dbContext.Characters.SingleOrDefault(x => x.Id == characterId);

            if (character == null) return null;

            var cutoffTime = DateTime.Now.AddMinutes(-characterCachetime);
            var cache = dbContext.CharacterEquipmentCaches
                .Include(x => x.Items)
                .Where(x => x.Character == character && x.CacheTime > cutoffTime)
                .FirstOrDefault();
            if (cache != null)
                return cache;

            var newCache = new CharacterEquipmentCache
            {
                CacheTime = DateTime.Now,
                Character = character
            };
            dbContext.CharacterEquipmentCaches.Add(newCache);
            dbContext.SaveChanges();

            try
            {
                var result = warcraftClient.GetCharacterEquipmentSummaryAsync(character.Realm, character.Name.ToLower(), "profile-eu").Result;
                var summaryResult = warcraftClient.GetCharacterProfileSummaryAsync(character.Realm, character.Name.ToLower(), "profile-eu").Result;
                var equipment = result.Value.EquippedItems;

                List<ItemCache> equipedItems = equipment.Select(x => mapEquippedItems(x)).ToList();

                newCache.Class = summaryResult.Value.CharacterClass.Name;
                newCache.Race = summaryResult.Value.Race.Name;
                newCache.Level = summaryResult.Value.Level;
                newCache.Items = equipedItems;
                newCache.AverageItemLevel = summaryResult.Value.AverageItemLevel;
                newCache.EquippedItemLevel = summaryResult.Value.EquippedItemLevel;

                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                dbContext.CharacterEquipmentCaches.Remove(newCache);
                dbContext.SaveChanges();
            }


            return newCache;
        }

        private ItemCache mapEquippedItems(EquippedItem item)
        {
            return new ItemCache
            {
                Slot = item.Slot.Name,
                Quality = item.Quality.Name,
                Level = item.Level.Value.ToInt()
            };
        }

        public IList<CharacterEquipmentCache> GetAllCharactersAndItems()
        {
            var characters = GetAllCharacters();
            var characterItems = new List<CharacterEquipmentCache>();

            foreach (var character in characters)
            {
                characterItems.Add(GetCharacterItems(character.Id));
            }

            return characterItems;

        }

        public void RemoveCharacter(int id)
        {
            var character = dbContext.Characters.FirstOrDefault(x => x.Id == id);
            character.Removed = true;
            dbContext.SaveChanges();
        }
    }
}
