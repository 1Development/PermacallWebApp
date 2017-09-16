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
        public static void NewPlayer(IncrementalPlayer player)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"name", player.Username},
                {"identifier", player.Identifier},
            };
            DB.MainDB.GetOneResultQuery("INSERT INTO Player(Name, Identifier) VALUES(Name=?, Identifier=?)", parameters);
        }

        public static IncrementalPlayer GetPlayerInfo(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id.ToString()}
            };
            var result = DB.MainDB.GetOneResultQuery("SELECT ID,Name,Identifier FROM Player WHERE ID=?", parameters);

            IncrementalPlayer player = null;
            if (result != null)
            {
                player.ID = result.Get("ID").ToInt();
                player.Identifier = result.Get("Identifier");
                player.Username = result.Get("Name");

                DateTime lastUpdateDate = DateTime.Now;
                DateTime.TryParse(result.Get("LastUpdate"), out lastUpdateDate);
                player.LastUpdate = lastUpdateDate;
            }

            return player;
        }

        public static IncrementalPlayer GetTeamMember(IncrementalPlayer player)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", player.ID},
                {"groupCode", player.GroupCode},
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

        public static void UpdatePlayer(IncrementalPlayer player)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"buildings", JsonConvert.SerializeObject(player.Buildings)},
                {"upgrades", JsonConvert.SerializeObject(player.Upgrades)},
                {"id", player.ID},
            };
            var result = DB.MainDB.GetOneResultQuery("UPDATE Player SET Buildings=?, Upgrades=?, LastUpdate=NOW() WHERE ID=?", parameters);
        }
        public static void UpdatePlayerGroupCode(IncrementalPlayer player)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"buildings", player.GroupCode},
            };
            var result = DB.MainDB.GetOneResultQuery("UPDATE Player SET GroupCode=?", parameters);
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
                player.Buildings = JsonConvert.DeserializeObject<Dictionary<string,int>>(result.Get("Buildings"));
                player.Upgrades = JsonConvert.DeserializeObject<Dictionary<string,int>>(result.Get("Upgrades"));

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

            Dictionary<string,string> player = null;
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