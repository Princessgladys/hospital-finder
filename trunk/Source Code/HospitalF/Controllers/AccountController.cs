using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HospitalF.App_Start;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.Utilities;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace HospitalF.Controllers
{
    public class AccountController : SecurityBaseController
    {
        #region VietLP

        //
        // GET: /Account/
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.ErrorMesage = (string)TempData["ErrorMesage"];
            return View();
        }

        /// <summary>
        /// Check if an account is valid or not
        /// </summary>
        /// <param name="email">Input email</param>
        /// <param name="password">Input password</param>
        /// <returns>Boolean value indicating an account is valid or not</returns>
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                // Check if an account is valid or not
                bool checkLogin = AccountModels.CheckLogin(email, password);

                if (checkLogin)
                {
                    // Check if user is Administrator
                    if (SimpleSessionPersister.Role.Equals(Constants.AdministratorRoleName))
                    {
                        return RedirectToAction(Constants.InitialHospitalListAction, Constants.HospitalController);
                    }

                    // Check if user is Hospital User
                    if (SimpleSessionPersister.Role.Equals(Constants.HospitalUserRoleName))
                    {

                    }

                    // Redirect to Login page as default
                    return RedirectToAction(Constants.LoginAction, Constants.AccountController);
                }
                else
                {
                    TempData["ErrorMesage"] = "Sai thông tin đăng nhập";
                    return RedirectToAction(Constants.IndexAction, Constants.AccountController);
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [HttpGet]
        public ActionResult FacebookLogin(string token)
        {
            try
            {
                WebClient client = new WebClient();
                string jsonResult = client.DownloadString(string.Concat("https://graph.facebook.com/me?access_token=", token));
                // Json.Net is really helpful if you have to deal
                // with Json from .Net http://json.codeplex.com/
                JObject jsonUserInfo = JObject.Parse(jsonResult);

                bool checkFacebookLogin = AccountModels.CheckFacebookLogin(jsonUserInfo);

                return RedirectToAction(Constants.IndexAction, Constants.HomeController);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
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
                return RedirectToAction(Constants.LoginAction, Constants.AccountController);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        #region SonNX

        /// <summary>
        /// Load paritial view Add Account
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult AddAccount()
        {
            return PartialView(Constants.AddAccountAction);
        }

        /// <summary>
        /// POST: /Account/AddAccount
        /// </summary>
        /// <returns></returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> AddAccount(AccountModels model)
        {
            try
            {
                int result = 0;

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.InsertHospitalUserAsync(model);
                }

                // Check returned result
                if (result == 1)
                {
                    return RedirectToAction(Constants.AddHospitalAction, Constants.HospitalController);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion
    }
}
