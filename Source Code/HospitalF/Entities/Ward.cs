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
        public int WardID;

        /// <summary>
        /// Property for Ward_Name attribute
        /// <summary>
        public string WardName;

        /// <summary>
        /// Property for Type attribute
        /// <summary>
        public string Type;

        /// <summary>
        /// Property for Coordinate attribute
        /// <summary>
        public string Coordinate;

        /// <summary>
        /// Property for District_ID attribute
        /// <summary>
        public int DistrictID;

        #endregion
    }
}
