using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalF.App_Start;
using HospitalF.Constant;

namespace HospitalF.Controllers
{
    public class CalendarController : SecurityBaseController
    {
        //
        // GET: /Calendar/
        [LayoutInjecter(Constants.HospitalUserLayout)]
        //[Authorize(Roles=Constants.HospitalUserRoleName)]
        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
