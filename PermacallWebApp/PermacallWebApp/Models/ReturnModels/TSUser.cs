using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallWebApp.Models.ReturnModels
{
    public class TSUser
    {
        public TSUser()
        {
        }
        public TSUser(string teamspeakDBID, string nickName, int accountID, int Enabled = 1)
        {
            NickName = nickName;
            TeamspeakDBID = teamspeakDBID;  
            AccountID = accountID;
            toEdit = false;
        }

        public string NickName { get; set; }
        public string TeamspeakDBID { get; set; }
        public int AccountID { get; set; }
        public bool toEdit { get; set; }

    }
}