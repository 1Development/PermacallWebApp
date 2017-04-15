using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PermacallWebApp.Models.LanParty;

namespace PermacallWebApp.Logic
{
    public class LanParty
    {
        public static string GetTimerString(DateTime StartTime)
        {
            var Seconds_Between_Dates = (DateTime.Now - StartTime).TotalSeconds;

            var hours = Math.Floor(Seconds_Between_Dates / 3600);
            var minutes = Math.Floor((Seconds_Between_Dates - hours * 3600) / 60);
            var seconds = Math.Floor(Seconds_Between_Dates - hours * 3600 - minutes * 60);
            var toPrintSec = "";
            if (seconds <= 9) toPrintSec = "0" + seconds;
            else toPrintSec = "" + seconds;
            var toPrintMin = "";
            if (minutes <= 9) toPrintMin = "0" + minutes;
            else toPrintMin = "" + minutes;

            return hours + ":" + toPrintMin + ":" + toPrintSec;
        }
    }
}