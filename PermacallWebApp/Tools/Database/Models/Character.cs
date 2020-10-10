using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Database.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlayerName { get; set; }
        public string Realm { get; set; }
        public DateTime AddedTime { get; set; }
        public bool Removed { get; set; }
    }
}
