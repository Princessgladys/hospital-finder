using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Doctor_Speciality table
    /// <summary>
    public class DoctorSpeciality
    {
        #region Doctor_Speciality Properties

        /// <summary>
        /// Property for Doctor_ID attribute
        /// <summary>
        public int DoctorID;

        /// <summary>
        /// Property for Speciality_ID attribute
        /// <summary>
        public int SpecialityID;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive;

        #endregion
    }
}
