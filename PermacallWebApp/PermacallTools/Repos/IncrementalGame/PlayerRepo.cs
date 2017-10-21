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
        public static bool NewPlayer(string name, string accountID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"name", name},
                {"accountID", accountID},
            };
            return DB.MainDB.UpdateQuery("INSERT INTO Player(Name, AccountID) VALUES(Name=?, Accountid=?)", parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IncrementalPlayer GetPlayerInfo(string id)
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
        public static IncrementalPlayer GetTeamMember(string accountID, string playerGroupCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"accountID", accountID},
                {"groupCode", playerGroupCode},
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID,Name FROM Player WHERE AccountID!=? AND GROUPCODE=?", parameters);

            IncrementalPlayer plr = new IncrementalPlayer();
            if (result != null)
            {
                plr.ID = result.Get("ID");
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
        public static bool UpdatePlayer(IncrementalPlayer player)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"buildings", JsonConvert.SerializeObject(player.Buildings)},
                {"upgrades", JsonConvert.SerializeObject(player.Upgrades)},
                {"id", player.ID},
            };
            return DB.MainDB.UpdateQuery("UPDATE Player SET Buildings=?, Upgrades=?, LastUpdate=NOW() WHERE ID=?", parameters);
        }

        /// <summary>
        /// Updates a player's GroupCode in the Database
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public static bool UpdatePlayerGroupCode(string groupCode, string playerID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"groupcode", groupCode},
                {"id", playerID},
            };
            return DB.MainDB.UpdateQuery("UPDATE Player SET GroupCode=? WHERE ID=?", parameters);
        }

        public static bool isGroupCodeDuplicate(string groupCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"groupcode", groupCode}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT COUNT(*) AS `COUNT` FROM PLAYER WHERE GroupCode=?", parameters);
            return result.Get("COUNT").ToInt() >= 2;
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
        public static string GetPlayerDataString(string id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT * FROM Player WHERE ID=?", parameters);

            if (result == null) return null;
            
            Dictionary<string, string> player = new Dictionary<string, string>();

            player["ID"] = result.Get("ID");
            player["Identifier"] = result.Get("Identifier");
            player["Username"] = result.Get("Name");
            player["Buildings"] = result.Get("Buildings");
            player["Upgrades"] = result.Get("Upgrades");
            player["LastUpdate"] = result.Get("LastUpdate");


            return JsonConvert.SerializeObject(player);
        }

    }
}