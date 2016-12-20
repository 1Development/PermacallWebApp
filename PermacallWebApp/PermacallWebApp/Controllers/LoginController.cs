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
        public ActionResult Index(string prevPage = null)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;

            Account viewModel = new Account();
            if (prevPage != null)
                viewModel.PreviousPage = prevPage;

            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID > 0) return RedirectToAction("Index", "Management");

            return View(viewModel);
        }

        //POST: Login
        [HttpPost]
        public ActionResult Index(Account account)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;

            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID > 0) return RedirectToAction("Index", "Management");

            if (ModelState.IsValid)
            {
                var loginRe = Login.AuthorizeUser(System.Web.HttpContext.Current, account.Username, account.Password);
                if (loginRe.Item1)
                {
                    return RedirectToAction("Index", "Management");
                }
                account.ErrorMessage = loginRe.Item2;
            }


            return View(account);
        }

        // GET: Login/Register
        public ActionResult Register()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;

            Account viewModel = new Account();

            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID > 0) return RedirectToAction("Index", "Management");

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return null;

            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID > 0) return RedirectToAction("Index", "Management");

            if (ModelState.IsValid)
            {
                if (account.Password == account.CheckPassword)
                {
                    if (!account.Username.Contains(' '))
                    {
                        if (AccountRepo.CheckAvailable(account.Username))
                        {
                            string randomstring = Login.GenerateRandomString(0, 128);
                            AccountRepo.InsertNewAccount(account.Username, Login.Encrypt(account.Password, randomstring), randomstring);
                            return RedirectToAction("RegisterSuccesfull", "Login");
                        }
                        else
                        {
                            account.ErrorMessage = "Username is not available";
                        }
                    }
                    else
                    {
                        account.ErrorMessage = "Username cannot contain spaces";
                    }
                }
                else
                {
                    account.ErrorMessage = "Passwords don't match!";
                }
            }


            return View(account);
        }

        public ActionResult RegisterSuccesfull()
        {
            Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true);
            return View();
        }

        public ActionResult Logout()
        {
            Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true);

            string[] allCookies = HttpContext.Request.Cookies.AllKeys;
            foreach (string cookie in allCookies)
            {
                HttpContext.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}