using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using PermacallWebApp.Models.ReturnModels;
using PCDataDLL;

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
            var result = DB.MainDB.GetOneResultQuery("SELECT ID, OPERATORCOUNT, USERNAME, PERMISSION FROM ACCOUNT WHERE SESSIONKEY = ?", parameters);

            if (result != null)
            {
                if (result.Get("ID") != null)
                {
                    User.PermissionGroup permissionGroup;
                    Enum.TryParse(result.Get("PERMISSION"), out permissionGroup);
                    return new User(result.Get("ID").ToInt(),
                        result.Get("OPERATORCOUNT").ToInt(),
                        result.Get("USERNAME").ToString(),
                        permissionGroup);
                }
                else
                    return new User(0, 0, "NOSESSION", User.PermissionGroup.GUEST);

            }
            return new User(-1, 0, "NOCONNECTION", User.PermissionGroup.GUEST);
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

        public static User GetAllUsers()
        {
            var result = DB.MainDB.GetMultipleResultsQuery("SELECT ID, OPERATORCOUNT, USERNAME, PERMISSION FROM ACCOUNT", null);

            if (result != null)
            {
                List<User> returnList = new List<User>();
                foreach (var row in result)
                {
                    User.PermissionGroup permissionGroup;
                    Enum.TryParse(row.Get("PERMISSION"), out permissionGroup);
                    returnList.Add(new User(row.Get("ID").ToInt(),
                        row.Get("OPERATORCOUNT").ToInt(),
                        row.Get("USERNAME"),
                        permissionGroup));
                }

            }
            return new User(-1, 0, "NOCONNECTION", User.PermissionGroup.GUEST);
        }

        public static bool UpdateAccount(User toEdit)
        {
            string sql = "UPDATE ACCOUT SET PERMISSION=?, OPERATORCOUNT=? WHERE ID=?";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"permission", toEdit.Permission.ToString()},
                {"password", toEdit.OperatorCount.ToString()},
                {"id", toEdit.ID.ToString()}
            };
            var result = DB.MainDB.InsertQuery(sql, parameters);

            return result;
        }


    }
}