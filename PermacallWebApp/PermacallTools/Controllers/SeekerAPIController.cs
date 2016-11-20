using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PermacallTools.Models.Seeker;
using PermacallTools.Repos;

namespace PermacallTools.Controllers
{
    public class SeekerAPIController : Controller
    {
        // GET: SeekerAPI
        public string Index()
        {
            return "This page is not supposed to be accessed directly, so please refrain from doing so";
        }

        //Get: SeekerAPI/HostGame?aID=???
        public string HostGame(string aID = null)
        {
            if (aID == null) return "";

            string GameCode = GenerateGameCode(DateTime.Now.Millisecond, 3, true);
            if (SeekerRepo.CreateGame(GameCode, aID))
                return GameCode;
            else return "0";
        }

        //Get: SeekerAPI/GameCheck?GC=???
        public string GameCheck(string GC = null)
        {
            if (GC == null) return "";

            if (SeekerRepo.GameCodeExist(GC))
            {
                return "1";
            }

            return "0";
        }

        //Get: SeekerAPI/JoinGame?GC=???&name=???&aID=???
        public string JoinGame(string GC = null, string name = null, string aID = null)
        {
            if (aID == null || name == null || GC == null) return "";

            if (SeekerRepo.GameCodeExist(GC) && SeekerRepo.JoinGame(GC, name, aID))
            {
                if (SeekerRepo.isHost(GC, aID)) return "2";
                return "1";
            }
                
            return "0";
        }

        public string IsStarted(string GC = null)
        {
            if (GC == null) return "";

            if (SeekerRepo.GameCodeExist(GC) && SeekerRepo.isStarted(GC))
            {
                return "1";
            }

            return "0";
        }

        //Get: SeekerAPI/StartGame?GC=???&name=???&aID=???
        public string StartGame(string GC = null, string aID = null)
        {
            if (aID == null || GC == null) return "";

            if (SeekerRepo.GameCodeExist(GC) && SeekerRepo.StartGame(GC,aID))
            {
                return "1";
            }

            return "0";
        }

        //Get: SeekerAPI/ThisPhoneFound?GC=???&aID=???
        public string ThisPhoneFound(string GC = null, string aID = null)
        {
            if (aID == null || GC == null) return "";

            if (SeekerRepo.GameCodeExist(GC) && SeekerRepo.SetFound(GC,aID))
            {
                return "1";
            }

            return "0";
        }

        //Get: SeekerAPI/ThisPhoneFound?GC=???&aID=???&nameF=???
        public string SetPhoneFinder(string GC = null, string aID = null, string nameF = null)
        {
            if (aID == null || GC == null || nameF == null) return "";

            if (SeekerRepo.GameCodeExist(GC) && SeekerRepo.PlusScore(GC, nameF, 1))
            {
                return "1";
            }

            return "0";
        }

        //Get: SeekerAPI/TimeExpired?GC=???&aID=???&name=???
        public string TimeExpired(string GC = null, string aID = null, string name = null)
        {
            if (aID == null || GC == null || name == null) return "";

            if (SeekerRepo.GameCodeExist(GC) && SeekerRepo.PlusScore(GC, name, 2))
            {
                return "1";
            }

            return "0";
        }

        //Get: SeekerAPI/GetScores?GC=???
        public string GetScores(string GC = null)
        {
            if (GC == null) return "";

            if (SeekerRepo.GameCodeExist(GC))
            {
                List<Score> scores = SeekerRepo.GetScores(GC);
                if (scores == null) return "SCORELIST RETURNED NULL";
                return JsonConvert.SerializeObject(scores);
            }

            return "ERROR";
        }



        //Logic
        public static string GenerateGameCode(int seed, int length = 16, bool fullCaps = false, bool DayRandom = false)
        {
            DateTime now = DateTime.Now;
            Random stringrnd;
            if (!DayRandom) stringrnd = new Random(seed + now.DayOfYear + now.Hour + now.Minute + now.Millisecond);
            else stringrnd = new Random(seed + now.DayOfYear + now.Year);
            string alpha = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (fullCaps) alpha = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";
            string returnstring = "";

            for (int i = 0; i < length; i++)
            {
                returnstring += alpha.Substring(stringrnd.Next(0, alpha.Length), 1);
            }

            if (SeekerRepo.GameCodeExist(returnstring)) return GenerateGameCode(seed, length, fullCaps, DayRandom);
            return returnstring;
        }
    }
}