using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PCDataDLL;
using PermacallWebApp.Models.LanParty;

namespace PermacallWebApp.Repos
{
    public class LanPartyRepo
    {
        public static LanParty GetLanParty(int id)
        {
            string sql = "SELECT * FROM LANPARTY WHERE ID = ?";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id}
            };
            var result = DB.MainDB.GetOneResultQuery(sql, parameters);

            if (result != null)
            {
                if (result.Get("ID") != null)
                {
                    LanParty retLanParty = new LanParty();
                    retLanParty.ID = result.Get("ID").ToInt();
                    retLanParty.LanPartyName = result.Get("LanPartyName");
                    retLanParty.LanPartyContent = result.Get("LanPartyContent");
                    retLanParty.Owner = result.Get("LanPartyOwner").ToInt();

                    retLanParty.LanPartyInsomnia =
                        result.Get("Insomnia") != null ?
                        JsonConvert.DeserializeObject<Insomnia>(result.Get("Insomnia")) :
                        new Insomnia();

                    return retLanParty;
                }
                return new LanParty();
            }
            return null;
        }

        public static LanParty GetMostRecentLanParty()
        {
            string sql = "SELECT * FROM LANPARTY ORDER BY ID DESC LIMIT 1";
            var result = DB.MainDB.GetOneResultQuery(sql, null);

            if (result != null)
            {
                if (result.Get("ID") != null)
                {
                    LanParty retLanParty = new LanParty();
                    retLanParty.ID = result.Get("ID").ToInt();
                    retLanParty.LanPartyName = result.Get("LanPartyName");
                    retLanParty.LanPartyContent = result.Get("LanPartyContent");
                    retLanParty.Owner = result.Get("LanPartyOwner").ToInt();
                    
                    retLanParty.LanPartyInsomnia =
                        result.Get("Insomnia") != null ?
                        JsonConvert.DeserializeObject<Insomnia>(result.Get("Insomnia")) :
                        new Insomnia();

                    return retLanParty;
                }
                return new LanParty();
            }
            return null;
        }

        public static bool UpdateLanParty(LanParty lanParty)
        {

            lanParty.LanPartyInsomnia.Users = lanParty.LanPartyInsomnia.Users.OrderBy(x => x.DropOutTime.Replace(":", "").ToInt()).ToList();
            string sql = "UPDATE LANPARTY SET LanPartyContent=?, Insomnia=? WHERE ID=?";
            var parameters = new Dictionary<string, Object>()
            {
                {"LanPartyContent", lanParty.LanPartyContent },
                {"insomnia", JsonConvert.SerializeObject(lanParty.LanPartyInsomnia) },
                {"id", lanParty.ID }
            };
            return DB.MainDB.UpdateQuery(sql, parameters);
        }

        public static List<LanParty> GetPreviousLanParties()
        {
            string sql = "SELECT ID, LanPartyName FROM LANPARTY ORDER BY ID DESC";
            var result = DB.MainDB.GetMultipleResultsQuery(sql, null);

            List<LanParty> retList = new List<LanParty>();

            if (result != null)
            {
                foreach (var row in result)
                {
                    if (row.Get("ID") != null)
                    {
                        LanParty retLanParty = new LanParty();
                        retLanParty.ID = row.Get("ID").ToInt();
                        retLanParty.LanPartyName = row.Get("LanPartyName");

                        retList.Add(retLanParty);
                    }
                }
                
                return retList;
            }
            return null;
        }
    }
}