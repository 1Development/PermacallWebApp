using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallWebApp.Models.ReturnModels
{
    public class TSUser
    {
        public enum Rank
        {
            NORMAL, OPERATOR
        }
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
        public User account { get; set; }
        public bool toEdit { get; set; }
        public bool isBot { get; set; }
        public DateTime added { get; set; }
        public Rank UserRank { get; set; }

    }
}