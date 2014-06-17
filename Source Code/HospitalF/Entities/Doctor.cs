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
        public int DoctorID;

        /// <summary>
        /// Property for First_Name attribute
        /// <summary>
        public string FirstName;

        /// <summary>
        /// Property for Last_Name attribute
        /// <summary>
        public string LastName;

        /// <summary>
        /// Property for Gender attribute
        /// <summary>
        public bool Gender;

        /// <summary>
        /// Property for Degree attribute
        /// <summary>
        public string Degree;

        /// <summary>
        /// Property for Experience attribute
        /// <summary>
        public string Experience;

        /// <summary>
        /// Property for Working_Day attribute
        /// <summary>
        public string WorkingDay;

        /// <summary>
        /// Property for Photo_ID attribute
        /// <summary>
        public int? PhotoID;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive;

        #endregion
    }
}
