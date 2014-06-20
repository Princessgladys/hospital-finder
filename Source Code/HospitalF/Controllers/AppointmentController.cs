using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using HospitalF.App_Start;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.Utilities;

namespace HospitalF.Controllers
{
    public class AppointmentController : Controller
    {
        public static List<Speciality> specialityList = null;
        public static List<Doctor> doctorList = null;
        public static int hospitalID = 23;
        //
        // GET: /Appointment/
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> CreateAppointment()
        {
            try
            {
                //load list of speciality
                
                specialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                //load list of doctor
                doctorList = new List<Doctor>();
                ViewBag.DoctorList = new SelectList(doctorList, Constants.DoctorID, Constants.DoctorName);
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
        [HttpPost]
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> CreateAppointment_Create(AppointmentModels model)
        {
            ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
            ViewBag.DoctorList = new SelectList(doctorList, Constants.DoctorID, Constants.DoctorName);
            if (!ModelState.IsValid)
            {

                return View();
            }
            else
            {
                TimeSpan EndTime;
                try
                {
                    EndTime = TimeSpan.Parse(model.StartTime).Add(new TimeSpan(0,20,0));
                    Appointment app = new Appointment();
                    app.Patient_Full_Name = model.FullName;
                    app.Patient_Gender = model.Gender == 0 ? true : false;
                    app.Patient_Phone_Number = model.PhoneNo;
                    app.Patient_Email = model.Email;
                    app.Patient_Birthday = model.Birthday;
                    app.In_Charge_Doctor = model.DoctorID;
                    //app.Hospital.Hospital_ID = model.hospitalID;
                    //app.Confirm_Code = AppointmentModels.CreateConfirmCode();
                    app.Confirm_Code = "AaBbCcDd";
                    app.Start_Time = TimeSpan.Parse(model.StartTime);
                    app.End_Time = EndTime;
                    app.Appointment_Date = model.AppDate;
                    app.Curing_Hospital = hospitalID;
                    int result =await AppointmentModels.InsertAppointment(app);
                    if (result != 1)
                    {
                        return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
                    }
                    else
                    {
                        return View("Confirm");
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
                }
            }
        }

        /// <summary>
        /// GET: /Appointment/GetDoctorBySpeciality
        /// </summary>
        /// <param name="specialityId">SpecialityID ID</param>
        /// <returns>Task[ActionResult] with JSON contains list of Doctor</returns>
        public async Task<ActionResult> GetDoctorBySpeciality(string specialityID)
        {
            try
            {
                int tempSpecialityID = 0;
                // Check if city ID is null or not
                if (!String.IsNullOrEmpty(specialityID) && Int32.TryParse(specialityID, out tempSpecialityID))
                {
                    doctorList = await AppointmentModels.LoadDoctorInDoctorSpecialityAsyn(tempSpecialityID);
                    var result = (from d in doctorList
                                  select new
                                  {
                                      name = d.First_Name + " " + d.Last_Name
                                  });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Return default value
                    doctorList = new List<Doctor>();
                    return Json(doctorList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                // Move to error page
                return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Confirm()
        {
            return View();
        }
    }
}
