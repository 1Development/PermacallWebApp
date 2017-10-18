using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;

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
            var result = PCDataDLL.DB.MPInc.CheckExist("SELECT SALT FROM ACCOUNT WHERE LOWER(USERNAME) = ?", parameters);
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

        public static bool SetSessionKey(string username, string sessionKey)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"sessionKey", sessionKey},
                {"username", username.ToLower()}
            };
            var result = DB.MPInc.UpdateQuery("UPDATE ACCOUNT SET SESSIONKEY=? WHERE LOWER(USERNAME) = ?", parameters);

            return result;
        }

        public static User GetUser(string sessionKey)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"sessionkey", sessionKey}
            };
            var result = DB.MPInc.GetOneResultQuery("SELECT ID, NORMALCOUNT, OPERATORCOUNT, USERNAME, STRIKES, PERMISSION, LASTSTRIKE FROM ACCOUNT WHERE SESSIONKEY = ? AND ENABLED=1", parameters);

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
    }
}