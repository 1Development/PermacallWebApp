using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallTools.Models.IncrementalGame
{
    public class IncrementalPlayer
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Identifier { get; set; }
        public Dictionary<string,int> Buildings { get; set; }
        public Dictionary<string,int> Upgrades { get; set; }
        public string GroupCode { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}