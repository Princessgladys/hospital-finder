using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalF.App_Start;
using HospitalF.Constant;

namespace HospitalF.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// GET: /Error/SystemFailureHome
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.HomeLayout)]
        public ActionResult SystemFailureHome()
        {
            ViewBag.SEM001 = ErrorMessage.SEM001;
            return View();
        }

        /// <summary>
        /// GET: /Error/SystemFailureAdmin
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        public ActionResult SystemFailureAdmin()
        {
            ViewBag.SEM001 = ErrorMessage.SEM001;
            return View();
        }
    }
}
