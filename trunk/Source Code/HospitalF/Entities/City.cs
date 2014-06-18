using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for City table
    /// <summary>
    public class CityEntity
    {
        #region City Properties

        /// <summary>
        /// Property for City_ID attribute
        /// <summary>
        public int CityID { get; set; }

        /// <summary>
        /// Property for City_Name attribute
        /// <summary>
        public string CityName { get; set; }

        /// <summary>
        /// Property for Type attribute
        /// <summary>
        public string Type { get; set; }

        #endregion
    }
}
