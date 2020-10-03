using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PCAuthLibCore
{
    public static class Login
    {
        public static User GetCurrentUser(HttpContext context)
        {
            //context.Request.
            string sessionKey;
            if (!String.IsNullOrEmpty(context.Request.Cookies["SessionKey"]))
            {
                sessionKey = context.Request.Cookies["SessionKey"];
            }
            else sessionKey = "nothing";

            var sessionRe = AccountRepo.GetUser(sessionKey);
            if (sessionRe.ID > 0)
            {
                context.Response.Cookies.Append("SessionKey", sessionKey, new CookieOptions() 
                { 
                    Expires = DateTime.Now.AddHours(12) 
                });
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

            context.Response.Cookies.Append("SessionKey", sessionKey, new CookieOptions()
            {
                Expires = DateTime.Now.AddHours(12)
            });

            return new Tuple<bool, string>(true, "Login Succesfull : " + sessionKey);

        }

        public static void Logout(HttpContext context, string ip)
        {
            context.Response.Cookies.Append("SessionKey", "", new CookieOptions()
            {
                Expires = DateTime.Now
            });
        }

        public static string GenerateRandomString(int seed, int length = 16, bool fullCaps = false, bool DayRandom = false)
        {
            DateTime now = DateTime.Now;
            Random stringrnd;
            if (!DayRandom) stringrnd = new Random((seed + "0" + now.ToString("ffff")).ToInt());
            else stringrnd = new Random((seed + "0" + now.DayOfYear + now.Year).ToInt());
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (fullCaps) alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string returnstring = "";

            for (int i = 0; i < length; i++)
            {
                returnstring += alpha.Substring(stringrnd.Next(0, alpha.Length), 1);
            }
            return returnstring;
        }

        public static string Encrypt(string Password, string salt = "PermaCall")
        {
            byte[] MessageBytes = Encoding.UTF8.GetBytes(string.Concat(Password, salt));
            SHA512Managed SHhash = new SHA512Managed();
            byte[] HashValue = SHhash.ComputeHash(MessageBytes);
            string strHex = "";
            foreach (byte b in HashValue) { strHex += String.Format("{0:x2}", b); }
            return strHex;
        }
    }
}