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
    }
}
