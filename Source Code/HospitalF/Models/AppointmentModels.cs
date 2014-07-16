﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using HospitalF.Constant;
using System.Text.RegularExpressions;

namespace HospitalF.Models
{
    public class AppointmentModels
    {
        #region Properties

        /// <summary>
        /// Get/set value for property FullName
        /// </summary>
        [Display(Name = Constants.FullName)]
        [Required(ErrorMessage = ErrorMessage.CEM001)]
        [StringLength(32, ErrorMessage = ErrorMessage.CEM003)]
        public string FullName { get; set; }

        /// <summary>
        /// Get/set value for property Gender
        /// </summary>
        [Display(Name = Constants.Gender)]
        public int Gender { get; set; }

        /// <summary>
        /// Get/set value for property Birthday
        /// </summary>
        [Display(Name = Constants.Birthday)]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Get/set value for property Email
        /// </summary>
        [Display(Name = Constants.Email)]
        [RegularExpression(Constants.EmailRegex, ErrorMessage = ErrorMessage.CEM005)]
        public string Email { get; set; }

        /// <summary>
        /// Get/set value for property PhoneNo
        /// </summary>
        [Display(Name = Constants.PhoneNo)]
        [Required(ErrorMessage = ErrorMessage.CEM001)]
        [RegularExpression(Constants.CellPhoneNoRegex, ErrorMessage = ErrorMessage.CEM005)]
        public string PhoneNo { get; set; }

        /// <summary>
        /// Get/set value for property Speciality_ID
        /// </summary>
        [Display(Name = Constants.Speciality)]
        public int SpecialityID { get; set; }

        /// <summary>
        /// Get/Set value for property Speciality_Name
        /// </summary>
        public string SpecialityName { get; set; }

        /// <summary>
        /// Get/set value for property Doctor_ID
        /// </summary>
        [Display(Name = Constants.Doctor)]
        public int DoctorID { get; set; }

        /// <summary>
        /// Get/set value for property Doctor_Name
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// Get/set value for property App_Date
        /// </summary>
        [Display(Name = Constants.App_Date)]
        public DateTime AppDate { get; set; }

        /// <summary>
        /// Get/set value for property StartTime
        /// </summary>
        [Display(Name = Constants.StartTime)]
        public string StartTime { get; set; }

        #endregion

        #region Load doctor in list of doctor
        public static async Task<Doctor> LoadDoctorInDoctorListAsyn(int DoctorID)
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from d in data.Doctors
                     where d.Doctor_ID == DoctorID
                     select d).FirstOrDefault());
            }
        }
        #endregion

        #region Create time to check health
        public static async Task<List<string>> LoadTimeCheckHealth(int hospitalID)
        {
            List<string> listTime = new List<string>();
            Hospital hospital = await LoadHospitalByHospitalID(hospitalID);
            TimeSpan ordinaryStart = (TimeSpan)hospital.Ordinary_Start_Time;
            TimeSpan ordinaryEnd = (TimeSpan)hospital.Ordinary_End_Time;
            TimeSpan holidayStart = (TimeSpan)hospital.Holiday_Start_Time;
            TimeSpan holidayEnd = (TimeSpan)hospital.Holiday_End_Time; 
            int timeCheck = 20;
            DateTime dt = DateTime.Today.Add(ordinaryStart);
            listTime.Add(dt.ToString("HH:mm"));
            string t1 = dt.ToString("hh:mm tt");
            string t2 = dt.ToString("HH:mm tt");
            for (TimeSpan time = ordinaryStart; time >= ordinaryStart && time < ordinaryEnd; time = time.Add(new TimeSpan(1, 0, 0)))
            {
                for (int i = 0; i <= 60; i += timeCheck)
                {
                    dt=DateTime.Today.Add(time.Add(new TimeSpan(0,i,0)));
                    if (!listTime[listTime.Count-1].Equals(dt.ToString("HH:mm")) &&
                        dt != DateTime.Today.Add(ordinaryEnd))
                    {
                        listTime.Add(dt.ToString("HH:mm"));
                    }
                }
            }
            return listTime;
        }
        #endregion

        #region Insert into database
        public static async Task<int> InsertAppointment(Appointment app)
        {
            int result = 0;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                //result = data.SP_INSERT_APPOINTMENT(null, null, null, null, null, null, null, null, null, null,null);
                result = await Task.Run(() => data.SP_INSERT_APPOINTMENT(app.Patient_Full_Name, app.Patient_Gender,
                    app.Patient_Birthday, app.Patient_Phone_Number, app.Patient_Email,
                    app.Appointment_Date, app.Start_Time, app.End_Time, app.In_Charge_Doctor,
                    app.Curing_Hospital, app.Confirm_Code));
                //result = await Task.Run(() => data.SP_INSERT_APPOINTMENT("Nguyen Thi A", true, DateTime.Parse("1991-01-03"), "0908616730", "a@a.com.vn",
                //     DateTime.Parse("2014-07-03"), null, null, 1, 25, "AABBCCDD"));
            }
            return result;
        }
        #endregion

        #region create confirm code
        public static string CreateConfirmCode()
        {
            string confirmCode = null;
            return confirmCode;
        }
        #endregion
    }
}