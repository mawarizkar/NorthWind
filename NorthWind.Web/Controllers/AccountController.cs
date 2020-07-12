using NorthWind.Providers;
using NorthWind.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NorthWind.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountProvider provider;

        public AccountController() {
            this.provider = new AccountProvider();
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginVM viewModel = new LoginVM();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginVM viewModel) {
            string returnUrl = String.Empty;
            string queryString = Server.UrlDecode(Request.UrlReferrer.Query);
            if (queryString != "") {
                returnUrl = queryString.Split('=')[1];
            }
            if (ModelState.IsValid) {
                viewModel.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(viewModel.Password, "SHA1");
                bool isAuthenticated = provider.IsAuthenticated(viewModel);
                if (isAuthenticated) {
                    //Session["Role"] = provider.GetRole(viewModel.Username);//Session mencatat role dari user.
                    FormsAuthentication.SetAuthCookie(viewModel.Username, false);
                    if (returnUrl != String.Empty) {
                        return Redirect(returnUrl); //Kalau ada Return Url nya
                    }
                    return RedirectToAction("CategoryIndex", "Product");
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register() 
        {
            var viewModel = new RegisterVM();
            viewModel.RolesDropdown = new List<DropDownVM>() {
                new DropDownVM{ Text = "user", Value = 0 },
                new DropDownVM{ Text = "manager", Value = 0},
                new DropDownVM{ Text = "admin", Value = 0}
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Register(RegisterVM viewModel) {
            if (ModelState.IsValid) {
                viewModel.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(viewModel.Password, "SHA1");
                provider.RegisterAccount(viewModel);
                return RedirectToAction("Login");
            }
            viewModel.RolesDropdown = new List<DropDownVM>() {
                new DropDownVM{ Text = "user", Value = 0 },
                new DropDownVM{ Text = "manager", Value = 0},
                new DropDownVM{ Text = "admin", Value = 0}
            };
            return View(viewModel);
        }
    }
}