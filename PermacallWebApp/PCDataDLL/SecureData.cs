using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PCDataDLL
{
    
    public static class SecureData
    {
        private static Dictionary<string, string> loginDataDict;

        private static Dictionary<string, string> LoginDataDict()
        {
            if (loginDataDict != null) return loginDataDict;
            if (File.Exists(@"C:\www\SecureData.PCdat"))
            {
                string loginDataText = File.ReadAllText(@"C:\www\SecureData.PCdat");
                loginDataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(loginDataText);
                return loginDataDict;
            }
            return null;
        }

        public static string ServerUsername
        {
            get { return LoginDataDict()["TSusername"]; }
        }

        public static string ServerPassword
        {
            get
            {
                return LoginDataDict()["TSpassword"];
            }
        }

        public static string PCDBString
        {
            get
            {
                return LoginDataDict()["PCDatabaseString"];
            }
        }
        public static string PFDBString
        {
            get
            {
                return LoginDataDict()["YouriPortDBString"];
            }
        }
    }
}