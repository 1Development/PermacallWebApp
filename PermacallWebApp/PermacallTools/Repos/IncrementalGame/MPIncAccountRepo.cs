using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;
using PermacallTools.Models.IncrementalGame;

namespace PermacallTools.Repos.IncrementalGame
{
    public class MPIncAccountRepo
    {
        public static Tuple<bool, string> GetSalt(string username)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"username", username.ToLower()}
            };
            var result = PCDataDLL.DB.MPInc.GetOneResultQuery("SELECT SALT FROM ACCOUNT WHERE LOWER(USERNAME) = ? AND ENABLED=1", parameters);

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
            var result = PCDataDLL.DB.MPInc.CheckExist("SELECT ID FROM ACCOUNT WHERE LOWER(USERNAME) = ?", parameters);
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
            var result = DB.MPInc.GetOneResultQuery("SELECT ID FROM ACCOUNT WHERE LOWER(USERNAME) = ? AND PASSWORD = ? AND ENABLED=1", parameters);

            if (result != null)
            {
                if (result.Get("ID") != null)
                    return new Tuple<bool, string>(true, result.Get("ID"));
                return new Tuple<bool, string>(false, "NOTCORRECT");
            }
            return new Tuple<bool, string>(false, "NOCONNECTION");
        }

        public static bool SetSessionKey(string id, string sessionKey)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"sessionKey", sessionKey},
                {"id", id}
            };
            var result = DB.MPInc.UpdateQuery("UPDATE ACCOUNT SET SESSIONKEY=? WHERE ID = ?", parameters);

            return result;
        }

        public static IncAccount GetUser(string sessionKey)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"sessionkey", sessionKey}
            };
            var result = DB.MPInc.GetOneResultQuery("SELECT ID, USERNAME FROM ACCOUNT WHERE SESSIONKEY = ? AND ENABLED=1", parameters);

            if (result != null && result.Get("ID") != null)
                return new IncAccount(result.Get("ID"), result.Get("Username"));

            return new IncAccount("0", "null");
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

        public static bool CheckPlayerAccount(string id, string accountID) 
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id},
                {"accountID", accountID}
            };
            return DB.MainDB.CheckExist("SELECT ID FROM Player WHERE ID = ? AND AccountID = ?", parameters);
        }
    }
}