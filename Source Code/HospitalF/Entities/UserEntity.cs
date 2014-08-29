using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalF.Entities
{
    public class UserEntity
    {
        #region User property
        /// <summary>
        /// Property for User_ID attribute
        /// </summary>
        public int User_ID { get; set; }

        /// <summary>
        /// Property for User_ID attribute
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Property for User_Role_Name attribute
        /// </summary>
        public string User_Role_Name { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// </summary>
        public bool? Is_Active { get; set; }
        #endregion
    }
}