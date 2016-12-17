using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallWebApp.Models.ReturnModels
{
    public class User
    {
        public enum PermissionGroup
        {
            GUEST,
            USER,
            ADMIN
        }

        public User(int id, int operatorCount, string username, PermissionGroup permission)
        {
            ID = id;
            Username = username;
            OperatorCount = operatorCount;
            this.Permission = permission;
        }

        public int ID { get; set; }
        public string Username { get; set; }
        public int OperatorCount { get; set; }
        public PermissionGroup Permission { get; set; }


        public List<TSUser> TSUsers { get; set; }
    }
}