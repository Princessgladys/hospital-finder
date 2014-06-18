using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Doctor_Hospital table
    /// <summary>
    public class DoctorHospitalEntity
    {
        #region Doctor_Hospital Properties

        /// <summary>
        /// Property for Doctor_ID attribute
        /// <summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int HospitalID { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
