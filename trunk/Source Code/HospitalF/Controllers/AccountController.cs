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
        public ActionResult Login(string email, string password, bool remember = false)
        {
            try
            {
                // Check if an account is valid or not
                bool checkLogin = AccountModel.CheckLogin(email, password);

                if (checkLogin)
                {
                    string returlUrl = "/";
                    string requestQuery = Server.UrlDecode(Request.UrlReferrer.Query);
                    if (string.IsNullOrEmpty(requestQuery))
                    {
                        // Check if user is Administrator
                        if (SimpleSessionPersister.Role.Equals(Constants.AdministratorRoleName))
                        {
                            return RedirectToAction(Constants.InitialHospitalListAction, Constants.HospitalController);
                        }

                        // Check if user is Hospital User
                        if (SimpleSessionPersister.Role.Equals(Constants.HospitalUserRoleName))
                        {
                            return RedirectToAction(Constants.IndexAction, Constants.HospitalController);
                        }
                    }
                    else if (requestQuery.Contains("ReturnUrl"))
                    {
                        returlUrl = requestQuery.Split('=')[1];
                    }
                    return Redirect(returlUrl);
                }
                else
                {
                    TempData["ErrorMesage"] = "Sai thông tin đăng nhập";
                    return RedirectToAction(Constants.LoginAction, Constants.AccountController);
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

                bool checkFacebookLogin = AccountModel.CheckFacebookLogin(jsonUserInfo);

                string returlUrl = "/";

                string urlReferrer = Server.UrlDecode(Request.UrlReferrer.AbsoluteUri);

                if (!string.IsNullOrEmpty(urlReferrer))
                {
                    returlUrl = urlReferrer;
                }
               
                return Redirect(returlUrl);
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
                return RedirectToAction(Constants.IndexAction, Constants.HomeController);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        public ActionResult ActivateUser(int userId = 0)
        {
            AccountModel.ActivateUser(userId);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult DeactivateUser(int userId = 0)
        {
            AccountModel.DeactivateUser(userId);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
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
        /// <param name="email">Email</param>
        /// <param name="secondEmail">Secondary email</param>
        /// <param name="firstName">Firstname</param>
        /// <param name="lastName">Lastname</param>
        /// <param name="phoneNumber">Phone number</param>
        /// <returns>AJAX result</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> AddAccount(string email, string secondEmail,
            string firstName, string lastName, string phoneNumber)
        {
            try
            {
                int result = 0;

                // Prepare data
                AccountModel model = new AccountModel();
                model.Email = email;
                model.SecondaryEmail = secondEmail;
                model.FirstName = firstName;
                model.LastName = lastName;
                model.PhoneNumber = phoneNumber;
                model.ConfirmedPerson = Int32.Parse(
                    User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.InsertHospitalUserAsync(model);
                }

                // Check returned result
                if (result == 1)
                {
                    return Json(new
                    {
                        result = 1.ToString() +
                            Constants.VerticalBar + model.Email
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        result = 0.ToString() +
                            Constants.VerticalBar + model.Email
                    }, JsonRequestBehavior.AllowGet);
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
