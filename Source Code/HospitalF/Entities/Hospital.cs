using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Hospital table
    /// <summary>
    public class HospitalEntity
    {
        #region Hospital Properties

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int HospitalID { get; set; }

        /// <summary>
        /// Property for Hospital_Name attribute
        /// <summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// Property for Hospital_Type attribute
        /// <summary>
        public int HospitalType { get; set; }

        /// <summary>
        /// Property for Address attribute
        /// <summary>
        public string Address { get; set; }

        /// <summary>
        /// Property for Ward_ID attribute
        /// <summary>
        public int WardID { get; set; }

        /// <summary>
        /// Property for District_ID attribute
        /// <summary>
        public int DistrictID { get; set; }

        /// <summary>
        /// Property for City_ID attribute
        /// <summary>
        public int CityID { get; set; }

        /// <summary>
        /// Property for Phone_Number attribute
        /// <summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Property for Fax attribute
        /// <summary>
        public string Fax { get; set; }

        /// <summary>
        /// Property for Email attribute
        /// <summary>
        public string Email { get; set; }

        /// <summary>
        /// Property for Website attribute
        /// <summary>
        public string Website { get; set; }

        /// <summary>
        /// Property for Start_Time attribute
        /// <summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Property for End_Time attribute
        /// <summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Property for Coordinate attribute
        /// <summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// Property for Created_Person attribute
        /// <summary>
        public int CreatedPerson { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
