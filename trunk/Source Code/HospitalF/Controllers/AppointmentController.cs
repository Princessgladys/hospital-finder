using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalF.App_Start;
using HospitalF.Constant;

namespace HospitalF.Controllers
{
    public class AppointmentController : Controller
    {
        //
        // GET: /Appointment/
        [LayoutInjecter(Constants.HomeLayout)]
        public ActionResult CreateAppointment()
        {
            return View();
        }

    }
}
