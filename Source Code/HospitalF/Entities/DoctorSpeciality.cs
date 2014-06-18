using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Doctor_Speciality table
    /// <summary>
    public class DoctorSpecialityEntity
    {
        #region Doctor_Speciality Properties

        /// <summary>
        /// Property for Doctor_ID attribute
        /// <summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Property for Speciality_ID attribute
        /// <summary>
        public int SpecialityID { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive { get; set; }

        #endregion
    }
}
