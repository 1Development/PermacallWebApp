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

        public DataPair(string sessionKey, string data)
        {
            this.SessionKey = sessionKey;
            this.data = data;
        }

        public string SessionKey { get; set; }
        public string data { get; set; }
    }
}