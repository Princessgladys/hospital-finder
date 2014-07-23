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
        /// Get/Set value for property LocationAddress
        /// </summary>
        public string LocationAddress { get; set; }

        /// <summary>
        /// Get/Set value for property StreetAddress
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Get/Set value for property WardName
        /// </summary>
        public string FullAddress { get; set; }

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
        /// Get/Set value for property Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get/Set value for property PhoneNo
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// Get/Set value for property PhoneNo2
        /// </summary>
        public string PhoneNo2 { get; set; }

        /// <summary>
        /// Get/Set value for property PhoneNo3
        /// </summary>
        public string PhoneNo3 { get; set; }

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

        /// <summary>
        /// Get/Set value for property SelectedSpecialities
        /// </summary>
        public List<string> SelectedSpecialities { get; set; }

        /// <summary>
        /// Get/Set value for property SelectedServices
        /// </summary>
        public List<string> SelectedServices { get; set; }

        /// <summary>
        /// Get/Set value for property SelectedFacilities
        /// </summary>
        public List<string> SelectedFacilities { get; set; }

        public List<Speciality> SpecialityList { get; set; }
        public List<Service> ServiceList { get; set; }
        public List<Facility> FacilityList { get; set; }
        public string SpecialityName { get; set; }
        public int SpecialityID { get; set; }
        public string DoctorName { get; set; }
        public List<Doctor> DoctorList { get; set; }
        
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

        /// <summary>
        /// Change status of a specific hospital
        /// </summary>
        /// <param name="hospitalId">Hospital ID</param>
        /// <returns>1: Successful. 0: Failed</returns>
        public async Task<int> ChangeHospitalStatusAsync(int hospitalId)
        {
            int result = 0;

            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_CHANGE_HOSPITAL_STATUS(hospitalId));
            }

            return result;
        }

        /// <summary>
        /// Insert new hospital
        /// </summary>
        /// <param name="model">Hospital model</param>
        /// <param name="speciality">Speciality list</param>
        /// <param name="service">Service list</param>
        /// <param name="facility">Facility list</param>
        /// <returns></returns>
        public async Task<int> InsertHospitalAsync(HospitalModel model, string speciality, string service, string facility)
        {
            int result = 0;
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_INSERT_HOSPITAL(model.HospitalName, model.HospitalTypeID, model.FullAddress,
                    model.CityID, model.DistrictID, model.WardID, model.PhoneNo, model.Fax, model.Email,
                    model.Website, model.HolidayStartTime, model.HolidayEndTime, model.OrdinaryStartTime, model.OrdinaryEndTime,
                    model.Coordinate, model.IsAllowAppointment, 1, null, speciality, service, facility));
            }
            return result;
        }

        #endregion
    }
}