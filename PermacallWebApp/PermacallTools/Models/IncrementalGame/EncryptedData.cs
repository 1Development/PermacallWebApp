using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallTools.Models.IncrementalGame
{
    public class EncryptedData
    {
        public EncryptedData(string key, string data)
        {
            this.key = key;
            this.data = data;
        }

        public string key { get; set; }
        public string data { get; set; }
    }
}