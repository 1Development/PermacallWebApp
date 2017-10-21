using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallTools.Models.IncrementalGame
{
    public class IncAccount
    {
        public IncAccount() { }

        public IncAccount(string id, string username)
        {
            ID = id;
            Username = username;
        }
        public string ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}