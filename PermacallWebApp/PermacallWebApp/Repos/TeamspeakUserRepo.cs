using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PermacallWebApp.Models.ReturnModels;

namespace PermacallWebApp.Repos
{
    public class TeamspeakUserRepo
    {
        public static List<TSUser> GetTeamspeakUsers(int accountID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"accountid", accountID.ToString() }
            };
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT TEAMSPEAKDBID, NICKNAME FROM TEAMSPEAKUSER WHERE ACCOUNTID = @accountid AND ENABLED = 1", parameters);

            if (result == null)
                return null;

            List<TSUser> returnList = new List<TSUser>();

            foreach (var row in result)
            {
                returnList.Add(new TSUser()
                {
                    AccountID = accountID,
                    NickName = row["NICKNAME"],
                    TeamspeakDBID = row["TEAMSPEAKDBID"]
                });
            }


            return returnList;
        }
        public static bool TSUserAvailable(string DBID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"teamspeakid", DBID }
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT TEAMSPEAKDBID, NICKNAME FROM TEAMSPEAKUSER WHERE TEAMSPEAKDBID = ? AND ENABLED = 1", parameters);
            
            if (result != null && result.Count == 0)
                return true;

            return false;
        }


        public static bool AddTeamspeakUserToAccount(TSUser toAddUser)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"teamspeakid", toAddUser.TeamspeakDBID},
                {"accountid", toAddUser.AccountID.ToString()},
                {"nickname", toAddUser.NickName }
            };
            var result = DB.MainDB.InsertQuery("INSERT INTO TEAMSPEAKUSER(TEAMSPEAKDBID, ACCOUNTID, NICKNAME) VALUES(@teamspeakid, @accountid, @nickname)", parameters);

            return result;
        }

        public static bool EditTSUser(string teamspeakid, TSUser editResult)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"newteamspeakid", editResult.TeamspeakDBID},
                {"accountid", editResult.AccountID.ToString() },
                {"nickname", editResult.NickName},
                {"enabled", 1.ToString() },
                {"teamspeakid", teamspeakid}
            };
            var result = DB.MainDB.UpdateQuery("UPDATE TEAMSPEAKUSER SET TEAMSPEAKDBID = ?, ACCOUNTID = ?, NICKNAME = ? WHERE ENABLED = ? AND TEAMSPEAKDBID = ?", parameters);

            return result;
        }

        public static bool DisableTSUser(string teamspeakid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"teamspeakid", teamspeakid}
            };
            var result = DB.MainDB.UpdateQuery("UPDATE TEAMSPEAKUSER SET ENABLED = 0 WHERE TEAMSPEAKDBID = ?", parameters);

            return result;
        }
    }
}