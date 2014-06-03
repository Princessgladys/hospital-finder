using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for User table
    /// <summary>
    public class User
    {
        #region User Properties

        /// <summary>
        /// Property for User_ID attribute
        /// <summary>
        public int UserID;

        /// <summary>
        /// Property for Email attribute
        /// <summary>
        public string Email;

        /// <summary>
        /// Property for Password attribute
        /// <summary>
        public string Password;

        /// <summary>
        /// Property for First_Name attribute
        /// <summary>
        public string FirstName;

        /// <summary>
        /// Property for Last_Name attribute
        /// <summary>
        public string LastName;

        /// <summary>
        /// Property for Phone_Number attribute
        /// <summary>
        public string PhoneNumber;

        /// <summary>
        /// Property for Role_ID attribute
        /// <summary>
        public int? RoleID;

        /// <summary>
        /// Property for Created_Person attribute
        /// <summary>
        public int? CreatedPerson;

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int? HospitalID;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive;

        #endregion
    }
}
