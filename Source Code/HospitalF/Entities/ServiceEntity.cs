using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalF.Entities
{
    public class ServiceEntity
    {
        #region Service property
        /// <summary>
        /// Property for Service_ID attribute
        /// </summary>
        public int Service_ID { get; set; }

        /// <summary>
        /// Property for Service_Name attribute
        /// </summary>
        public string Service_Name { get; set; }
        /// <summary>
        /// Property for Type_ID attribute
        /// </summary>
        public int? Type_ID { get; set; }
        /// <summary>
        /// Property for Type_Name attribute
        /// </summary>
        public string Type_Name { get; set; }
        /// <summary>
        /// Property for Is_Active attribute
        /// </summary>
        public bool? Is_Active { get; set; }
        #endregion
    }
}