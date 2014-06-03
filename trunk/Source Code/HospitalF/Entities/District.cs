using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for District table
    /// <summary>
    public class District
    {
        #region District Properties

        /// <summary>
        /// Property for District_ID attribute
        /// <summary>
        public int DistrictID;

        /// <summary>
        /// Property for District_Name attribute
        /// <summary>
        public string DistrictName;

        /// <summary>
        /// Property for Type attribute
        /// <summary>
        public string Type;

        /// <summary>
        /// Property for Coordinate attribute
        /// <summary>
        public string Coordinate;

        /// <summary>
        /// Property for City_ID attribute
        /// <summary>
        public int? CityID;

        #endregion
    }
}
