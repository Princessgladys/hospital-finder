using System;
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

        #region Load doctor
        /// <summary>
        /// Load all doctor in Doctor_Speciality
        /// </summary>
        /// <param name="SpecialityID"></param>
        /// <returns>List[DoctorEnity] that contains list of doctor with appropriate Speciality code</returns>
        public static async Task<List<Doctor>> LoadDoctorInDoctorSpecialityAsyn(int SpecialityID)
        {
            List<Doctor> doctorList = new List<Doctor>();
            Doctor doctor = null;
            List<SP_LOAD_DOCTOR_BY_SPECIALITYIDResult> result = null;
            // Take doctor in specific speciality in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                data.SP_LOAD_DOCTOR_BY_SPECIALITYID(SpecialityID).ToList());
            }
            // Assign value for each doctor
            foreach (SP_LOAD_DOCTOR_BY_SPECIALITYIDResult r in result)
            {
                doctor = new Doctor();
                doctor.Doctor_ID = r.Doctor_ID;
                doctor.First_Name = r.First_Name;
                doctor.Last_Name = r.Last_Name;
                doctorList.Add(doctor);
            }
            Appointment app = new Appointment();
            return doctorList;
        }
        #endregion

        #region
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

        #region Load hospital by hospital ID
        public static async Task<Hospital> LoadHospitalByHospitalID(int hospitalID)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from h in data.Hospitals
                     where h.Hospital_ID == hospitalID
                     select h).FirstOrDefault());
            }
        }
        #endregion

        #region Create time to check health
        public static async Task<List<string>> LoadTimeCheckHealth(int hospitalID)
        {
            List<string> listTime = new List<string>();
            //Hospital hospital = await LoadHospitalByHospitalID(hospitalID);
            //TimeSpan start = (TimeSpan)hospital.Start_Time;
            //TimeSpan end = (TimeSpan)hospital.End_Time;
            //int timeCheck = (int)hospital.Time;
            //DateTime dt = DateTime.Today.Add(start);
            //listTime.Add(dt.ToString("HH:mm"));
            //string t1 = dt.ToString("hh:mm tt");
            //string t2 = dt.ToString("HH:mm tt");
            //for (TimeSpan time = start; time >= start && time < end; time = time.Add(new TimeSpan(1, 0, 0)))
            //{
            //    for (int i = 0; i <= 60; i += timeCheck)
            //    {
            //        dt=DateTime.Today.Add(time.Add(new TimeSpan(0,i,0)));
            //        if (!listTime[listTime.Count-1].Equals(dt.ToString("HH:mm")) &&
            //            dt!=DateTime.Today.Add( end))
            //        {
            //            listTime.Add(dt.ToString("HH:mm"));
            //        }
            //    }
            //}
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