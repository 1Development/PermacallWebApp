using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using PermacallTools.Models.WoWGuild;

namespace PermacallTools.Controllers
{
    public class WoWGuildController : Controller
    {
        // GET: Tools
        public ActionResult Index()
        {
            Logic.Login.ForceHTTPSConnection(System.Web.HttpContext.Current, false);

            Guild returnGuild = new Guild() { Updated = DateTime.MinValue };

            if (System.IO.Directory.Exists("C:\\www\\tools\\WoWGuild"))
            {
                DirectoryInfo info = new DirectoryInfo("C:\\www\\tools\\WoWGuild");
                FileInfo[] files = info.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();

                if (files.Length > 0)
                {
                    string json = System.IO.File.ReadAllText(files[0].FullName);
                    returnGuild = JsonConvert.DeserializeObject<Guild>(json);
                }
            }
            if (returnGuild.Updated.AddMinutes(15) < DateTime.Now)
                using (var webClient = new WebClient())
                {
                    returnGuild.WoWCharacters = new List<WoWCharacter>();
                    returnGuild.Updated = DateTime.Now;
                    webClient.Encoding = Encoding.UTF8;

                    var GuildJson =
                        webClient.DownloadString(
                            "https://eu.api.battle.net/wow/guild/grim-batol/Oath%20of%20Oblivion?fields=members&locale=en_GB&apikey=qzsht7ng7q4heek9hq6ehekjdpfmtbwu");
                    WoWGuild guild =
                        JsonConvert.DeserializeObject<WoWGuild>(GuildJson);
                    foreach (var member in guild.members)
                    {
                        string URI = "https://eu.api.battle.net/wow/character/" +
                                     normalisedString(member.character.realm) +
                                     "/" + normalisedString(member.character.name) + 
                                     "?fields=items&locale=en_GB&apikey=qzsht7ng7q4heek9hq6ehekjdpfmtbwu";
                        var charJson = webClient.DownloadString(URI);
                        WoWCharacter character = JsonConvert.DeserializeObject<WoWCharacter>(charJson);
                        if (character.level == 110) returnGuild.WoWCharacters.Add(character);
                    }
                    returnGuild.WoWCharacters = 
                        returnGuild.WoWCharacters.OrderByDescending(x => x.items.averageItemLevelEquipped).ThenByDescending(x=> x.items.averageItemLevel).ToList();

                    string exportJson = JsonConvert.SerializeObject(returnGuild);
                    System.IO.Directory.CreateDirectory("C:\\www\\tools\\WoWGuild");
                    string path = Path.Combine("C:\\www\\tools\\WoWGuild",
                        returnGuild.Updated.ToString("d-M-yyyy_HH.mm"));
                    System.IO.File.WriteAllText(path, exportJson);
                }
            return View(returnGuild);
        }

        public string normalisedString(string input)
        {
            string returnString = input.Replace(" ", "-");
            return returnString;
        }

        public ActionResult History(string record = "")
        {
            HistoryModel returnModel = new HistoryModel();

            if (System.IO.Directory.Exists("C:\\www\\tools\\WoWGuild"))
            {
                DirectoryInfo info = new DirectoryInfo("C:\\www\\tools\\WoWGuild");
                FileInfo[] files = info.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();

                if (record == "")
                {
                    if (files.Length > 0)
                    {
                        List<string> fileList = new List<string>();
                        foreach (FileInfo file in files)
                        {
                            fileList.Add(file.Name);
                        }
                        returnModel.AllBackups = fileList.ToArray();
                    }
                }
                else
                {
                    foreach (FileInfo file in files)
                    {
                        if (file.Name == record)
                        {
                            string json = System.IO.File.ReadAllText(file.FullName);
                            returnModel.WoWGuild = JsonConvert.DeserializeObject<Guild>(json);
                        }
                    }
                    
                }

            }


            return View(returnModel);
        }

    }
}