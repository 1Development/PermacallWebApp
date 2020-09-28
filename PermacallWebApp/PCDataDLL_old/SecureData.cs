using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PCDataDLL
{
    
    public static class SecureData
    {
        private static Dictionary<string, string> loginDataDict;
        private static string SecureDataFilePath = @"C:\www\SecureData.PCdat";

        private static Dictionary<string, string> LoginDataDict()
        {
            if (loginDataDict != null) return loginDataDict;
            if (File.Exists(SecureDataFilePath))
            {
                string loginDataText = File.ReadAllText(SecureDataFilePath);
                loginDataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(loginDataText);
                return loginDataDict;
            }
            else
            {
                File.WriteAllText(SecureDataFilePath,
                    "{ \"TSusername\": \"\", \"TSpassword\": \"\", \"PCDatabaseString\": \"\", \"YouriPortDBString\": \"\" }");
                return LoginDataDict();
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

        public static string WowToolsString
        {
            get { return LoginDataDict()["WowToolsString"]; }
        }

        public static string HostEmailPassword
        {
            get { return LoginDataDict()["YouriHostEmailPassword"]; }
        }
    }
}