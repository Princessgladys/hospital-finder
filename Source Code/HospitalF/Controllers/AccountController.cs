using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HospitalF.App_Start;
using HospitalF.Constant;
using HospitalF.Models;
using Newtonsoft.Json.Linq;

namespace HospitalF.Controllers
{
    public class AccountController : SecurityBaseController
    {
        //
        // GET: /Account/
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ErrorMesage = (string)TempData["ErrorMesage"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            bool checkLogin = AccountModel.CheckLogin(email, password);
            if (checkLogin)
            {
                return RedirectToAction(Constants.IndexAction, Constants.AccountController);
            }
            else
            {
                TempData["ErrorMesage"] = "Sai thông tin đăng nhập";
                return RedirectToAction(Constants.IndexAction, Constants.AccountController);
            }

        }

        [HttpGet]
        public ActionResult FacebookLogin(string token)
        {
            WebClient client = new WebClient();
            string jsonResult = client.DownloadString(string.Concat("https://graph.facebook.com/me?access_token=", token));
            // Json.Net is really helpful if you have to deal
            // with Json from .Net http://json.codeplex.com/
            JObject jsonUserInfo = JObject.Parse(jsonResult);

            bool checkFacebookLogin = AccountModel.CheckFacebookLogin(jsonUserInfo);

            return RedirectToAction(Constants.IndexAction, Constants.HomeController);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Abandon();

                // clear authentication cookie
                HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                cookie1.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie1);

                // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
                cookie2.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie2);
                return RedirectToAction(Constants.IndexAction, Constants.HomeController);
            }
            catch (Exception)
            {
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }
        
    }
}
