using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HospitalF.App_Start;
using HospitalF.Constant;
using HospitalF.Models;

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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMesage"] = "Sai thông tin đăng nhập";
                return RedirectToAction("Index", "Account");
            }

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
                return RedirectToAction("SystemFailureHome", "Error");
            }
            catch (Exception)
            {
                return RedirectToAction("SystemFailureHome", "Error");
            }
        }

    }
}
