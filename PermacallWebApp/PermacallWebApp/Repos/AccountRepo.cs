using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;
using PermacallWebApp.Models;
using TS3QueryLib.Core;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Entities;

namespace PermacallWebApp.Repos
{
    public class AccountRepo
    {
        private static DateTime lastCheck;
        public static Tuple<bool, string> GetSalt(string username)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"username", username.ToLower()}
            };
            var result = PCDataDLL.DB.MainDB.GetOneResultQuery("SELECT SALT FROM ACCOUNT WHERE LOWER(USERNAME) = ? AND ENABLED=1", parameters);

            if (result != null && result.Get("SALT") != null)
                return new Tuple<bool, string>(true, result.Get("SALT"));
            if (result != null)
                return new Tuple<bool, string>(false, "NO_SALT");


            return new Tuple<bool, string>(false, "NOCONNECTION");
        }

        public static bool CheckAvailable(string username)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"username", username.ToLower()}
            };
            var result = PCDataDLL.DB.MainDB.CheckExist("SELECT SALT FROM ACCOUNT WHERE LOWER(USERNAME) = ?", parameters);
            if (!result) return true;
            return false;
        }

        public static Tuple<bool, string> ValidateCredentials(string username, string password)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"username", username.ToLower()},
                {"password", password}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID FROM ACCOUNT WHERE LOWER(USERNAME) = ? AND PASSWORD = ? AND ENABLED=1", parameters);

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
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"sessionKey", sessionKey},
                {"username", username.ToLower()}
            };
            var result = DB.MainDB.UpdateQuery("UPDATE ACCOUNT SET SESSIONKEY=? WHERE LOWER(USERNAME) = ?", parameters);

            return result;
        }

        public static User GetUser(string sessionKey)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"sessionkey", sessionKey}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID, NORMALCOUNT, OPERATORCOUNT, USERNAME, STRIKES, PERMISSION, LASTSTRIKE FROM ACCOUNT WHERE SESSIONKEY = ? AND ENABLED=1", parameters);

            if (result != null)
            {
                if (result.Get("ID") != null)
                {
                    PCAuthLib.User.PermissionGroup permissionGroup;
                    Enum.TryParse(result.Get("PERMISSION"), out permissionGroup);
                    DateTime lastStrike = DateTime.Now;
                    DateTime.TryParse(result.Get("LASTSTRIKE"), out lastStrike);
                    return new User(result.Get("ID").ToInt(),
                        result.Get("NORMALCOUNT").ToInt(),
                        result.Get("OPERATORCOUNT").ToInt(),
                        result.Get("USERNAME").ToString(),
                        result.Get("STRIKES").ToInt(),
                        permissionGroup)
                    {
                        LastStrike = lastStrike
                    };
                }
                else
                    return new User(0, 0, 0, "NOSESSION", 0, PCAuthLib.User.PermissionGroup.GUEST);

            }
            return new User(-1, 0, 0, "NOCONNECTION", 0, PCAuthLib.User.PermissionGroup.GUEST);
        }

        public static User GetUser(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID, NORMALCOUNT, OPERATORCOUNT, USERNAME, STRIKES, LASTSTRIKE, PERMISSION FROM ACCOUNT WHERE ID = ? AND ENABLED=1", parameters);

            if (result != null)
            {
                if (result.Get("ID") != null)
                {
                    PCAuthLib.User.PermissionGroup permissionGroup;
                    Enum.TryParse(result.Get("PERMISSION"), out permissionGroup);
                    DateTime lastStrike = DateTime.Now;
                    DateTime.TryParse(result.Get("LASTSTRIKE"), out lastStrike);
                    return new User(result.Get("ID").ToInt(),
                        result.Get("NORMALCOUNT").ToInt(),
                        result.Get("OPERATORCOUNT").ToInt(),
                        result.Get("USERNAME").ToString(),
                        result.Get("STRIKES").ToInt(),
                        permissionGroup)
                    {
                        LastStrike = lastStrike
                    };
                }
                else
                    return new User(0, 0, 0, "NOSESSION", 0, PCAuthLib.User.PermissionGroup.GUEST);

            }
            return new User(-1, 0, 0, "NOCONNECTION", 0, PCAuthLib.User.PermissionGroup.GUEST);
        }

        public static bool InsertNewAccount(string username, string password, string salt)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
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
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT ID, NORMALCOUNT, OPERATORCOUNT, USERNAME, STRIKES, LASTSTRIKE, PERMISSION FROM ACCOUNT WHERE ENABLED=1", null);

            if (result != null)
            {
                List<User> returnList = new List<User>();

                foreach (var row in result)
                {
                    PCAuthLib.User.PermissionGroup permissionGroup;
                    Enum.TryParse(row.Get("PERMISSION"), out permissionGroup);
                    DateTime lastStrike = DateTime.Now;
                    DateTime.TryParse(row.Get("LASTSTRIKE"), out lastStrike);
                    returnList.Add(new User(row.Get("ID").ToInt(),
                        row.Get("NORMALCOUNT").ToInt(),
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
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"permission", toEdit.Permission.ToString()},
                {"operatorCount", toEdit.OperatorCount.ToString()},
                {"strikes", toEdit.Strikes.ToString()},
                {"id", toEdit.ID.ToString()}
            };
            var result = DB.MainDB.InsertQuery(sql, parameters);

            return result;
        }
        public static bool SetAccountOperatorCount(int UserID, int operatorCount = 0, int normalCount = 0)
        {
            string sql = "UPDATE ACCOUNT SET OPERATORCOUNT=?, NORMALCOUNT=? WHERE ID=?";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"operatorCount", operatorCount},
                {"normalCount", normalCount},
                {"id", UserID}
            };
            var result = DB.MainDB.InsertQuery(sql, parameters);

            return result;
        }

        public static bool DeleteAccount(int accountID)
        {
            string Accountsql = "UPDATE ACCOUNT SET ENABLED=0 WHERE ID=?";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", accountID.ToString()},
            };
            var result = DB.MainDB.UpdateQuery(Accountsql, parameters);

            var tsUsers = TeamspeakUserRepo.GetTeamspeakUsers(accountID);

            string teamspeakSQL = "UPDATE TEAMSPEAKUSER SET ENABLED = 0 WHERE ACCOUNTID=?";

            var result2 = DB.MainDB.UpdateQuery(teamspeakSQL, parameters);

            return result && result2;
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
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"strikes", (toStrikeUser.Strikes).ToString()},
                {"id", toStrikeUser.ID.ToString()}
            };
            var accountUpdateResult = DB.MainDB.UpdateQuery(sql, parameters);
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
                        var foundClient =
                            AllClients.FirstOrDefault(x => x.ClientDatabaseId == tsUser.TeamspeakDBID.ToUInt());

                        var uniqueClient = queryRunner.GetClientNameAndUniqueIdByClientDatabaseId(tsUser.TeamspeakDBID.ToUInt());
                        if (uniqueClient.ClientUniqueId != null && banMinutes > 0)
                            queryRunner.AddBanRule(null, null, uniqueClient.ClientUniqueId, new TimeSpan(0, 0, banMinutes, 0, 0),
                                "You have been banned for " + banMinutes + " for having " + toStrikeUser.Strikes +
                                    " strikes.");
                        if (banMinutes > 0 && foundClient != null)
                        {
                            queryRunner.PokeClient(foundClient.ClientId,
                                "You have been banned for " + banMinutes + " for having " + toStrikeUser.Strikes +
                                " strikes.");
                            queryRunner.BanClient(foundClient.ClientId, new TimeSpan(0, 0, banMinutes, 0, 0),
                                "You have been banned for " + banMinutes + " for having " + toStrikeUser.Strikes +
                                " strikes.");
                        }
                        else if (foundClient != null)
                        {
                            queryRunner.PokeClient(foundClient.ClientId,
                            "You have received a strike, you now have " + toStrikeUser.Strikes + " strikes.");
                            queryRunner.MoveClient(foundClient.ClientId, 7);
                        }
                    }
                }
                queryRunner.Logout();
            }
            return true;
        }

        public static bool StrikeReductionCheck()
        {
            if (!(lastCheck == null || lastCheck < DateTime.Now.AddDays(-1)))
                return false;
            List<User> userList = GetAllUsers();
            List<TSUser> tsUserList = TeamspeakUserRepo.GetAllTSUsers();
            List<TSUser> toEditTSUsers = new List<TSUser>();
            if (tsUserList == null || userList == null) return false;
            string sql = "UPDATE ACCOUNT SET LASTSTRIKE=?, STRIKES=? WHERE ID=?";
            if (userList != null)
                foreach (User user in userList)
                {
                    if (user.Strikes > 0 && user.LastStrike <= DateTime.Now.AddDays(-30))
                    {
                        user.Strikes--;
                        user.TSUsers.AddRange(tsUserList.FindAll(x => x.AccountID == user.ID));
                        foreach (TSUser tsUser in user.TSUsers)
                        {
                            tsUser.account = user;
                            toEditTSUsers.Add(tsUser);
                        }
                        Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                        {"laststrike", DateTime.Now.AddDays(-23)},
                        {"strikes", user.Strikes},
                        {"id", user.ID}
                    };
                        DB.MainDB.UpdateQuery(sql, parameters);
                    }
                }

            using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
            {
                queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                queryRunner.SelectVirtualServerById(1);
                queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                { // REAL EXCECUTED CODE
                    var AllClients = queryRunner.GetClientList();
                    foreach (TSUser tsUser in toEditTSUsers)
                    {
                        var foundClient =
                            AllClients.FirstOrDefault(x => x.ClientDatabaseId == tsUser.TeamspeakDBID.ToUInt());

                        if (foundClient != null)
                        {
                            queryRunner.SendTextMessage(MessageTarget.Client, foundClient.ClientId,
                                "You have had one strike removed, you now have: " + tsUser.account.Strikes + " strikes left");
                        }
                    }
                }
                queryRunner.Logout();
            }

            lastCheck = DateTime.Now;
            return true;
        }

        public static bool SetStrikeCount(int UserID, int strikes)
        {
            User user = GetUser(UserID);
            List<TSUser> tsUserList = TeamspeakUserRepo.GetAllTSUsers();
            if (tsUserList == null || user == null) return false;

            if (user.Strikes != strikes)
            {
                user.Strikes = strikes;
                user.TSUsers.AddRange(tsUserList.FindAll(x => x.AccountID == user.ID));
                string sql = "UPDATE ACCOUNT SET STRIKES=? WHERE ID=?";
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"strikes", user.Strikes},
                    {"id", user.ID}
                };
                DB.MainDB.UpdateQuery(sql, parameters);


                using (QueryRunner queryRunner = new QueryRunner(new SyncTcpDispatcher("127.0.0.1", 10011)))
                {
                    queryRunner.Login(SecureData.ServerUsername, SecureData.ServerPassword).GetDumpString();
                    queryRunner.SelectVirtualServerById(1);
                    queryRunner.UpdateCurrentQueryClient(new ClientModification { Nickname = "PermacallWebApp" });

                    {
                        // REAL EXCECUTED CODE
                        var AllClients = queryRunner.GetClientList();
                        foreach (TSUser tsUser in user.TSUsers)
                        {
                            var foundClient =
                                AllClients.FirstOrDefault(x => x.ClientDatabaseId == tsUser.TeamspeakDBID.ToUInt());

                            if (foundClient != null)
                            {
                                queryRunner.SendTextMessage(MessageTarget.Client, foundClient.ClientId,
                                    "You had your strikes changed, you now have: " + user.Strikes + " strikes");
                            }
                        }
                    }
                    queryRunner.Logout();
                }
                return true;
            }
            return false;
        }


    }
}