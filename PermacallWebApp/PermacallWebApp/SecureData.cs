using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PermacallWebApp
{
    
    public static class SecureData
    {
        public static string[] LoginDatatxt;

        public static string ServerUsername
        {
            get
            {
                if (LoginDatatxt != null) return LoginDatatxt[0];
                if (File.Exists(@"C:\LoginData.txt"))
                {
                    LoginDatatxt = File.ReadAllLines(@"C:\LoginData.txt");
                    return LoginDatatxt[0];
                }
                return "YouDidntReallyExpectMeToJustShowThisToYouDidYou?";
            }
        }

        public static string ServerPassword
        {
            get
            {
                if (LoginDatatxt != null) return LoginDatatxt[1];
                if (File.Exists(@"C:\LoginData.txt"))
                {
                    LoginDatatxt = File.ReadAllLines(@"C:\LoginData.txt");
                    return LoginDatatxt[1];
                }
                return "YouDidntReallyExpectMeToJustShowThisToYouDidYou?";
            }
        }

        public static string DatabaseString
        {
            get
            {
                if (LoginDatatxt != null) return LoginDatatxt[2];
                if (File.Exists(@"C:\LoginData.txt"))
                {
                    LoginDatatxt = File.ReadAllLines(@"C:\LoginData.txt");
                    return LoginDatatxt[2];
                }
                return "YouDidntReallyExpectMeToJustShowThisToYouDidYou?";
            }
        }
    }
}