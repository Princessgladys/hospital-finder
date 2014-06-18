using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for User table
    /// <summary>
    public class UserEntity
    {
        #region User Properties

        /// <summary>
        /// Property for User_ID attribute
        /// <summary>
        public int UserID { get; set; }

        /// <summary>
        /// Property for Email attribute
        /// <summary>
        public string Email { get; set; }

        /// <summary>
        /// Property for Password attribute
        /// <summary>
        public string Password { get; set; }

        /// <summary>
        /// Property for Secondary_Email attribute
        /// <summary>
        public string SecondaryEmail { get; set; }

        /// <summary>
        /// Property for First_Name attribute
        /// <summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Property for Last_Name attribute
        /// <summary>
        public string LastName { get; set; }

        /// <summary>
        /// Property for Phone_Number attribute
        /// <summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Property for Role_ID attribute
        /// <summary>
        public int RoleID { get; set; }

        /// <summary>
        /// Property for Confirmed_Person attribute
        /// <summary>
        public int? ConfirmedPerson { get; set; }

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int? HospitalID { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
