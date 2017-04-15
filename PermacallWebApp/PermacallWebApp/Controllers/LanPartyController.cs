using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermacallWebApp.Logic;
using PermacallWebApp.Models.LanParty;
using PermacallWebApp.Models.ViewModels;
using PermacallWebApp.Repos;
using LanParty = PermacallWebApp.Models.LanParty.LanParty;

namespace PermacallWebApp.Controllers
{
    public class LanPartyController : Controller
    {
        // GET: LanParty
        public ActionResult Index(int ID = 0)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");

            LanParty viewModel = new LanParty();

            if (ID == 0)
            {
                var lanParty = LanPartyRepo.GetMostRecentLanParty();
                if (lanParty != null)
                {
                    viewModel = lanParty;
                }
            }
            else
            {
                var lanParty = LanPartyRepo.GetLanParty(ID);
                if (lanParty != null)
                {
                    viewModel = lanParty;
                }
            }

            viewModel.LanPartyContent = BBCode.ParseBBCode(viewModel.LanPartyContent);

            return View(viewModel);
        }

        // POST: LanParty
        [HttpPost]
        public ActionResult Index(LanParty viewModel)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");
            if (viewModel == null) return RedirectToAction("Index");

            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);

            var lanParty = LanPartyRepo.GetLanParty(viewModel.ID);
            if (lanParty == null || lanParty.ID != viewModel.ID)
                return RedirectToAction("Index");
            if (currentUser.ID != lanParty.Owner && currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN)
                return RedirectToAction("Index");

            InsomniaUser usr = new InsomniaUser();
            usr.Name = viewModel.NewDropOut;
            usr.DropOutTime = Logic.LanParty.GetTimerString(lanParty.LanPartyInsomnia.Start);

            lanParty.LanPartyInsomnia.Users.Add(usr);

            LanPartyRepo.UpdateLanParty(lanParty);

            return RedirectToAction("Index", new { ID = lanParty.ID});
        }

        public ActionResult InsomniaContent(int ID = 0)
        {
            LanParty viewModel = new LanParty();

            if (ID == 0)
            {
                var lanParty = LanPartyRepo.GetMostRecentLanParty();
                if (lanParty != null)
                {
                    viewModel = lanParty;
                }
            }
            else
            {
                var lanParty = LanPartyRepo.GetLanParty(ID);
                if (lanParty != null)
                {
                    viewModel = lanParty;
                }
            }

            viewModel.LanPartyContent = BBCode.ParseBBCode(viewModel.LanPartyContent);

            return PartialView(viewModel);
        }


        public ActionResult Edit(int ID = 0, string delInsom = "")
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true))
                return View("~/Views/Home/NoSecureConnection.cshtml");
            if (ID == 0) return RedirectToAction("Index");

            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);

            var lanParty = LanPartyRepo.GetLanParty(ID);
            if (lanParty == null || currentUser.ID == 0)
                return RedirectToAction("Index");
            if (currentUser.ID != lanParty.Owner && currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN)
                return RedirectToAction("Index");

            if (delInsom != "")
            {
                lanParty.LanPartyInsomnia.Users.Remove(lanParty.LanPartyInsomnia.Users.Find(x => x.Name == delInsom));
                LanPartyRepo.UpdateLanParty(lanParty);
                return RedirectToAction("Edit", new { ID = lanParty.ID });
            }

            return View(lanParty);

        }

        [HttpPost]
        public ActionResult Edit(LanParty viewModel = null)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");
            if (viewModel == null) return RedirectToAction("Index");

            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);

            var lanParty = LanPartyRepo.GetLanParty(viewModel.ID);
            if (lanParty == null || lanParty.ID != viewModel.ID)
                return RedirectToAction("Index");
            if (currentUser.ID != lanParty.Owner && currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN)
                return RedirectToAction("Index");

            if (LanPartyRepo.UpdateLanParty(viewModel))
                return RedirectToAction("Index", new { ID = viewModel.ID });


            return View(lanParty);

        }

        public ActionResult List()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");

            return View();
        }
    }
}