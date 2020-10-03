using System;
using System.ComponentModel;

namespace PCAuthLibCore
{
    public class User
    {
        public enum PermissionGroup
        {
            GUEST,
            USER,
            OPERATOR,
            ADMIN
        }

        public User()
        {
        }

        public User(int id, int normalCount, int operatorCount, string username, int strikes, PermissionGroup permission)
        {
            ID = id;
            Username = username;
            OperatorCount = operatorCount;
            NormalCount = normalCount;
            Strikes = strikes;
            this.Permission = permission;
        }


        public int ID { get; set; }
        public string Username { get; set; }
        [DisplayName("Operator Count")]
        public int OperatorCount { get; set; }
        [DisplayName("Normal User Count")]
        public int NormalCount { get; set; }
        [DisplayName("Permission Rank")]
        public PermissionGroup Permission { get; set; }

        public int Strikes { get; set; }
        public DateTime LastStrike { get; set; }
        public bool hasBeenStriked { get; set; }
        public bool toEdit { get; set; }
    }
}