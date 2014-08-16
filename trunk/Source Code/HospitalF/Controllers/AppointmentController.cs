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
using System.Globalization;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace HospitalF.Controllers
{
    public class AppointmentController : SecurityBaseController
    {
        /*
        public static List<Speciality> specialityList = null;
        public static List<Doctor> doctorList = null;
        public static int[] workingDay = null;
        public static List<string> timeList = null;
        public static AppointmentModels model = null;

        #region Add Appointment
        //
        // GET: /Appointment/
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> Index(int hospitalID, int doctorID)
        {
            try
            {
                model = new AppointmentModels();

                Hospital hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalID);

                //load list of speciality
                List<int> doctor_Speciality;
                
                specialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
                
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                //load list of doctor
                doctorList = new List<Doctor>();
                if (doctorID != null)
                {
                    model.DoctorID = doctorID;
                    using (LinqDBDataContext data = new LinqDBDataContext())
                    {
                        doctor_Speciality = (from d in data.Doctor_Specialities where d.Doctor_ID == doctorID select d.Speciality_ID).ToList();
                        model.SpecialityID = doctor_Speciality[0];
                        //doctorList = await HospitalUtil.LoadDoctorInDoctorSpecialityAsyn(model.SpecialityID, hospitalID);
                    }
                    
                }
                ViewBag.DoctorList = new SelectList(doctorList, Constants.DoctorID, Constants.DoctorName);

                //load time to check health of hospital
                timeList = await AppointmentModels.LoadTimeCheckHealth(hospitalID);
                ViewBag.TimeList = new SelectList(timeList);

                model.HospitalName = hospital.Hospital_Name;
                model.HospitalID = hospitalID;
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    var result = (from hs in data.Hospital_Services
                                  where hs.Service_ID == 2 && hs.Hospital_ID == hospitalID
                                  select hs).FirstOrDefault();
                    if (result != null)
                    {
                        ViewBag.IsHealthInsurance = true;
                    }
                    else
                    {
                        ViewBag.IsHealthInsurance = false;
                    }
                }

                return View(model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Appointment/Index
        /// </summary>
        /// <param name="model">AppointmentModels</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpPost]
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> Index(AppointmentModels model)
        {
            ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
            ViewBag.DoctorList = new SelectList(doctorList, Constants.DoctorID, Constants.DoctorName);

            TimeSpan EndTime;
            try
            {
                string confirmCode = SMSUtil.GetConfirmCode();
                Hospital hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(model.HospitalID);
                EndTime = TimeSpan.Parse(model.StartTime).Add(new TimeSpan(0,(int) hospital.Avg_Curing_Time, 0));
                Appointment appointment = new Appointment();
                appointment.Patient_Full_Name = model.FullName;
                appointment.Patient_Gender = model.Gender == 0 ? true : false;
                appointment.Patient_Phone_Number = model.PhoneNo;
                appointment.Patient_Email = model.Email;
                appointment.In_Charge_Doctor = model.DoctorID;
                appointment.Confirm_Code = confirmCode;
                appointment.Start_Time = TimeSpan.Parse(model.StartTime);
                appointment.End_Time = EndTime;
                appointment.Curing_Hospital = model.HospitalID;
                appointment.Health_Insurance_Code = model.HealthInsuranceCode;
                appointment.Symptom_Description = model.SymptomDescription;
                appointment.Speciality_ID = model.SpecialityID;
                // convert model.birthday and model.appDate "dd/mm/yy" to mm/dd/yy
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA",true);
                string datetime = model.Birthday.ToString().Trim();
                DateTime dt = DateTime.Parse(datetime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                appointment.Patient_Birthday = dt;

                datetime = model.AppDate.ToString().Trim();
                dt = DateTime.Parse(datetime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                appointment.Appointment_Date = dt;

                int result = await AppointmentModels.InsertAppointment(appointment);
                if (result != 1)
                {
                    ViewBag.CreateAppointmentStatus = 1.ToString() + Constants.Minus + appointment.Patient_Full_Name;
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    //try
                    //{
                    //    SMSUtil.Send(model.PhoneNo, model.HospitalName, confirmCode);
                    //}
                    //catch (Exception ex)
                    //{
                    //    LoggingUtil.LogException(ex);

                    //}
                    ViewBag.CreateAppointmentStatus = 0.ToString() + Constants.Minus + appointment.Patient_Full_Name;
                    return View("Confirm");
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }
        #endregion

        #region Load ajax

        public async Task<ActionResult> LoadTimeCheckHealth(string hospitalID, string doctorID, string checkHealthDate)
        {
            try
            {
                List<string> timeList = null;
                int tempHospitalID = 0, tempDoctorID = 0;
                DateTime tempDate = new DateTime();
                if (!String.IsNullOrEmpty(hospitalID) && Int32.TryParse(hospitalID, out tempHospitalID) &&
                    !String.IsNullOrEmpty(doctorID) && Int32.TryParse(doctorID, out tempDoctorID) &&
                    !String.IsNullOrEmpty(checkHealthDate) && DateTime.TryParse(checkHealthDate, out tempDate))
                {
                    timeList = await AppointmentModels.LoadTimeCheckHealth(tempHospitalID, tempDoctorID, tempDate);
                    return Json(timeList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    timeList = new List<string>();
                    return Json(timeList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggingUtil.LogException(ex);
                // Move to error page
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Appointment/GetDoctorBySpeciality
        /// </summary>
        /// <param name="specialityId">SpecialityID ID</param>
        /// <returns>Task[ActionResult] with JSON contains list of Doctor</returns>
        public async Task<ActionResult> GetDoctorBySpeciality(string specialityID, string hospitalID)
        {
            try
            {
                int tempSpecialityID = 0, tempHospitalID = 0;
                // Check if city ID is null or not
                if (!String.IsNullOrEmpty(specialityID) && Int32.TryParse(specialityID, out tempSpecialityID) &&
                    !String.IsNullOrEmpty(hospitalID) && Int32.TryParse(hospitalID, out tempHospitalID))
                {
                    doctorList = await HospitalUtil.LoadDoctorInDoctorSpecialityAsyn(tempSpecialityID, tempHospitalID);
                    var result = (from d in doctorList
                                  select new
                                  {
                                      id = d.Doctor_ID,
                                      name = d.Last_Name + " " + d.First_Name
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
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                // Move to error page
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Appointment/
        /// </summary>
        /// <param name="specialityId">SpecialityID ID</param>
        /// <returns>Task[ActionResult] with JSON contains list of Doctor</returns>
        public async Task<ActionResult> GetWorkingDay(string doctorID)
        {
            try
            {
                Doctor doctor = null;
                int tempDoctorID = 0;
                string[] strWorkingDays = null;
                // Check if city ID is null or not
                if (!String.IsNullOrEmpty(doctorID) && doctorID != "0" && Int32.TryParse(doctorID, out tempDoctorID))
                {
                    doctor = await AppointmentModels.LoadDoctorInDoctorListAsyn(tempDoctorID);
                    strWorkingDays = doctor.Working_Day.Trim().Split(',');
                    workingDay = new int[strWorkingDays.Length];
                    for (int i = 0; i < strWorkingDays.Length; i++)
                    {
                        workingDay[i] = Convert.ToInt16(strWorkingDays[i].Trim());
                    }

                    return Json(workingDay, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Return default value
                    workingDay = null;
                    return Json(workingDay, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                // Move to error page
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }
        #endregion

        #region Confirm
        /// <summary>
        /// Get/Confirm
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Confirm()
        {
            return View();
        }

        /// <summary>
        /// Post:/Confirm
        /// </summary>
        /// <param name="confirmCode">confirm code</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost]
        public async Task<ActionResult> Confirm(AppointmentModels model)
        {
            try
            {
                ////validate recaptcha
                //RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();

                //if (String.IsNullOrEmpty(recaptchaHelper.Response))
                //{
                //    TempData["RateActionStatus"] = false;
                //    TempData["RateActionMessage"] = "Vui lòng nhập mã bảo mật bên dưới.";

                //    return RedirectToAction(Constants.HospitalAction, Constants.HomeController, new { hospitalId = id, redirect = "yes" });
                //}

                //RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();

                //if (recaptchaResult != RecaptchaVerificationResult.Success)
                //{
                //    TempData["RateActionStatus"] = false;
                //    TempData["RateActionMessage"] = "Vui lòng nhập lại mã bảo mật bên dưới.";

                //    return RedirectToAction(Constants.HospitalAction, Constants.HomeController, new { hospitalId = id, redirect = "yes" });
                //}

                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    Appointment appointment = (from a in data.Appointments
                                               where a.Confirm_Code == model.ConfirmCode &&
                                                     a.Is_Confirm==false
                                               select a).FirstOrDefault();
                    if (appointment != null)
                    {
                        DateTime today = DateTime.Now;
                        DateTime appointmentDate = DateTime.Parse(appointment.Appointment_Date.ToString()).Add(TimeSpan.Parse(appointment.Start_Time.ToString()));
                        // ngay gio confirm lon hon ngay hen kham
                        if (today.CompareTo(appointmentDate) > 0)
                        {
                            appointment.Is_Confirm = true;
                            appointment.Is_Active = false;
                        }
                            //ngay gio confirm trung ngay hen kham
                        else if (today.CompareTo(appointmentDate) == 0)
                        {
                            appointment.Is_Confirm = true;
                            appointment.Is_Active = true;
                        }
                            //ngay gio confirm nho hon ngay hen kham
                        else
                        {
                            appointment.Is_Confirm = true;
                            appointment.Is_Active = true;
                        }
                        data.SubmitChanges();

                        //--add column Speciality_ID
                        //USE HospitalF
                        //GO

                        //ALTER TABLE Appointment
                        //--ADD Speciality_ID INT

                        //ADD CONSTRAINT FK_Appointment_Speciality
                        //FOREIGN KEY (Speciality_ID)
                        //REFERENCES Speciality(Speciality_ID)

                        //sync event into google calendar
                        List<string> gmailList = (from u in data.Users
                                                  where u.Hospital_ID == appointment.Curing_Hospital
                                                  select u.Email).ToList();
                        Doctor doctor = (from d in data.Doctors
                                         where d.Doctor_ID == appointment.In_Charge_Doctor
                                         select d).FirstOrDefault();

                        Speciality speciality = (from s in data.Specialities
                                                 where s.Speciality_ID == appointment.Speciality_ID
                                                 select s).FirstOrDefault();
                        GoogleUtil.InsertEventToCalendar(appointment, doctor, speciality, gmailList);
                        return RedirectToAction(Constants.IndexAction, Constants.HomeController);
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

        }
        #endregion
         * */
    }
}
