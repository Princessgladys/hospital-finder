using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using HospitalF.Entities;

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

        public static List<FeedbackType> LoadFeedbeackTypeList()
        {
            List<FeedbackType> feedbackTypeList = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                feedbackTypeList = (from ft in data.FeedbackTypes
                                    where ft.Is_Active == true
                                    select ft).ToList<FeedbackType>();
            }
            return feedbackTypeList;
        }

        public static List<FeedbackEntity> LoadAdministratorFeedback(DateTime fromDate, DateTime toDate, int feedbackType, int responseType)
        {
            List<FeedbackEntity> feedbackList = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                feedbackList = (from f in data.Feedbacks
                                from ft in data.FeedbackTypes
                                where f.Feedback_Type == ft.Type_ID && ft.Is_Active == true &&
                                      fromDate <= f.Created_Date && f.Created_Date <= toDate &&
                                      (f.Feedback_Type == 1 || f.Feedback_Type == 2 || f.Feedback_Type == 3) && // Only Type_ID == 1 and Type_ID == 2 and Type_ID == 3
                                      (feedbackType == 0 || f.Feedback_Type == feedbackType) &&
                                      (responseType == 0 || f.Is_Response == (responseType == 1 ? true : false))
                                select new FeedbackEntity()
                                {
                                    Feedback_ID = f.Feedback_ID,
                                    Header = f.Header,
                                    Feedback_Content = f.Feedback_Content,
                                    Email = f.Email,
                                    Feedback_Type = ft.Type_Name,
                                    Created_Date = f.Created_Date,
                                    Is_Response = f.Is_Response
                                }).ToList<FeedbackEntity>();
            }
            return feedbackList;
        }

        public static bool IsAssignedHospital(int hospitalId)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                User user = (from u in data.Users
                             where u.Hospital_ID == hospitalId
                             select u).SingleOrDefault();
                if (user != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ApproveFeedback(int feedbackId)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                Feedback feedback = (from f in data.Feedbacks
                                     where f.Feedback_ID == feedbackId
                                     select f).SingleOrDefault();
                if (feedback != null)
                {
                    feedback.Is_Response = true;
                    data.SubmitChanges();
                    return true;
                }
            }
            return false;
        }
    }
}