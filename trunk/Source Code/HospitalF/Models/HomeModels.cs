using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using HospitalF.Entities;
using HospitalF.Constant;

namespace HospitalF.Models
{
    public class HomeModels
    {
        /// <summary>
        /// Get/Set value for property SearchValue
        /// </summary>
        [Display(Name = Constants.SearchValue)]
        [StringLength(100, ErrorMessage = ErrorMessage.CEM003)]
        public string SearchValue { get; set; }

        /// <summary>
        /// Get/Set value for property CityID
        /// </summary>
        [Display(Name = Constants.City)]
        [Required(ErrorMessage = ErrorMessage.CEM012)]
        public int CityID { get; set; }

        /// <summary>
        /// Get/Set value for property CityID
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Get/Set value for property DistrictID
        /// </summary>
        [Display(Name = Constants.District)]
        [Required(ErrorMessage = ErrorMessage.CEM012)]
        public int DistrictID { get; set; }

        /// <summary>
        /// Get/Set value for property DistrictName
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityID
        /// </summary>
        [Display(Name = Constants.Speciality)]
        public int SpecialityID { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityName
        /// </summary>
        public string SpecialityName { get; set; }

        /// <summary>
        /// Get/Set value for property DiseaseID
        /// </summary>
        [Display(Name = Constants.Disease)]
        public int DiseaseID { get; set; }

        /// <summary>
        /// Get/Set value for property DiseaseName
        /// </summary>
        [Display(Name = Constants.Disease)]
        [StringLength(64, ErrorMessage = ErrorMessage.CEM003)]
        public string DiseaseName { get; set; }

        /// <summary>
        /// Get/Set value for property CurrentLocation
        /// </summary>
        [Display(Name = Constants.CurrentLocation)]
        public string CurrentLocation { get; set; }

        /// <summary>
        /// Get/Set value for property AppointedLocation
        /// </summary>
        [Display(Name = Constants.AppointedAddress)]
        [StringLength(128, ErrorMessage = ErrorMessage.CEM003)]
        public string AppointedAddress { get; set; }
    }
}