using System;
namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Facility table
    /// <summary>
    public class FacilityEntity
    {
        #region Facility Properties

        /// <summary>
        /// Property for Facility_ID attribute
        /// <summary>
        public int FacilityID { get; set; }

        /// <summary>
        /// Property for Facility_Name attribute
        /// <summary>
        public string FacilityName { get; set; }

        #endregion
    }
}
