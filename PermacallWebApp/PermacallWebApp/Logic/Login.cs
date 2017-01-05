using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using PermacallWebApp.Models.ReturnModels;
using PermacallWebApp.Repos;

namespace PermacallWebApp.Logic
{
    public static class Login
    {
        public static User GetCurrentUser(HttpContext context)
        {
            try
            {
                AccountRepo.StrikeReductionCheck();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }

            string sessionKey;
            if (!String.IsNullOrEmpty(context.Request.Cookies["SessionData"]?["SessionKey"]))
            {
                sessionKey = context.Request.Cookies["SessionData"]["SessionKey"];
            }
            else sessionKey = "nothing";


            var sessionRe = AccountRepo.GetUser(sessionKey);
            if (sessionRe.ID > 0)
            {
                context.Response.Cookies["SessionData"]["SessionKey"] = sessionKey;
                context.Response.Cookies["SessionData"].Expires = DateTime.Now.AddMinutes(120);

                return sessionRe;
            }
            return sessionRe;
        }

        public static Tuple<bool, string> AuthorizeUser(HttpContext context, string username, string password)
        {
            var saltRe = AccountRepo.GetSalt(username);
            if (!saltRe.Item1)
            {
                if(saltRe.Item2 == "NOCONNECTION") return new Tuple<bool, string>(false, "Connection to the database could not be established");
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
            context.Response.Cookies["SessionData"].Expires = DateTime.Now.AddMinutes(120);

            return new Tuple<bool, string>(true, "Login Succesfull");

        }

        public static string GenerateRandomString(int seed, int length = 16, bool fullCaps = false, bool DayRandom = false)
        {
            DateTime now = DateTime.Now;
            Random stringrnd;
            if (!DayRandom) stringrnd = new Random(seed + now.DayOfYear + now.Hour + now.Minute + now.Millisecond);
            else stringrnd = new Random(seed + now.DayOfYear + now.Year);
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

        public static bool ForceHTTPSConnection(HttpContext context, bool forceHTTPS)
        {
            if (!context.Request.IsLocal && !context.Request.IsSecureConnection && forceHTTPS)
            {
                string redirectUrl = context.Request.Url.ToString().Replace("http:", "https:");
                context.Response.Redirect(redirectUrl, false);
                context.ApplicationInstance.CompleteRequest();
                return false;
            }
            if (!context.Request.IsLocal && context.Request.IsSecureConnection && !forceHTTPS)
            {
                string redirectUrl = context.Request.Url.ToString().Replace("https:", "http:");
                context.Response.Redirect(redirectUrl, false);
                context.ApplicationInstance.CompleteRequest();
                return false;
            }
            LogRepo.Log("[" + context.Request.HttpMethod + "] " + context.Request.RawUrl, LogRepo.LogCategory.Request, context.Request.UserHostAddress);
            return true;
        }
    }
}