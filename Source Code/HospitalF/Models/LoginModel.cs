using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalF.Constant;

namespace HospitalF.Models
{
    /// <summary>
    /// Class define properties for /Account/Login View
    /// </summary>
    public class LoginModel
    {
        [Required]
        [Display(Name = "Your Email")]
        //[RegularExpression(Constants.EmailRegex, ErrorMessage = ErrorMessage.CEM005)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}