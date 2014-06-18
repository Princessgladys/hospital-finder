using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Feedback table
    /// <summary>
    public class FeedbackEntity
    {
        #region Feedback Properties

        /// <summary>
        /// Property for Feedback_ID attribute
        /// <summary>
        public int FeedbackID { get; set; }

        /// <summary>
        /// Property for Header attribute
        /// <summary>
        public string Header { get; set; }

        /// <summary>
        /// Property for Feedback_Content attribute
        /// <summary>
        public string FeedbackContent { get; set; }

        /// <summary>
        /// Property for Feedback_Type attribute
        /// <summary>
        public int FeedbackType { get; set; }

        /// <summary>
        /// Property for Email attribute
        /// <summary>
        public string Email { get; set; }

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int HospitalID { get; set; }

        /// <summary>
        /// Property for Created_Date attribute
        /// <summary>
        public DateTime CreatedDate { get; set; }

        #endregion
    }
}
