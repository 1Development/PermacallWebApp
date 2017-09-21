using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallTools.Models.IncrementalGame
{
    public class DataPair
    {
        public DataPair(string key, string data)
        {
            this.key = key;
            this.data = data;
        }

        public DataPair(DataPair data, IncrementalPlayer player)
        {
            this.key = data.key;
            this.data = data.data;
            Player = player;
        }

        public string key { get; set; }
        public string data { get; set; }
        public IncrementalPlayer Player { get; set; }
    }
}