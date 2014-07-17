using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HospitalF.Models
{
    public class HospitalModel
    {
        #region AnhDTH

        #region Hospital Properties

        /// <summary>
        /// Get/Set value for property HospitalID
        /// </summary>
        public int HospitalID { get; set; }

        /// <summary>
        /// Get/Set value for property HospitalID
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// Get/Set value for property HospitalTypeName
        /// </summary>
        public string HospitalTypeName { get; set; }

        /// <summary>
        /// Get/Set value for property HospitalTypeID
        /// </summary>
        public int HospitalTypeID { get; set; }

        /// <summary>
        /// Get/Set value for property WardName
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Get/Set value for property WardName
        /// </summary>
        public string WardName { get; set; }

        /// <summary>
        /// Get/Set value for property WardID
        /// </summary>
        public int WardID { get; set; }

        /// <summary>
        /// Get/Set value for property DistrictName
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// Get/Set value for property DistrictID
        /// </summary>
        public int DistrictID { get; set; }

        /// <summary>
        /// Get/Set value for property CityName
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Get/Set value for property CityID
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// Get/Set value for property PhoneNo
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// Get/Set value for property Fax
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Get/Set value for property Website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Get/Set value for property OrdinaryStartTime
        /// </summary>
        public string OrdinaryStartTime { get; set; }

        /// <summary>
        /// Get/Set value for property OrdinaryEndTime
        /// </summary>
        public string OrdinaryEndTime { get; set; }

        /// <summary>
        /// Get/Set value for property HolidayStartTime
        /// </summary>
        public string HolidayStartTime { get; set; }

        /// <summary>
        /// Get/Set value for property HolidayEndTime
        /// </summary>
        public string HolidayEndTime { get; set; }

        /// <summary>
        /// Get/Set value for property Coordinate
        /// </summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// Get/Set value for property ShortDescription
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Get/Set value for property FullDescription
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// Get/Set value for property FullDescription
        /// </summary>
        public bool IsAllowAppointment { get; set; }

        /// <summary>
        /// Get/Set value for property CreatedPerson
        /// </summary>
        public string CreatedPerson { get; set; }

        /// <summary>
        /// Get/Set value for property CreatedPerson
        /// </summary>
        public Boolean IsActive { get; set; }

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

        #endregion

        #region SonNX

        /// <summary>
        /// [Aministrator] Load list of hospital base on input values
        /// </summary>
        /// <param name="hospitalName">Hospital name</param>
        /// <param name="cityId">City ID</param>
        /// <param name="districtId">District ID</param>
        /// <param name="hospitalType">Hospital type ID</param>
        /// <param name="isActive">Status</param>
        /// <returns>List[SP_LOAD_HOSPITAL_LISTResult] that contains list of suitable hospitals</returns>
        public async Task<List<SP_LOAD_HOSPITAL_LISTResult>> LoadListOfHospital(
            string hospitalName, int cityId, int districtId, int hospitalType, bool isActive)
        {
            // Declare new list
            List<SP_LOAD_HOSPITAL_LISTResult> hospitalList = 
                new List<SP_LOAD_HOSPITAL_LISTResult>();

            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                hospitalList = await Task.Run(() =>
                    data.SP_LOAD_HOSPITAL_LIST(HospitalName, cityId, districtId,
                    hospitalType, isActive).ToList());
            }

            // Return list of hospitals
            return hospitalList;
        }

        #endregion
    }
}