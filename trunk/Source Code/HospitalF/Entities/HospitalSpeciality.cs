using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Hospital_Speciality table
    /// <summary>
    public class HospitalSpeciality
    {
        #region Hospital_Speciality Properties

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int HospitalID;

        /// <summary>
        /// Property for Speciality_ID attribute
        /// <summary>
        public int SpecialityID;

        /// <summary>
        /// Property for Is_Main_Speciality attribute
        /// <summary>
        public bool? IsMainSpeciality;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive;

        #endregion
    }
}
