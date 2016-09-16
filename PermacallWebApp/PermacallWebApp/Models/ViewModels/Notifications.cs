using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallWebApp.Models
{
    public static class Notifications
    {
        public static string GreenNotification { get; set; }

        public static bool GreenNotExist()
        {
            if (!string.IsNullOrEmpty(GreenNotification)) return true;
            return false;
        }

        public static string GetGreenNot()
        {
            string returnString = GreenNotification;
            GreenNotification = "";
            return returnString;
        }
    }
}