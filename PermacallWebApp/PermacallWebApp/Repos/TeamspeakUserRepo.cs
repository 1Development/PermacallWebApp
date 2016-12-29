using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PCDataDLL;
using PermacallWebApp.Models.ReturnModels;

namespace PermacallWebApp.Repos
{
    public class TeamspeakUserRepo
    {
        public static List<TSUser> GetTeamspeakUsers(int accountID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"accountid", accountID.ToString() }
            };
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT TEAMSPEAKDBID, NICKNAME, RANK FROM TEAMSPEAKUSER WHERE ACCOUNTID = @accountid AND ENABLED = 1", parameters);

            if (result == null)
                return null;

            List<TSUser> returnList = new List<TSUser>();

            foreach (var row in result)
            {
                TSUser.Rank r;
                Enum.TryParse(row.Get("RANK"), out r);
                returnList.Add(new TSUser()
                {
                    AccountID = accountID,
                    NickName = row.Get("NICKNAME"),
                    TeamspeakDBID = row.Get("TEAMSPEAKDBID"),
                    UserRank = r
                });
            }


            return returnList;
        }
        public static bool TSUserAvailable(string DBID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"teamspeakid", DBID }
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT TEAMSPEAKDBID, NICKNAME FROM TEAMSPEAKUSER WHERE TEAMSPEAKDBID = ? AND ENABLED = 1", parameters);
            
            if (result != null && !result.Exists())
                return true;

            return false;
        }


        public static bool AddTeamspeakUserToAccount(TSUser toAddUser)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"teamspeakid", toAddUser.TeamspeakDBID},
                {"accountid", toAddUser.AccountID.ToString()},
                {"nickname", toAddUser.NickName },
                {"rank", toAddUser.UserRank.ToString() }
            };
            var result = DB.MainDB.InsertQuery("INSERT INTO TEAMSPEAKUSER(TEAMSPEAKDBID, ACCOUNTID, NICKNAME, RANK) VALUES(@teamspeakid, @accountid, @nickname, @rank)", parameters);

            return result;
        }

        public static bool UpdateTSUser(string teamspeakid, TSUser editResult)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
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

        public static bool DisableTSUser(string teamspeakDBID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"teamspeakid", teamspeakDBID}
            };
            var result = DB.MainDB.UpdateQuery("UPDATE TEAMSPEAKUSER SET ENABLED = 0 WHERE TEAMSPEAKDBID = ?", parameters);

            return result;
        }

        public static List<TSUser> GetAllTSUsers()
        {
            var result =
                DB.MainDB.GetMultipleResultsQuery(
                    "SELECT teamspeakDBID, AccountID, NickName, Added, Rank FROM TEAMSPEAKUSER WHERE ENABLED = 1", null);
            List<TSUser> returnList = new List<TSUser>();
            if (result != null)
            {
                foreach (DBResult row in result)
                {
                    TSUser.Rank r = TSUser.Rank.NORMAL;
                    Enum.TryParse(row.Get("Rank"), out r);
                    returnList.Add(new TSUser()
                    {
                        NickName = row.Get("NickName"),
                        TeamspeakDBID = row.Get("teamspeakDBID"),
                        added = DateTime.Parse(row.Get("Added")),
                        AccountID = row.Get("AccountID").ToInt(),
                        UserRank = r
                    });
                }
                return returnList;
            }
            return null;
        }
    }
}