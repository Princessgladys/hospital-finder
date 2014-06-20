using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using HospitalF.App_Start;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.App_Start;
using HospitalF.Utilities;

namespace HospitalF.Controllers
{
    public class AppointmentController : Controller
    {
        public static List<Speciality> specialityList = null;
        public static int hospitalID = 64;
        //
        // GET: /Appointment/
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> Index()
        {
            try
            {
                //load list of speciality
                specialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
            }
            catch (Exception)
            {
                return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
            }
            return View();
        }
        /// <summary>
        /// GET: /Appointment/Index
        /// </summary>
        /// <param name="model">AppointmentModels</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpGet]
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> CreateAppointment(AppointmentModels model)
        {
            return View();
        }
    }
}
