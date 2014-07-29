using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Constant;

namespace HospitalF.Models
{
    /// <summary>
    /// Class define bussiness method for DataModel
    /// </summary>
    public class DataModel
    {
        #region Properties

        /// <summary>
        /// Get/Set value for property HospitalID
        /// </summary>
        public int ServiceID { get; set; }

        /// <summary>
        /// Get/Set value for property ServiceName
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Get/Set value for property FacilityID
        /// </summary>
        public int FacilityID { get; set; }

        /// <summary>
        /// Get/Set value for property FacilityName
        /// </summary>
        public string FacilityName { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityID
        /// </summary>
        public int SpecialityID { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityName
        /// </summary>
        public string SpecialityName { get; set; }

        /// <summary>
        /// Get/Set value for property IsMainSpeciality
        /// </summary>
        public bool IsMainSpeciality { get; set; }

        /// <summary>
        /// Get/Set value for property TypeID
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Get/Set value for property TypeName
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Get/Set value for property IsActive
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

        #region Method

        

        #endregion
    }
}