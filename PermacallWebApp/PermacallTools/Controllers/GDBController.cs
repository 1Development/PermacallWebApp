using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PermacallTools.Controllers
{
    public class GDBController : Controller
    {
        public static Dictionary<string, Dictionary<string, float>[]> SavedDictionaries;
        // GET: GDB
        public string Index(string gameCode, int playerNr, string json)
        {
            if (SavedDictionaries == null) SavedDictionaries = new Dictionary<string, Dictionary<string, float>[]>();

            Dictionary<string, float> resourceDict = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);

            if (!SavedDictionaries.ContainsKey(gameCode))
            {
                SavedDictionaries.Add(gameCode, new Dictionary<string, float>[2]);
            }
            SavedDictionaries[gameCode][OtherPlr(playerNr)] = resourceDict;
            Dictionary<string, float> returnDict = SavedDictionaries[gameCode][playerNr];

            return JsonConvert.SerializeObject(returnDict);
        }

        private int OtherPlr(int plrNr)
        {
            if (plrNr == 1)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}