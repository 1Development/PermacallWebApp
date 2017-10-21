using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PermacallTools.Logic
{
    public class Login
    {
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
            return true;
        }

        public static string GenerateRandomString(int length = 16, bool fullCaps = false)
        {
            Random stringrnd = new Random();
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (fullCaps) alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string returnstring = "";

            for (int i = 0; i < length; i++)
            {
                returnstring += alpha.Substring(stringrnd.Next(0, alpha.Length), 1);
            }
            return returnstring;
        }

        public static string GenerateRandomFromHash(int seed, int length = 16, bool fullCaps = false, bool DayRandom = false)
        {
            DateTime now = DateTime.Now;
            string retString;
            if (DayRandom) retString = Encrypt(seed + now.DayOfYear + now.ToString("yy") + "");
            else retString = Encrypt(seed + now.DayOfYear + now.ToString("yy") + now.Hour + now.Minute + now.Second + now.Millisecond);

            retString = new Regex("[^a-zA-Z0-9]").Replace(retString, "");
            if (fullCaps) retString = retString.ToUpper();

            return retString.Substring(0, length);
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