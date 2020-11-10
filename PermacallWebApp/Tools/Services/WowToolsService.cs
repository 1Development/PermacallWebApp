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

namespace Tools.Services
{
    public class WowToolsService : IWowToolsService
    {
        private const int characterCachetime = 10;
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

        public CharacterEquipmentCache GetCharacterItems(int characterId, bool updateCache)
        {
            Character character = dbContext.Characters.SingleOrDefault(x => x.Id == characterId);

            if (character == null) return null;

            var cutoffTime = DateTime.Now.AddMinutes(-characterCachetime);

            if (!updateCache)
            {
                var oldCache = dbContext.CharacterEquipmentCaches
                    .Include(x => x.Items)
                    .OrderByDescending(x => x.CacheTime)
                    .Where(x => x.Character == character && x.Class != null)
                    .FirstOrDefault();
                if (oldCache != null)
                {
                    oldCache.OldCache = oldCache.CacheTime < cutoffTime;
                    return oldCache;
                }
            }

            var cache = dbContext.CharacterEquipmentCaches
                .Include(x => x.Items)
                .Where(x => x.Character == character && x.CacheTime > cutoffTime)
                .FirstOrDefault();

            if (cache != null)
            {
                if (cache.Class == null && cache.CacheTime < DateTime.Now.AddSeconds(-20))
                {
                    dbContext.CharacterEquipmentCaches.Remove(cache);
                    dbContext.SaveChanges();
                }
                else
                {
                    return cache;
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

                dbContext.SaveChanges();
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
