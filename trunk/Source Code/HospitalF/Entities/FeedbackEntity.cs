using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalF.Entities
{
    public class FeedbackEntity
    {
        #region Feedback property
        /// <summary>
        /// Property for Feedback_ID attribute
        /// </summary>
        public int Feedback_ID { get; set; }

        /// <summary>
        /// Property for Header attribute
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Property for Feedback_Content attribute
        /// </summary>
        public string Feedback_Content { get; set; }

        /// <summary>
        /// Property for Feedback_Type attribute
        /// </summary>
        public string Feedback_Type { get; set; }

        /// <summary>
        /// Property for Email attribute
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Property for Hospital_ID attribute
        /// </summary>
        public int? Hospital_ID { get; set; }

        /// <summary>
        /// Property for Created_Date attribute
        /// </summary>
        public DateTime? Created_Date { get; set; }

        /// <summary>
        /// Property for Is_Response attribute
        /// </summary>
        public bool? Is_Response { get; set; }
        #endregion
    }
}