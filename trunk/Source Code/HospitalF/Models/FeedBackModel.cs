using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace HospitalF.Models
{
    public class FeedBackModel
    {
        #region Properties

        /// <summary>
        /// Get/set value for property FeedbackID
        /// </summary>
        public int FeedbackID { get; set; }

        /// <summary>
        /// Get/set value for property Header
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Get/set value for property FeedbackType
        /// </summary>
        public int FeedbackType { get; set; }

        /// <summary>
        /// Get/set value for property FeedbackTypeName
        /// </summary>
        public string FeedbackTypeName { get; set; }

        /// <summary>
        /// Get/set value for property FeedbackContent
        /// </summary>
        public string FeedbackContent { get; set; }

        /// <summary>
        /// Get/set value for property Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Get/set value for property HospitalID
        /// </summary>
        public int HospitalID { get; set; }

        /// <summary>
        /// Get/set value for property HospitalName
        /// </summary>
        public string HospitalName { get; set; }

        #endregion

        public bool InsertNewFeedback()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    Feedback newFeedback = new Feedback()
                    {
                       Header = this.Header,
                       Feedback_Content = this.FeedbackContent,
                       Feedback_Type = this.FeedbackType,
                       Email = this.Email,
                       Hospital_ID = this.HospitalID,
                       Created_Date = DateTime.Now,
                       Is_Response = false
                    };
                    data.Feedbacks.InsertOnSubmit(newFeedback);
                    data.SubmitChanges();
                    ts.Complete();
                    return true;
                }
            }
        }

        public List<FeedbackType> LoadFeedbeackTypeList()
        {
            List<FeedbackType> feedbackTypeList = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                feedbackTypeList = (from ft in data.FeedbackTypes
                                    select ft).ToList<FeedbackType>();
            }
            return feedbackTypeList;
        }
    }
}