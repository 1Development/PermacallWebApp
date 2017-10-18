using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PermacallTools.Logic;
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

        // POST: MPInc/NewPlayer
        [HttpPost]
        public string NewPlayer(DataPair data)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "0";
            IncrementalPlayer playerData = JsonConvert.DeserializeObject<IncrementalPlayer>(data.data);
            bool succes = PlayerRepo.NewPlayer(playerData.Username, );

            return succes ? "1" : "0";
        }

        // POST: MPInc/Main
        public string Main(DataPair encData)
        {
            //switch ("1")
            //{
            //    // POST: MPInc/Main Command: GetTeamMember
            //    case "GetTeamMember":
            //        {
            //            IncrementalPlayer teamPlr = PlayerRepo.GetTeamMember(sender.ID, sender.GroupCode);
            //            string returnData = JsonConvert.SerializeObject(teamPlr);
            //            return JsonConvert.SerializeObject(EncryptData(returnData, sender));
            //        }
            //    case "SendPlayerData":
            //        {
            //            IncrementalPlayer plr = JsonConvert.DeserializeObject<IncrementalPlayer>(data["Player"]);
            //            var result = PlayerRepo.UpdatePlayer(plr.Buildings, plr.Upgrades, sender.ID);
            //            return result ? "1" : "0";
            //        }
            //    case "UpdateGroupCode":
            //        {
            //            var result = PlayerRepo.UpdatePlayerGroupCode(data["NewGroupCode"], sender.ID);
            //            return result ? "1" : "0";
            //        }
            //}

            //return "Invalid Request";
        }

        public string GetTeamMember(DataPair data)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return "[E] No Secure Connection";
            if (!PlayerRepo.CheckPlayerKey(data.playerID, data.playerKey)) return "[E] Invalid Credentials";

            IncrementalPlayer sender = PlayerRepo.GetPlayerData(data.playerID);
            IncrementalPlayer teamPlr = PlayerRepo.GetTeamMember(sender.ID, sender.GroupCode);

            DataPair returnPair = new DataPair(sender.ID, PlayerRepo.IncrementPlayerKey(sender.Key), JsonConvert.SerializeObject(teamPlr)));

            string returnData = ;
            return returnData;
        }

    }
}