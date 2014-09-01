using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalF.Models;

namespace HospitalF.Entities
{
    public class DoctorEntity
    {
        #region Doctor Properties
        /// <summary>
        /// Property for Doctor_ID attribute
        /// </summary>
        public int Doctor_ID { get; set; }

        /// <summary>
        /// Property for Fist_Name attribute
        /// </summary>
        public string First_Name { get; set; }

        /// <summary>
        /// Property for Last_Name attribute
        /// </summary>
        public string Last_Name { get; set; }

        /// <summary>
        /// Property for Gender attribute
        /// </summary>
        public bool? Gender { get; set; }

        /// <summary>
        /// Property for Photo attribute
        /// </summary>
        public Photo Photo { get; set; }

        /// <summary>
        /// Property for Specialities attribute
        /// </summary>
        public List<Speciality> Specialities { get; set; }

        /// <summary>
        /// Property for Degree attribute
        /// </summary>
        public string Degree { get; set; }

        /// <summary>
        /// Property for Experience attribute
        /// </summary>
        public string Experience { get; set; }

        /// <summary>
        /// Property for Working_Day attribute
        /// </summary>
        public string Working_Day { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// </summary>
        public bool? Is_Active { get; set; }
        #endregion
    }
}