using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HospitalF.Models
{
    public class HospitalModel
    {
        #region Hospital Properties
        public int HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string TypeName { get; set; }
        public string PhoneNo { get; set; }
        public string Fax { get; set; }
        public string SpecialityName { get; set; }
        public int SpecialityID { get; set; }
        public string DoctorName { get; set; }
        public List<Doctor> DoctorList { get; set; }
        public List<Speciality> SpecialityList { get; set; }
        public List<Service> ServiceList { get; set; }
        public List<Facility> FacilityList { get; set; }
        #endregion

        #region Doctor Properties
        public string Degree { get; set; }
        #endregion

        #region Load hospitalType in List hospitalType
        public string LoadHospitalTypeInList(int hospitalTypeID, List<HospitalType> typeList)
        {
            string result = "";
            foreach (HospitalType htype in typeList)
            {
                if (htype.Type_ID == hospitalTypeID)
                {
                    result = htype.Type_Name;
                }
            }
            return result;
        }
        #endregion

        #region load photo
        public static Photo LoadPhotoByPhotoID(int photoID)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return (
                    from p in data.Photos
                    where p.Photo_ID == photoID
                    select p).FirstOrDefault();
            }
        }
        #endregion
    }
}