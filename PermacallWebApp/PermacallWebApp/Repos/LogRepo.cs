using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;

namespace PermacallWebApp.Repos
{
    public class LogRepo
    {
        public enum LogCategory
        {
            Request,
            Error
        }
        public static bool Log(string logDestription, LogCategory category, string ip, string username)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"desc", logDestription},
                {"cate", category.ToString()},
                {"ip", ip },
                {"username", username }
            };
            var result = DB.MainDB.InsertQuery("INSERT INTO LOG(DESCRIPTION, CATEGORY, IP, USERNAME) VALUES (?, ?, ?, ?)", parameters);

            return result;
        }
    }
}