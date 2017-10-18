using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using PCDataDLL;
using PermacallTools.Models.IncrementalGame;

namespace PermacallTools.Repos.IncrementalGame
{
    public class PlayerRepo
    {
        /// <summary>
        /// Creates a new player entry in the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static bool NewPlayer(string name, string identifier)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"name", name},
                {"identifier", identifier},
            };
            return DB.MainDB.UpdateQuery("INSERT INTO Player(Name, Identifier) VALUES(Name=?, Identifier=?)", parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IncrementalPlayer GetPlayerInfo(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID,Name,Key,GroupCode FROM Player WHERE ID=?", parameters);

            IncrementalPlayer player = new IncrementalPlayer();
            if (result != null)
            {
                player.ID = result.Get("ID");
                player.Key = result.Get("Key");
                player.Username = result.Get("Name");
                player.GroupCode = result.Get("GroupCode");

                DateTime lastUpdateDate = DateTime.Now;
                DateTime.TryParse(result.Get("LastUpdate"), out lastUpdateDate);
                player.LastUpdate = lastUpdateDate;
            }

            return player;
        }

        /// <summary>
        /// Returns team member by group code
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="playerGroupCode"></param>
        /// <returns></returns>
        public static IncrementalPlayer GetTeamMember(int playerID, string playerGroupCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", playerID},
                {"groupCode", playerGroupCode},
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID,Name,Key FROM Player WHERE ID!=? AND GROUPCODE=?", parameters);

            IncrementalPlayer plr = new IncrementalPlayer();
            if (result != null)
            {
                plr.ID = result.Get("ID");
                plr.Key = result.Get("Key");
                plr.Username = result.Get("Name");

                DateTime lastUpdateDate = DateTime.Now;
                DateTime.TryParse(result.Get("LastUpdate"), out lastUpdateDate);
                plr.LastUpdate = lastUpdateDate;
            }

            return plr;
        }

        /// <summary>
        /// Updates a player's values(upgrades/buildings)
        /// </summary>
        /// <param name="buildings"></param>
        /// <param name="upgrades"></param>
        /// <param name="playerID"></param>
        /// <param name="playerKey"></param>
        /// <returns></returns>
        public static bool UpdatePlayer(Dictionary<string, int> buildings, Dictionary<string, int> upgrades, int playerID, string playerKey)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"buildings", JsonConvert.SerializeObject(buildings)},
                {"upgrades", JsonConvert.SerializeObject(upgrades)},
                {"id", playerID},
            };
            return DB.MainDB.UpdateQuery("UPDATE Player SET Buildings=?, Upgrades=?, LastUpdate=NOW() WHERE ID=? AND PlayerKey=?", parameters);
        }

        /// <summary>
        /// Updates a player's GroupCode in the Database
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public static bool UpdatePlayerGroupCode(string groupCode, int playerID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"groupcode", groupCode},
                {"id", playerID},
            };
            return DB.MainDB.UpdateQuery("UPDATE Player SET GroupCode=? WHERE ID=?", parameters);
        }

        /// <summary>
        /// Gets IncrementalPlayer object from database by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IncrementalPlayer GetPlayerData(string id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT * FROM Player WHERE ID=?", parameters);

            IncrementalPlayer player = new IncrementalPlayer();
            if (result != null)
            {
                player.ID = result.Get("ID");
                player.Key = result.Get("Key");
                player.Username = result.Get("Name");
                player.Buildings = JsonConvert.DeserializeObject<Dictionary<string, int>>(result.Get("Buildings"));
                player.Upgrades = JsonConvert.DeserializeObject<Dictionary<string, int>>(result.Get("Upgrades"));

                DateTime lastUpdateDate = DateTime.Now;
                DateTime.TryParse(result.Get("LastUpdate"), out lastUpdateDate);
                player.LastUpdate = lastUpdateDate;
            }

            return player;
        }

        /// <summary>
        /// Gets complete player data string by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetPlayerDataString(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT * FROM Player WHERE ID=?", parameters);

            Dictionary<string, string> player = new Dictionary<string, string>();
            if (result != null)
            {
                player["ID"] = result.Get("ID");
                player["Identifier"] = result.Get("Identifier");
                player["Username"] = result.Get("Name");
                player["Buildings"] = result.Get("Buildings");
                player["Upgrades"] = result.Get("Upgrades");
                player["LastUpdate"] = result.Get("LastUpdate");
            }

            return JsonConvert.SerializeObject(player);
        }
        /// <summary>
        /// Checks if the player id/key combination is valid
        /// </summary>
        /// <returns>Returns True if id/key combination is valid, and false if invalid</returns>
        public static bool CheckPlayerKey(string playerID, string playerKey)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"playerID", playerID.ToString()},
                {"playerKey" , playerKey }
            };
            return DB.MainDB.CheckExist("SELECT ID FROM Player WHERE ID=? AND PlayerKey=?", parameters);
        }

        /// <summary>
        /// Updates player id/key combination
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="oldPlayerKey"></param>
        /// <returns></returns>
        public static bool UpdatePlayerKey(int playerID, string oldPlayerKey)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"playerKey" , IncrementPlayerKey(oldPlayerKey) },
                {"playerID", playerID.ToString()},
            };
            return DB.MainDB.UpdateQuery("UPDAET Player SET PlayerKey=? WHERE ID=?", parameters);
        }

        /// <summary>
        /// Increments the player key by 1 step
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string IncrementPlayerKey(string input)
        {
            var half1 = SHA1(input.Substring(0, input.Length / 2));
            var half2 = SHA1(input.Substring(input.Length / 2));
            var result = SHA1(half1 + "IncMP" + half2);
            if (result.Length <= 8) return result;
            return result.Substring((result.Length - 8) / 2, (result.Length - 8) / 2 + 8);
        }

        /// <summary>
        /// SHA1 hash used in incrementation process.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string SHA1(string input)
        {
            var hash = (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input)));
            return string.Join("", hash.Select(x => x.ToString("X2")).ToArray());
        }

    }
}