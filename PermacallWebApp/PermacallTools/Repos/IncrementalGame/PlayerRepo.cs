using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PCDataDLL;
using PermacallTools.Models.IncrementalGame;

namespace PermacallTools.Repos.IncrementalGame
{
    public class PlayerRepo
    {
        public static bool NewPlayer(string name, string identifier)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"name", name},
                {"identifier", identifier},
            };
            return DB.MainDB.UpdateQuery("INSERT INTO Player(Name, Identifier) VALUES(Name=?, Identifier=?)", parameters);
        }

        public static IncrementalPlayer GetPlayerInfo(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID,Name,Identifier,GroupCode FROM Player WHERE ID=?", parameters);

            IncrementalPlayer player = null;
            if (result != null)
            {
                player.ID = result.Get("ID").ToInt();
                player.Identifier = result.Get("Identifier");
                player.Username = result.Get("Name");
                player.GroupCode = result.Get("GroupCode");

                DateTime lastUpdateDate = DateTime.Now;
                DateTime.TryParse(result.Get("LastUpdate"), out lastUpdateDate);
                player.LastUpdate = lastUpdateDate;
            }

            return player;
        }

        public static IncrementalPlayer GetTeamMember(int playerID, string playerGroupCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", playerID},
                {"groupCode", playerGroupCode},
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID,Name,Identifier FROM Player WHERE ID!=? AND GROUPCODE=?", parameters);

            IncrementalPlayer plr = null;
            if (result != null)
            {
                plr.ID = result.Get("ID").ToInt();
                plr.Identifier = result.Get("Identifier");
                plr.Username = result.Get("Name");

                DateTime lastUpdateDate = DateTime.Now;
                DateTime.TryParse(result.Get("LastUpdate"), out lastUpdateDate);
                plr.LastUpdate = lastUpdateDate;
            }

            return plr;
        }

        public static bool UpdatePlayer(Dictionary<string, int> buildings, Dictionary<string, int> upgrades, int playerID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"buildings", JsonConvert.SerializeObject(buildings)},
                {"upgrades", JsonConvert.SerializeObject(upgrades)},
                {"id", playerID},
            };
            return DB.MainDB.UpdateQuery("UPDATE Player SET Buildings=?, Upgrades=?, LastUpdate=NOW() WHERE ID=?", parameters);
        }
        public static bool UpdatePlayerGroupCode(string groupCode, int playerID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"groupcode", groupCode},
                {"id", playerID},
            };
            return DB.MainDB.UpdateQuery("UPDATE Player SET GroupCode=? WHERE ID=?", parameters);
        }

        public static IncrementalPlayer GetPlayerData(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT * FROM Player WHERE ID=?", parameters);

            IncrementalPlayer player = null;
            if (result != null)
            {
                player.ID = result.Get("ID").ToInt();
                player.Identifier = result.Get("Identifier");
                player.Username = result.Get("Name");
                player.Buildings = JsonConvert.DeserializeObject<Dictionary<string, int>>(result.Get("Buildings"));
                player.Upgrades = JsonConvert.DeserializeObject<Dictionary<string, int>>(result.Get("Upgrades"));

                DateTime lastUpdateDate = DateTime.Now;
                DateTime.TryParse(result.Get("LastUpdate"), out lastUpdateDate);
                player.LastUpdate = lastUpdateDate;
            }

            return player;
        }

        public static string GetPlayerDataString(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT * FROM Player WHERE ID=?", parameters);

            Dictionary<string, string> player = null;
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

    }
}