using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Database.Models
{
    public class CharacterEquipmentCache
    {
        public int Id { get; set; }
        public Character Character { get; set; }
        public DateTime CacheTime { get; internal set; }
        public int AverageItemLevel { get; set; }
        public int EquippedItemLevel { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }
        public int Level { get; internal set; }
        public List<ItemCache> Items { get; set; }

        [NotMapped]
        public bool OldCache { get; internal set; }

        public override bool Equals(object obj)
        {
            if(obj is CharacterEquipmentCache)
            {
                var cache = (CharacterEquipmentCache)obj;
                return AverageItemLevel == cache.AverageItemLevel &&
                   EquippedItemLevel == cache.EquippedItemLevel &&
                   Class == cache.Class &&
                   Race == cache.Race &&
                   Level == cache.Level &&
                   Items.All(x => x.Equals(cache.Items.Single(y => y.Slot == x.Slot)));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AverageItemLevel, EquippedItemLevel, Class, Race, Level, Items);
        }
    }
}
