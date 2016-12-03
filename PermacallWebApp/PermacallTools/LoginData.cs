using System.IO;

namespace PermacallTools
{
    
    public static class LoginData
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
                if (LoginDatatxt != null) return LoginDatatxt[3];
                if (File.Exists(@"C:\LoginData.txt"))
                {
                    LoginDatatxt = File.ReadAllLines(@"C:\LoginData.txt");
                    return LoginDatatxt[3];
                }
                return "YouDidntReallyExpectMeToJustShowThisToYouDidYou?";
            }
        }
    }
}