using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HospitalF.Models
{
    public class FeedBackModels
    {
        #region properties

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

        #region load feedback type
        public static List<FeedbackType> LoadFeedbackType()
        {
            List<FeedbackType> list = new List<FeedbackType>();
            FeedbackType fbt = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                var result = (from fb in data.FeedbackTypes where fb.Is_Active == true select fb).ToList();
                foreach (FeedbackType fb in result)
                {
                    fbt = new FeedbackType();
                    fbt.Type_ID = fb.Type_ID;
                    fbt.Type_Name = fb.Type_Name;
                    list.Add(fbt);
                }
                return list;
            }
        }
        #endregion
        
        #region insert feedback
        public async Task<int> InsertFeedbackAsync(FeedBackModels model)
        {
            int result = 0;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                if (model.FeedbackType != 3 && model.FeedbackType != 4)
                {
                    model.HospitalID = 0;
                }
                result = await Task.Run(() => data.SP_INSERT_FEEDBACK(model.Header,
                    model.FeedbackContent, model.FeedbackType, model.Email, model.HospitalID));
            }
            return result;
        }
        #endregion
    }
}