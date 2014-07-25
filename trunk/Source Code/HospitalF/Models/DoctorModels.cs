using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalF.Models
{
    public class DoctorModels
    {
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Fullname { get; set; }
        public int Gender { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string WorkingDay { get; set; }
        public int PhotoID { get; set; }
        public string PhotoFilePath { get; set; }
        public int SpecialityID { get; set; }
        public string SpecialityName { get; set; }
        public List<Speciality> SpecialityList { get; set; }

        /// <summary>
        /// Get/Set value for property SelectedSpecialities
        /// </summary>
        public List<string> SelectedSpecialities { get; set; }
    }
}