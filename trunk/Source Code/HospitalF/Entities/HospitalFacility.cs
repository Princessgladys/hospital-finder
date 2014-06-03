using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Hospital_Facility table
    /// <summary>
    public class HospitalFacility
    {
        #region Hospital_Facility Properties

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int HospitalID;

        /// <summary>
        /// Property for Facility_ID attribute
        /// <summary>
        public int FacilityID;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive;

        #endregion
    }
}
