using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallTools.Models.IncrementalGame
{
    public class DataPair
    {
        public DataPair()
        {
        }

        public DataPair(string playerId, string playerKey, string data)
        {
            this.playerID = playerId;
            this.data = data;
        }

        public string playerID { get; set; }
        public string playerKey { get; set; }
        public string data { get; set; }
    }
}