using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using HospitalF.Constant;
using System.Text.RegularExpressions;
using HospitalF.Utilities;

namespace HospitalF.Models
{
    public class AppointmentModels
    {
        #region Properties

        /// <summary>
        /// Get/set value for property HospitalName
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// Get/set value for property HospitalID
        /// </summary>
        public int HospitalID { get; set; }

        /// <summary>
        /// Get/set value for property FullName
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Get/set value for property Gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Get/set value for property Birthday
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Get/set value for property Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get/set value for property PhoneNo
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// Get/set value for property Speciality_ID
        /// </summary>
        public int SpecialityID { get; set; }

        /// <summary>
        /// Get/Set value for property Speciality_Name
        /// </summary>
        public string SpecialityName { get; set; }

        /// <summary>
        /// Get/set value for property Doctor_ID
        /// </summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Get/set value for property Doctor_Name
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// Get/set value for property App_Date
        /// </summary>
        public DateTime AppDate { get; set; }

        /// <summary>
        /// Get/set value for property StartTime
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Get/set value for property HealthInsurancecode
        /// </summary>
        public string HealthInsuranceCode { get; set; }

        /// <summary>
        /// Get/set value for property SymptomDescription
        /// </summary>
        public string SymptomDescription { get; set; }

        /// <summary>
        /// Get/set value for property ConfirmCode
        /// </summary>
        public string ConfirmCode { get; set; }

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
        /// <summary>
        /// AppointmentModels/LoadTimeCheckHealth
        /// </summary>
        /// <param name="hospitalID">Hospital id</param>
        /// <returns>Task<List<string>> that contains all time</returns>
        public static async Task<List<string>> LoadTimeCheckHealth(int hospitalID)
        {
            List<string> listTime = new List<string>();
            Hospital hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalID);
            TimeSpan ordinaryStart = (TimeSpan)hospital.Ordinary_Start_Time;
            TimeSpan ordinaryEnd = (TimeSpan)hospital.Ordinary_End_Time; ;
            TimeSpan holidayStart = (TimeSpan)hospital.Holiday_Start_Time; 
            TimeSpan holidayEnd = (TimeSpan)hospital.Holiday_End_Time; ;
            
            DateTime dt = DateTime.Today.Add(ordinaryStart);
            listTime.Add(dt.ToString("HH:mm"));
            //string t1 = dt.ToString("hh:mm tt");
            //string t2 = dt.ToString("HH:mm tt");
            for (TimeSpan time = ordinaryStart; time >= ordinaryStart && time < ordinaryEnd; time = time.Add(new TimeSpan(1, 0, 0)))
            {
                for (int i = 0; i <= 60; i += (int)hospital.Avg_Curing_Time)
                {
                    dt = DateTime.Today.Add(time.Add(new TimeSpan(0, i, 0)));
                    if (!listTime[listTime.Count - 1].Equals(dt.ToString("HH:mm")) &&
                        dt != DateTime.Today.Add(ordinaryEnd))
                    {
                        listTime.Add(dt.ToString("HH:mm"));
                    }
                }
            }
            return listTime;
        }

        /// <summary>
        /// AppointmentModels/LoadtimeCheckHealth
        /// </summary>
        /// <param name="hospitalID">hospital ID</param>
        /// <param name="doctorID">doctor ID</param>
        /// <param name="checkHealthDate">check health date</param>
        /// <returns></returns>
        public static async Task<List<string>> LoadTimeCheckHealth(int hospitalID, int doctorID, DateTime checkHealthDate)
        {
            List<string> listTime = await LoadTimeCheckHealth(hospitalID);
            List<string> checkTimeList =await LoadAppointmentTime(hospitalID, doctorID, checkHealthDate);
            foreach (string checkTime in checkTimeList)
            {
                foreach (string item in listTime)
                {
                    if (item.Equals(checkTime))
                    {
                        listTime.Remove(item);
                        break;
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
                result = await Task.Run(() => data.SP_INSERT_APPOINTMENT(app.Patient_Full_Name, app.Patient_Gender,
                    app.Patient_Birthday, app.Patient_Phone_Number, app.Patient_Email,
                    app.Appointment_Date, app.Start_Time, app.End_Time, app.In_Charge_Doctor,
                    app.Curing_Hospital, app.Confirm_Code, app.Health_Insurance_Code, app.Symptom_Description,app.Speciality_ID));
            }
            return result;
        }
        #endregion

        #region get appointment time of each day
        private async static Task<List<String>> LoadAppointmentTime(int hospitalID, int doctorID, DateTime checkHealthDate)
        {
            List<String> timeList = new List<string>();
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                List<Appointment> result =await Task.Run(()=>(from a in data.Appointments
                              where a.In_Charge_Doctor == doctorID && a.Curing_Hospital == hospitalID &&
                                      a.Appointment_Date == checkHealthDate
                              select a).ToList());
                foreach (Appointment a in result)
                {
                    timeList.Add(DateTime.Today.Add((TimeSpan)a.Start_Time).ToString("HH:mm"));
                }
            }
            return timeList;
        }

        #endregion
    }
}