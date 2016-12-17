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
        public static Tuple<bool,string> GetSalt(string username)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"username", username.ToLower()}
            };
            var result = PCDataDLL.DB.MainDB.GetOneResultQuery("SELECT SALT FROM ACCOUNT WHERE LOWER(USERNAME) = ?", parameters);

            if (result != null && result.ContainsKey("SALT") && result["SALT"] != null)
                return new Tuple<bool, string>(true, result["SALT"]);
            if(result != null && result.Count==0)
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

        public static Tuple<bool,string> ValidateCredentials(string username, string password)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"username", username.ToLower()},
                {"password", password}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID FROM ACCOUNT WHERE LOWER(USERNAME) = ? AND PASSWORD = ?", parameters);

            if (result != null)
            {
                if (result.Count > 0 && result["ID"] != null)
                    return new Tuple<bool, string>(true, result["ID"]);
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
                if (result.Count > 0)
                    return new User(result["ID"].ToInt(),
                        result["OPERATORCOUNT"].ToInt(),
                        result["USERNAME"].ToString(),
                        (User.PermissionGroup)result["PERMISSION"].ToInt());
                else return new User(0, 0, "NOSESSION", User.PermissionGroup.Guest);
            }
            return new User(-1, 0, "NOCONNECTION", User.PermissionGroup.Guest);
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


    }
}