using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Ward table
    /// <summary>
    public class WardEntity
    {
        #region Ward Properties

        /// <summary>
        /// Property for Ward_ID attribute
        /// <summary>
        public int WardID { get; set; }

        /// <summary>
        /// Property for Ward_Name attribute
        /// <summary>
        public string WardName { get; set; }

        /// <summary>
        /// Property for Type attribute
        /// <summary>
        public string Type { get; set; }

        /// <summary>
        /// Property for Coordinate attribute
        /// <summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// Property for District_ID attribute
        /// <summary>
        public int DistrictID { get; set; }

        #endregion
    }
}
