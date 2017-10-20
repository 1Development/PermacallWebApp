using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using PermacallWebApp.Models;
using PermacallWebApp.Repos;
using Login = PermacallWebApp.Logic.Login;

namespace PermacallWebApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(string redirectURL = null)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");

            Account viewModel = new Account();
            if (redirectURL != null)
                viewModel.RedirectPage = redirectURL;
            else if (Request.UrlReferrer != null)
            {
                string prevURL = Request.UrlReferrer.AbsoluteUri;
                if(prevURL.Contains("permacall.nl") || Request.IsLocal)
                redirectURL = prevURL;
            }

            User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.ID > 0)
            {
                if (currentUser.Permission >= PCAuthLib.User.PermissionGroup.USER)
                    return RedirectToAction("ShowTeamspeak", "Management");

                return RedirectToAction("Index", "Management");
            }
            viewModel.RedirectPage = redirectURL;

            return View(viewModel);
        }

        //POST: Login
        [HttpPost]
        public ActionResult Index(Account account)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");

            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID > 0) return RedirectToAction("Index", "Management");

            if (ModelState.IsValid)
            {
                var loginRe = Login.AuthorizeUser(System.Web.HttpContext.Current, account.Username, account.Password);
                if (loginRe.Item1)
                {
                    User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);

                    if (!String.IsNullOrEmpty(account.RedirectPage) && account.RedirectPage.Length > 0 &&
                        (account.RedirectPage.Contains("permacall.nl") || Request.IsLocal))
                        return Redirect(account.RedirectPage);

                    if (currentUser.ID > 0)
                    {
                        if (currentUser.Permission >= PCAuthLib.User.PermissionGroup.USER)
                            return RedirectToAction("ShowTeamspeak", "Management");
                    }
                    return RedirectToAction("Index", "Management");
                }
                account.ErrorMessage = loginRe.Item2;
            }


            return View(account);
        }

        // GET: Login/Register
        public ActionResult Register()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");

            Account viewModel = new Account();

            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID > 0) return RedirectToAction("Index", "Management");

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");

            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID > 0) return RedirectToAction("Index", "Management");

            if (ModelState.IsValid)
            {
                if (account.Password == account.CheckPassword)
                {
                    if (!account.Username.Contains(' '))
                    {
                        if (AccountRepo.CheckAvailable(account.Username))
                        {
                            string randomstring = Login.GenerateRandomString(32);
                            AccountRepo.InsertNewAccount(account.Username, Login.Encrypt(account.Password, randomstring), randomstring);
                            return RedirectToAction("RegisterSuccesfull", "Login");
                        }
                        else
                        {
                            ViewBag.Error = "Username is not available";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Username cannot contain spaces";
                    }
                }
                else
                {
                    ViewBag.Error = "Passwords don't match!";
                }
            }


            return View(account);
        }

        public ActionResult RegisterSuccesfull()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");
            return View();
        }

        public ActionResult Logout()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return View("~/Views/Home/NoSecureConnection.cshtml");

            Login.Logout(System.Web.HttpContext.Current, Request.UserHostAddress);

            return RedirectToAction("Index", "Home");
        }
    }
}