using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for District table
    /// <summary>
    public class DistrictEntity
    {
        #region District Properties

        /// <summary>
        /// Property for District_ID attribute
        /// <summary>
        public int DistrictID { get; set; }

        /// <summary>
        /// Property for District_Name attribute
        /// <summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// Property for Type attribute
        /// <summary>
        public string Type { get; set; }

        /// <summary>
        /// Property for Coordinate attribute
        /// <summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// Property for City_ID attribute
        /// <summary>
        public int CityID { get; set; }

        #endregion
    }
}
