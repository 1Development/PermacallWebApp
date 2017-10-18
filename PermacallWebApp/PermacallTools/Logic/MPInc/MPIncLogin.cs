using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using PermacallTools.Models.IncrementalGame;

namespace PermacallTools.Logic.MPInc
{
    public class MPIncLogin
    {
        public static IncrementalPlayer GetCurrentUser(HttpContext context)
        {
            string sessionKey;
            if (!String.IsNullOrEmpty(context.Request.Cookies["SessionData"]?["SessionKey"]))
            {
                sessionKey = context.Request.Cookies["SessionData"]["SessionKey"];
            }
            else sessionKey = "nothing";


            var sessionRe = AccountRepo.GetUser(sessionKey);
            if (sessionRe.ID > 0)
            {
                CacheUserName(context.Request.UserHostAddress, sessionRe.Username);

                context.Response.Cookies["SessionData"]["SessionKey"] = sessionKey;
                context.Response.Cookies["SessionData"].Expires = DateTime.Now.AddHours(12);

                return sessionRe;
            }
            return sessionRe;
        }

        public static Tuple<bool, string> AuthorizeUser(HttpContext context, string username, string password)
        {
            var saltRe = AccountRepo.GetSalt(username);
            if (!saltRe.Item1)
            {
                if (saltRe.Item2 == "NOCONNECTION") return new Tuple<bool, string>(false, "Connection to the database could not be established");
                return new Tuple<bool, string>(false, "Username not found!");
            }
            var authRe = AccountRepo.ValidateCredentials(username, Encrypt(password, saltRe.Item2));
            if (!authRe.Item1)
            {
                if (saltRe.Item2 == "NOCONNECTION") return new Tuple<bool, string>(false, "Connection to the database could not be established");
                return new Tuple<bool, string>(false, "Username/Password combination incorrect!");
            }

            string sessionKey = GenerateRandomString(authRe.Item2.ToInt(), 64);
            AccountRepo.SetSessionKey(username, sessionKey);

            context.Response.Cookies["SessionData"]["SessionKey"] = sessionKey;
            context.Response.Cookies["SessionData"].Expires = DateTime.Now.AddHours(12);

            return new Tuple<bool, string>(true, "Login Succesfull");

        }
    }
}