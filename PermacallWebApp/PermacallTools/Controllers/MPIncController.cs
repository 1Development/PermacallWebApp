using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PermacallTools.Logic;
using PermacallTools.Logic.MPInc;
using PermacallTools.Models.IncrementalGame;
using PermacallTools.Repos.IncrementalGame;

namespace PermacallTools.Controllers
{
    public class MPIncController : Controller
    {
        public string Index()
        {
            return "Succes";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">IncAccount toRegisterAccount</param>
        /// <returns></returns>
        [HttpPost]
        public string Register(DataPair data)
        {
            if (!Logic.Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "ConnErr";
            IncAccount playerData = JsonConvert.DeserializeObject<IncAccount>(data.data, new JsonSerializerSettings() { });
            bool available = MPIncAccountRepo.CheckAvailable(playerData.Username);
            if (!available) return "0";

            string newSalt = Logic.Login.GenerateRandomString();


            if (MPIncAccountRepo.InsertNewAccount(playerData.Username, Logic.Login.Encrypt(playerData.Password, newSalt), newSalt))
                return "1";

            return "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">IncAccount toLoginAccount</param>
        /// <returns></returns>
        [HttpPost]
        public string Login(DataPair data)
        {
            if (!Logic.Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "ConnErr";
            IncAccount playerData = JsonConvert.DeserializeObject<IncAccount>(data.data);
            Tuple<bool, string> result = MPIncAccountRepo.ValidateCredentials(playerData.Username, playerData.Password);
            if (!result.Item1) return "InvalidLogin";

            string newSessionKey = Logic.Login.GenerateRandomString();
            return MPIncAccountRepo.SetSessionKey(result.Item2, newSessionKey) ? newSessionKey : "Error";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">IncrementalPlayer toCreatePlayer</param>
        /// <returns></returns>
        [HttpPost]
        public string NewPlayer(DataPair data)
        {
            if (!Logic.Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "0";
            IncAccount account = MPIncAccountRepo.GetUser(data.SessionKey);
            if (account.ID == "0") return "InvalidSession";

            IncrementalPlayer playerData = JsonConvert.DeserializeObject<IncrementalPlayer>(data.data);
            bool succes = PlayerRepo.NewPlayer(playerData.Username, account.ID);

            return succes ? "1" : "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">int playerID</param>
        /// <returns></returns>
        [HttpPost]
        public string GetPlayerData(DataPair data)
        {
            if (!Logic.Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "0";
            IncAccount account = MPIncAccountRepo.GetUser(data.SessionKey);
            if (account.ID == "0") return "InvalidSession";

            if (!MPIncAccountRepo.CheckPlayerAccount(data.data, account.ID)) return "InvalidData";
            string returnString = PlayerRepo.GetPlayerDataString(data.data);

            if (String.IsNullOrWhiteSpace(returnString)) return "InvalidData";
            return returnString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">string groupCode</param>
        /// <returns></returns>
        [HttpPost]
        public string GetTeamMember(DataPair data)
        {
            if (!Logic.Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "0";
            IncAccount account = MPIncAccountRepo.GetUser(data.SessionKey);
            if (account.ID == "0") return "InvalidSession";

            if (!MPIncAccountRepo.CheckPlayerAccount(data.data, account.ID)) return "InvalidData";

            IncrementalPlayer teamPlr = PlayerRepo.GetTeamMember(account.ID, data.data);
            if (teamPlr == null) return "Error";

            string returnString = PlayerRepo.GetPlayerDataString(teamPlr.ID);


            if (String.IsNullOrWhiteSpace(returnString)) return "InvalidData";
            return returnString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">Dictionary [playerID, newGroupCode]</param>
        /// <returns></returns>
        [HttpPost]
        public string SetGroupCode(DataPair data)
        {
            if (!Logic.Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "0";
            IncAccount account = MPIncAccountRepo.GetUser(data.SessionKey);
            if (account.ID == "0") return "InvalidSession";

            Dictionary<string, string> requestData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.data);

            if (!MPIncAccountRepo.CheckPlayerAccount(requestData["playerID"], account.ID)) return "InvalidData";

            if (PlayerRepo.isGroupCodeDuplicate(requestData["newGroupCode"])) return "GroupCodeDuplicate";

            return PlayerRepo.UpdatePlayerGroupCode(requestData["newGroupCode"], requestData["playerID"]) ? "1" : "Error";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">IncrementalPlayer playerToUpdate</param>
        /// <returns></returns>
        [HttpPost]
        public string UpdatePlayer(DataPair data)
        {
            if (!Logic.Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "0";
            IncAccount account = MPIncAccountRepo.GetUser(data.SessionKey);
            if (account.ID == "0") return "InvalidSession";

            IncrementalPlayer player = JsonConvert.DeserializeObject<IncrementalPlayer>(data.data);

            if (!MPIncAccountRepo.CheckPlayerAccount(player.ID, account.ID)) return "InvalidData";

            return PlayerRepo.UpdatePlayer(player) ? "1" : "0";
        }




    }
}