using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PermacallWebApp.Models
{
    public class User
    {
        //public enum PermissionGroup
        //{
        //    GUEST,
        //    USER,
        //    OPERATOR,
        //    ADMIN
        //}

        public User()
        {
            TSUsers = new List<TSUser>();
        }

        public User(int id, int normalCount, int operatorCount, string username, int strikes, PCAuthLib.User.PermissionGroup permission)
        {
            ID = id;
            Username = username;
            OperatorCount = operatorCount;
            NormalCount = normalCount;
            Strikes = strikes;
            this.Permission = permission;
            TSUsers = new List<TSUser>();
        }


        public int ID { get; set; }
        public string Username { get; set; }
        [DisplayName("Operator Count")]
        public int OperatorCount { get; set; }
        [DisplayName("Normal User Count")]
        public int NormalCount { get; set; }
        [DisplayName("Permission Rank")]
        public PCAuthLib.User.PermissionGroup Permission { get; set; }

        public int Strikes { get; set; }
        public DateTime LastStrike { get; set; }
        public bool hasBeenStriked { get; set; }
        public bool toEdit { get; set; }


        public List<TSUser> TSUsers { get; set; }
    }
}