using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallWebApp.Models.ReturnModels
{
    public class User
    {
        public User(int id, int operatorCount, string username)
        {
            ID = id;
            Username = username;
            OperatorCount = operatorCount;
        }

        public int ID { get; set; }
        public string Username { get; set; }
        public int OperatorCount { get; set; }

        public List<TSUser> TSUsers { get; set; }
    }
}