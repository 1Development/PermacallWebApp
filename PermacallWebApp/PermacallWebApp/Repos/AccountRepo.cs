using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PermacallWebApp.Models.ReturnModels;
using PCDataDLL;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Entities;

namespace PermacallWebApp.Repos
{
    public class AccountRepo
    {
        public static Tuple<bool, string> GetSalt(string username)
        {
            GetAllUsers(); //TODO : REMOVE THIS
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"username", username.ToLower()}
            };
            var result = PCDataDLL.DB.MainDB.GetOneResultQuery("SELECT SALT FROM ACCOUNT WHERE LOWER(USERNAME) = ?", parameters);

            if (result != null && result.Get("SALT") != null)
                return new Tuple<bool, string>(true, result.Get("SALT"));
            if (result != null)
                return new Tuple<bool, string>(false, "NO_SALT");


            return new Tuple<bool, string>(false, "NOCONNECTION");
        }

        public static bool CheckAvailable(string username)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"username", username.ToLower()}
            };
            var result = PCDataDLL.DB.MainDB.CheckExist("SELECT SALT FROM ACCOUNT WHERE LOWER(USERNAME) = ?", parameters);
            if (!result) return true;
            return false;
        }

        public static Tuple<bool, string> ValidateCredentials(string username, string password)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"username", username.ToLower()},
                {"password", password}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID FROM ACCOUNT WHERE LOWER(USERNAME) = ? AND PASSWORD = ?", parameters);

            if (result != null)
            {
                if (result.Get("ID") != null)
                    return new Tuple<bool, string>(true, result.Get("ID"));
                return new Tuple<bool, string>(false, "NOTCORRECT");
            }
            return new Tuple<bool, string>(false, "NOCONNECTION");
        }

        public static bool SetSessionKey(string username, string sessionKey)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"sessionKey", sessionKey},
                {"username", username.ToLower()}
            };
            var result = DB.MainDB.UpdateQuery("UPDATE ACCOUNT SET SESSIONKEY=? WHERE LOWER(USERNAME) = ?", parameters);

            return result;
        }

        public static User GetUser(string sessionKey)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"sessionkey", sessionKey}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID, OPERATORCOUNT, USERNAME, STRIKES, PERMISSION FROM ACCOUNT WHERE SESSIONKEY = ?", parameters);

            if (result != null)
            {
                if (result.Get("ID") != null)
                {
                    User.PermissionGroup permissionGroup;
                    Enum.TryParse(result.Get("PERMISSION"), out permissionGroup);
                    return new User(result.Get("ID").ToInt(),
                        result.Get("OPERATORCOUNT").ToInt(),
                        result.Get("USERNAME").ToString(),
                        result.Get("STRIKES").ToInt(),
                        permissionGroup);
                }
                else
                    return new User(0, 0, "NOSESSION", 0, User.PermissionGroup.GUEST);

            }
            return new User(-1, 0, "NOCONNECTION", 0, User.PermissionGroup.GUEST);
        }

        public static User GetUser(int id)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID, OPERATORCOUNT, USERNAME, STRIKES, LASTSTRIKE, PERMISSION FROM ACCOUNT WHERE ID = ?", parameters);

            if (result != null)
            {
                if (result.Get("ID") != null)
                {
                    User.PermissionGroup permissionGroup;
                    Enum.TryParse(result.Get("PERMISSION"), out permissionGroup);
                    DateTime lastStrike = DateTime.Now;
                    DateTime.TryParse(result.Get("LASTSTRIKE"), out lastStrike);
                    return new User(result.Get("ID").ToInt(),
                        result.Get("OPERATORCOUNT").ToInt(),
                        result.Get("USERNAME").ToString(),
                        result.Get("STRIKES").ToInt(),
                        permissionGroup)
                    {
                        LastStrike = lastStrike
                    };
                }
                else
                    return new User(0, 0, "NOSESSION", 0, User.PermissionGroup.GUEST);

            }
            return new User(-1, 0, "NOCONNECTION", 0, User.PermissionGroup.GUEST);
        }

        public static bool InsertNewAccount(string username, string password, string salt)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"username", username.ToLower()},
                {"password", password},
                {"salt", salt }
            };
            var result = DB.MainDB.InsertQuery("INSERT INTO ACCOUNT(USERNAME, PASSWORD, SALT) VALUES (?, ?, ?)", parameters);

            return result;
        }

        public static List<User> GetAllUsers()
        {
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT ID, OPERATORCOUNT, USERNAME, STRIKES, LASTSTRIKE, PERMISSION FROM ACCOUNT", null);

            if (result != null)
            {
                List<User> returnList = new List<User>();

                foreach (var row in result)
                {
                    User.PermissionGroup permissionGroup;
                    Enum.TryParse(row.Get("PERMISSION"), out permissionGroup);
                    DateTime lastStrike = DateTime.Now;
                    DateTime.TryParse(row.Get("LASTSTRIKE"), out lastStrike);
                    returnList.Add(new User(row.Get("ID").ToInt(),
                        row.Get("OPERATORCOUNT").ToInt(),
                        row.Get("USERNAME"),
                        row.Get("STRIKES").ToInt(),
                        permissionGroup)
                    {
                        LastStrike = lastStrike
                    });
                }
                return returnList;
            }
            return null;
        }

        public static bool UpdateAccount(User toEdit)
        {
            string sql = "UPDATE ACCOUNT SET PERMISSION=?, OPERATORCOUNT=?, STRIKES=? WHERE ID=?";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"permission", toEdit.Permission.ToString()},
                {"operatorCount", toEdit.OperatorCount.ToString()},
                {"strikes", toEdit.Strikes.ToString()},
                {"id", toEdit.ID.ToString()}
            };
            var result = DB.MainDB.InsertQuery(sql, parameters);

            return result;
        }

        public static bool StrikeUser(int userID)
        {
            User toStrikeUser = GetUser(userID);
            if (toStrikeUser.Strikes - 3 >= 0 &&
                toStrikeUser.LastStrike >
                DateTime.Now.AddMinutes(-15 * Math.Pow(2, toStrikeUser.Strikes - 3.0).ToInt()))
                return false;
            toStrikeUser.TSUsers = TeamspeakUserRepo.GetTeamspeakUsers(userID);
            toStrikeUser.Strikes += 1;
            string sql = "UPDATE ACCOUNT SET LASTSTRIKE=CURRENT_TIMESTAMP, STRIKES=? WHERE ID=?";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"strikes", (toStrikeUser.Strikes).ToString()},
                {"id", toStrikeUser.ID.ToString()}
            };
            //var accountUpdateResult = DB.MainDB.UpdateQuery(sql, parameters);
            int banMinutes = -1;
            if (toStrikeUser.Strikes - 3 >= 0)
                banMinutes = 15 * Math.Pow(2, toStrikeUser.Strikes - 3.0).ToInt();

            using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
            {
                queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                queryRunner.SelectVirtualServerById(1);
                queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                { // REAL EXCECUTED CODE
                    var AllClients = queryRunner.GetClientList();
                    foreach (TSUser tsUser in toStrikeUser.TSUsers)
                    {
                        var resultClient = queryRunner.EditDatbaseClient(tsUser.TeamspeakDBID.ToInt(),
                            new ClientModification { Description = "TESTOMGGG" });


                        var foundClient =
                            AllClients.FirstOrDefault(x => x.ClientDatabaseId == tsUser.TeamspeakDBID.ToUInt());

                        //queryRunner.AddBanRule(null,null,queryRunner.GetClientNameAndUniqueIdByClientDatabaseId(tsUser.ToUInt()).ClientUniqueId)


                        queryRunner.EditDatbaseClient(tsUser.TeamspeakDBID.ToInt(),
                            new ClientModification() { Description = "Strikes : " + toStrikeUser.Strikes });
                        if (banMinutes > 0)
                        {
                            if (foundClient != null)
                                queryRunner.PokeClient(tsUser.TeamspeakDBID.ToUInt(),
                                "You have been banned for " + banMinutes + " for having " + toStrikeUser.Strikes +
                                " strikes.");
                            queryRunner.BanClient(tsUser.TeamspeakDBID.ToUInt(), new TimeSpan(0, 0, banMinutes, 0, 0));
                        }
                        else
                        {
                            if (foundClient != null)
                                queryRunner.PokeClient(tsUser.TeamspeakDBID.ToUInt(),
                                "You have received a strike, you now have " + toStrikeUser.Strikes + " strikes.");
                        }
                    }
                    if (banMinutes > 0)
                        queryRunner.SendGlobalMessage(toStrikeUser.Username + " has been banned for " + banMinutes +
                                                             " for having " + toStrikeUser.Strikes + " strikes");
                }
                queryRunner.Logout();
            }


            return false;

        }


    }
}