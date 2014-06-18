using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for FeedbackType table
    /// <summary>
    public class FeedbackTypeEntity
    {
        #region FeedbackType Properties

        /// <summary>
        /// Property for Type_ID attribute
        /// <summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Property for Type_Name attribute
        /// <summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
