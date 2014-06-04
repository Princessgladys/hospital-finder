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
        public int HospitalID;

        /// <summary>
        /// Property for Hospital_Name attribute
        /// <summary>
        public string HospitalName;

        /// <summary>
        /// Property for Hospital_Type attribute
        /// <summary>
        public int? HospitalType;

        /// <summary>
        /// Property for Address attribute
        /// <summary>
        public string Address;

        /// <summary>
        /// Property for Ward_ID attribute
        /// <summary>
        public int? WardID;

        /// <summary>
        /// Property for District_ID attribute
        /// <summary>
        public int? DistrictID;

        /// <summary>
        /// Property for City_ID attribute
        /// <summary>
        public int? CityID;

        /// <summary>
        /// Property for Phone_Number attribute
        /// <summary>
        public string PhoneNumber;

        /// <summary>
        /// Property for Fax attribute
        /// <summary>
        public string Fax;

        /// <summary>
        /// Property for Email attribute
        /// <summary>
        public string Email;

        /// <summary>
        /// Property for Website attribute
        /// <summary>
        public string Website;

        /// <summary>
        /// Property for Start_Time attribute
        /// <summary>
        public TimeSpan? StartTime;

        /// <summary>
        /// Property for End_Time attribute
        /// <summary>
        public TimeSpan? EndTime;

        /// <summary>
        /// Property for Coordinate attribute
        /// <summary>
        public string Coordinate;

        /// <summary>
        /// Property for Created_Person attribute
        /// <summary>
        public int? CreatedPerson;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive;

        #endregion
    }
}
