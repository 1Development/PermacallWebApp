using System;
using System.Collections.Generic;
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
        public List<ItemCache> Items { get; set; }
    }
}
