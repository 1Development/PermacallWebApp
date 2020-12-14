using ArgentPonyWarcraftClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using PCDataDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tools.Database;
using Tools.Database.Models;
using Tools.Models;

namespace Tools.Services
{
    public class WowToolsService : IWowToolsService
    {
        private const int characterCachetime = 5;
        private WarcraftClient warcraftClient;
        private ToolContext dbContext;
        private static Semaphore wowApiPool;
        private readonly ILogger<WowToolsService> logger;

        public WowToolsService(ToolContext dbContext, ILogger<WowToolsService> logger)
        {
            if (wowApiPool == null)
            {
                wowApiPool = new Semaphore(1, 1);
            }

            this.dbContext = dbContext;
            this.logger = logger;

            var clientId = SecureData.BlizzardApiClientId;
            var clientSecret = SecureData.BlizzardApiClientSecret;

            warcraftClient = new WarcraftClient(clientId, clientSecret, Region.Europe, Locale.en_GB);
        }

        public IList<CharacterEquipmentCache> GetAllCharacterItems(IList<Character> characters)
        {
            throw new NotImplementedException();
        }

        public string AddCharacter(AddCharacterModel addCharacter)
        {
            var result = warcraftClient.GetCharacterEquipmentSummaryAsync(addCharacter.Realm.ToLower().Replace(" ", "-"), addCharacter.Name.ToLower(), "profile-eu").Result;

            if (result.Success)
            {
                var character = result.Value.Character;

                var existingCharacter = dbContext.Characters.FirstOrDefault(x => x.Name == character.Name && x.Realm == character.Realm.Slug && !x.Removed);
                if (existingCharacter != null)
                {
                    if (!String.IsNullOrWhiteSpace(addCharacter.Player) &&
                        (existingCharacter.IsMain != addCharacter.IsMain ||
                        !String.Equals(addCharacter.Player, existingCharacter.PlayerName, StringComparison.InvariantCultureIgnoreCase
                        )))
                    {
                        existingCharacter.PlayerName = addCharacter.Player;
                        existingCharacter.IsMain = addCharacter.IsMain;
                        dbContext.SaveChanges();
                        return "Character Edited";
                    }
                    return "Character already exists";
                }

                dbContext.Characters.Add(new Character
                {
                    PlayerName = addCharacter.Player,
                    Name = character.Name,
                    Realm = character.Realm.Slug,
                    IsMain = addCharacter.IsMain,
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

        public CharacterEquipmentCache GetCharacterItems(int characterId, bool updateCache)
        {
            String.Join(":", null ?? new int[0]);
            Character character = dbContext.Characters.SingleOrDefault(x => x.Id == characterId);

            if (character == null) return null;

            var cutoffTime = DateTime.Now.AddMinutes(-characterCachetime);

            var oldCache = dbContext.CharacterEquipmentCaches
                    .Include(x => x.Items)
                    .OrderByDescending(x => x.CacheTime)
                    .Where(x => x.Character == character && x.Class != null)
                    .FirstOrDefault();

            if (!updateCache)
            {
                if (oldCache != null)
                {
                    oldCache.OldCache = oldCache.CacheTime < cutoffTime;
                    return oldCache;
                }
            }

            //var cache = dbContext.CharacterEquipmentCaches
            //    .Include(x => x.Items)
            //    .Where(x => x.Character == character && x.CacheTime > cutoffTime)
            //    .FirstOrDefault();

            if (oldCache != null && oldCache.CacheTime > cutoffTime)
            {
                if (oldCache.Class == null && oldCache.CacheTime < DateTime.Now.AddSeconds(-20))
                {
                    dbContext.CharacterEquipmentCaches.Remove(oldCache);
                    dbContext.SaveChanges();
                }
                else
                {
                    return oldCache;
                }
            }

            var newCache = new CharacterEquipmentCache
            {
                CacheTime = DateTime.Now,
                Character = character
            };

            RequestResult<CharacterEquipmentSummary> result = null;
            RequestResult<CharacterProfileSummary> summaryResult = null;

            try
            {
                if (wowApiPool.WaitOne(1000))
                {
                    try
                    {
                        dbContext.CharacterEquipmentCaches.Add(newCache);
                        dbContext.SaveChanges();

                        result = warcraftClient.GetCharacterEquipmentSummaryAsync(character.Realm, character.Name.ToLower(), "profile-eu").Result;
                        summaryResult = warcraftClient.GetCharacterProfileSummaryAsync(character.Realm, character.Name.ToLower(), "profile-eu").Result;
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, e.Message);
                    }
                    finally
                    {
                        wowApiPool.Release();
                    }
                }
                else
                {
                    newCache.OldCache = true;
                    return newCache;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            try
            {
                var equipment = result.Value.EquippedItems;
                List<ItemCache> equipedItems = equipment.Select(x => mapEquippedItems(x)).ToList();

                newCache.Class = summaryResult.Value.CharacterClass.Name;
                newCache.Race = summaryResult.Value.Race.Name;
                newCache.Level = summaryResult.Value.Level;
                newCache.Items = equipedItems;
                newCache.AverageItemLevel = summaryResult.Value.AverageItemLevel;
                newCache.EquippedItemLevel = summaryResult.Value.EquippedItemLevel;

                if (newCache.Equals(oldCache))
                {
                    dbContext.CharacterEquipmentCaches.Remove(newCache);
                    oldCache.CacheTime = DateTime.Now;
                    dbContext.SaveChanges();
                }
                else
                {
                    dbContext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                dbContext.CharacterEquipmentCaches.Remove(newCache);
                dbContext.SaveChanges();
                logger.LogError(e, e.Message);
            }


            return newCache;
        }

        private ItemCache mapEquippedItems(EquippedItem item)
        {
            var gems = item.Sockets?
                .Where(x => x.Item != null)
                .Select(x => x.Item.Id)
                .ToArray();
            var enchants = item.Enchantments?
                .Where(x => x.enchantment_id != 0)
                .Select(x => x.enchantment_id)
                .ToArray();

            return new ItemCache
            {
                Slot = item.Slot.Name,
                Quality = item.Quality.Name,
                Level = item.Level.Value.ToInt(),
                ItemId = item.Item.Id,
                Bonus = String.Join(":", item.BonusList ?? new int[0]),
                Gems = String.Join(":", gems ?? new int[0]),
                Enchants = String.Join(":", enchants ?? new int[0])
            };
        }

        public IList<CharacterEquipmentCache> GetAllCharactersAndItems()
        {
            var characters = GetAllCharacters();
            var characterItems = new List<CharacterEquipmentCache>();

            foreach (var character in characters)
            {
                characterItems.Add(GetCharacterItems(character.Id, true));
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
