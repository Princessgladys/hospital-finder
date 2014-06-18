using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Doctor table
    /// <summary>
    public class DoctorEntity
    {
        #region Doctor Properties

        /// <summary>
        /// Property for Doctor_ID attribute
        /// <summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Property for First_Name attribute
        /// <summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Property for Last_Name attribute
        /// <summary>
        public string LastName { get; set; }

        /// <summary>
        /// Property for Gender attribute
        /// <summary>
        public bool Gender { get; set; }

        /// <summary>
        /// Property for Degree attribute
        /// <summary>
        public string Degree { get; set; }

        /// <summary>
        /// Property for Experience attribute
        /// <summary>
        public string Experience { get; set; }

        /// <summary>
        /// Property for Working_Day attribute
        /// <summary>
        public string WorkingDay { get; set; }

        /// <summary>
        /// Property for Photo_ID attribute
        /// <summary>
        public int? PhotoID { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
