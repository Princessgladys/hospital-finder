using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using HospitalF.Constant;

namespace HospitalF.Models
{
    
    #region Roles list
    public enum RList
    {
        Administrator = 1,
        HospitalUser = 2,
        User = 3,
    }
    #endregion

    public class UsersModel
    {
        #region Properties

        /// <summary>
        /// Property for Email attribute
        /// <summary>
        [Display(Name = Constants.Email)]
        [RegularExpression(Constants.EmailRegex, ErrorMessage = ErrorMessage.CEM005)]
        public string Email { get; set; }

        /// <summary>
        /// Property for Password attribute
        /// <summary>
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Property for ConfirmPassword attribute
        /// <summary>
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Property for Secondary_Email attribute
        /// <summary>
        [Display(Name = "Secondary Email")]
        public string SecondaryEmail { get; set; }

        /// <summary>
        /// Property for First_Name attribute
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Property for Last_Name attribute
        /// <summary>
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Property for Phone_Number attribute
        /// <summary>
        [Display(Name = Constants.PhoneNo)]
        [Required(ErrorMessage = ErrorMessage.CEM001)]
        [RegularExpression(Constants.CellPhoneNoRegex, ErrorMessage = ErrorMessage.CEM005)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Property for Role_ID attribute
        /// <summary>
        public UsersModel()
        {
            RolesList = new List<SelectListItem>();
        }
        [Display(Name = "Role")]
        public int RoleID { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
        #endregion
    }
}