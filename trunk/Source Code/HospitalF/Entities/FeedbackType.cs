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
        public int TypeID;

        /// <summary>
        /// Property for Type_Name attribute
        /// <summary>
        public string TypeName;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive;

        #endregion
    }
}
