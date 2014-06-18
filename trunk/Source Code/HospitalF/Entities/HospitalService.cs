using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Hospital_Service table
    /// <summary>
    public class HospitalServiceEntity
    {
        #region Hospital_Service Properties

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int HospitalID { get; set; }

        /// <summary>
        /// Property for Service_ID attribute
        /// <summary>
        public int ServiceID { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
