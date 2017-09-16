using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PermacallTools.Logic;
using PermacallTools.Models.IncrementalGame;

namespace PermacallTools.Controllers
{
    public class MPInc : Controller
    {
        // GET: MultiplayerIncremental/NewPlayer
        [HttpPost]
        public string NewPlayer(EncryptedData encData)
        {
            string decryptedData = StringCipher.Decrypt(encData.data, encData.key);
            Dictionary<string,string> playerData = JsonConvert.DeserializeObject<Dictionary<string,string>>()
        }

        private EncryptedData EncryptData(string dataToEncrypt, IncrementalPlayer player)
        {
            return new EncryptedData(player.ID.ToString(), StringCipher.Encrypt(dataToEncrypt, player.Identifier));
        }
    }
}