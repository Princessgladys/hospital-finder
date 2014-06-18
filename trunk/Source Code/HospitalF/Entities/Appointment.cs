using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Appointment table
    /// <summary>
    public class AppointmentEntity
    {
        #region Appointment Properties

        /// <summary>
        /// Property for Appointment_ID attribute
        /// <summary>
        public int AppointmentID { get; set; }

        /// <summary>
        /// Property for Patient_Full_Name attribute
        /// <summary>
        public string PatientFullName { get; set; }

        /// <summary>
        /// Property for Patient_Gender attribute
        /// <summary>
        public bool PatientGender { get; set; }

        /// <summary>
        /// Property for Patient_Birthday attribute
        /// <summary>
        public DateTime PatientBirthday { get; set; }

        /// <summary>
        /// Property for Patient_Phone_Number attribute
        /// <summary>
        public string PatientPhoneNumber { get; set; }

        /// <summary>
        /// Property for Patient_Email attribute
        /// <summary>
        public string PatientEmail { get; set; }

        /// <summary>
        /// Property for Appointment_Date attribute
        /// <summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Property for Start_Time attribute
        /// <summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Property for End_Time attribute
        /// <summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Property for In_Charge_Doctor attribute
        /// <summary>
        public int InChargeDoctor { get; set; }

        /// <summary>
        /// Property for Curing_Hospital attribute
        /// <summary>
        public int CuringHospital { get; set; }

        /// <summary>
        /// Property for Confirm_Code attribute
        /// <summary>
        public string ConfirmCode { get; set; }

        /// <summary>
        /// Property for Is_Confirm attribute
        /// <summary>
        public bool IsConfirm { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
