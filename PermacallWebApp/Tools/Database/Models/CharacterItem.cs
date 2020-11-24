using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Database.Models
{
    public class ItemCache
    {
        public int Id { get; set; }

        public string Slot { get; set; }
        public string Quality { get; set; }
        public int Level { get; set; }
        public int ItemId { get; set; }
        public string Bonus { get; set; }
        public string Enchants { get; set; }
        public string Gems { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ItemCache)
            {
                var cache = (ItemCache)obj;
                return Slot == cache.Slot &&
                   Quality == cache.Quality &&
                   Level == cache.Level &&
                   ItemId == cache.ItemId &&
                   Bonus == cache.Bonus &&
                   Enchants == cache.Enchants &&
                   Gems == cache.Gems;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Slot, Quality, Level, ItemId, Bonus, Enchants, Gems);
        }
    }
}
