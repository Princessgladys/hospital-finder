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
        public int AppointmentID;

        /// <summary>
        /// Property for Patient_First_Name attribute
        /// <summary>
        public string PatientFirstName;

        /// <summary>
        /// Property for Patient_Last_Name attribute
        /// <summary>
        public string PatientLastName;

        /// <summary>
        /// Property for Patient_Gender attribute
        /// <summary>
        public bool? PatientGender;

        /// <summary>
        /// Property for Patient_Birthday attribute
        /// <summary>
        public DateTime? PatientBirthday;

        /// <summary>
        /// Property for Patient_Phone_Number attribute
        /// <summary>
        public string PatientPhoneNumber;

        /// <summary>
        /// Property for Patient_Email attribute
        /// <summary>
        public string PatientEmail;

        /// <summary>
        /// Property for Appointment_Date attribute
        /// <summary>
        public DateTime? AppointmentDate;

        /// <summary>
        /// Property for Start_Time attribute
        /// <summary>
        public TimeSpan? StartTime;

        /// <summary>
        /// Property for End_Time attribute
        /// <summary>
        public TimeSpan? EndTime;

        /// <summary>
        /// Property for In_Charge_Doctor attribute
        /// <summary>
        public int? InChargeDoctor;

        /// <summary>
        /// Property for Curing_Hospital attribute
        /// <summary>
        public int? CuringHospital;

        /// <summary>
        /// Property for Created_Person attribute
        /// <summary>
        public int? CreatedPerson;

        /// <summary>
        /// Property for Is_Confirm attribute
        /// <summary>
        public bool? IsConfirm;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive;

        #endregion
    }
}
