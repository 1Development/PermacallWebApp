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
    public class MPInc : Controller
    {
        // GET: MultiplayerIncremental/NewPlayer
        [HttpPost]
        public string NewPlayer(DataPair encData)
        {
            string decryptedData = StringCipher.Decrypt(encData.data, encData.key);
            IncrementalPlayer playerData = JsonConvert.DeserializeObject<IncrementalPlayer>(decryptedData);
            bool succes = PlayerRepo.NewPlayer(playerData.Username, playerData.Identifier);

            return succes ? "1" : "0";
        }

        public string Main(DataPair encData)
        {
            DataPair decryptedData = DecryptData(encData);
            string command = decryptedData.key;
            IncrementalPlayer sender = decryptedData.Player;
            Dictionary<string, string> data =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(decryptedData.data);

            switch (command)
            {
                case "GetTeamMember":
                    {
                        string playerID = data["PlayerID"];
                        string playerGroupCode = data["GroupCode"];
                        IncrementalPlayer plr = PlayerRepo.GetTeamMember(playerID.ToInt(), playerGroupCode);
                        string returnData = JsonConvert.SerializeObject(plr);
                        return JsonConvert.SerializeObject(EncryptData(returnData, sender));
                    }
                case "SendPlayerData":
                {
                    string playerID = data["PlayerID"];
                    string playerGroupCode = data["GroupCode"];
                    IncrementalPlayer plr = PlayerRepo.GetTeamMember(playerID.ToInt(), playerGroupCode);
                    string returnData = JsonConvert.SerializeObject(plr);
                    return JsonConvert.SerializeObject(EncryptData(returnData, sender));
                }
            }

            return "Invalid Request";
        }




        private DataPair EncryptData(string dataToEncrypt, IncrementalPlayer player)
        {
            return new DataPair(player.ID.ToString(), StringCipher.Encrypt(dataToEncrypt, player.Identifier));
        }
        private DataPair DecryptData(DataPair dataToDencrypt)
        {
            var playerData = Repos.IncrementalGame.PlayerRepo.GetPlayerInfo(dataToDencrypt.key.ToInt());
            return new DataPair(JsonConvert.DeserializeObject<DataPair>(StringCipher.Decrypt(dataToDencrypt.data, playerData.Identifier)), playerData);
        }
    }
}