﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermacallWebApp.Logic;
using TS3QueryLib;
using TS3QueryLib.Core;
using TS3QueryLib.Core.CommandHandling;
using TS3QueryLib.Core.Common;
using TS3QueryLib.Core.Server;
using TS3QueryLib.Core.Server.Entities;
using PermacallWebApp.Repos;

namespace PermacallWebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");
            return View();
        }

        public ActionResult NoSecureConnection()
        {
            if (Request.IsLocal)
            {
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}