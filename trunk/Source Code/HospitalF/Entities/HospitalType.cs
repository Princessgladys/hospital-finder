using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for HospitalType table
    /// <summary>
    public class HospitalTypeEntity
    {
        #region HospitalType Properties

        /// <summary>
        /// Property for Type_ID attribute
        /// <summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Property for Type_Name attribute
        /// <summary>
        public string TypeName { get; set; }

        #endregion
    }
}
