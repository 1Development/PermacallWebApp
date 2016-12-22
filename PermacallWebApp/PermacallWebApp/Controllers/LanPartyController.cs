using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermacallWebApp.Logic;

namespace PermacallWebApp.Controllers
{
    public class LanPartyController : Controller
    {
        // GET: LanParty
        public ActionResult Index()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;
            return View();
        }
    }
}