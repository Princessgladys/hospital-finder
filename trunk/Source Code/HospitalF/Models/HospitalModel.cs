using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;
using HospitalF.Constant;
using HospitalF.Utilities;

namespace HospitalF.Models
{
    /// <summary>
    /// Class define bussiness method for HospitalModel
    /// </summary>
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
        public string HospitalEmail { get; set; }

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
        public int CreatedPerson { get; set; }

        /// <summary>
        /// Get/Set value for property PersonInCharged
        /// </summary>
        public string PersonInCharged { get; set; }

        /// <summary>
        /// Get/Set value for property SelectedPersonInCharged
        /// </summary>
        public List<string> SelectedPersonInCharged { get; set; }

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

        /// <summary>
        /// Get/Set value for property PhotoFilesPath
        /// </summary>
        public string PhotoFilesPath { get; set; }

        /// <summary>
        /// Get/Set value for property PhotoFilesPath
        /// </summary>
        public string TagsInput { get; set; }

        /// <summary>
        /// Get/Set value for property PhotoFilesPath
        /// </summary>
        public int RecordStatus { get; set; }

        /// <summary>
        /// Get/Set value for property AverageCuringTime
        /// </summary>
        public int? AverageCuringTime { get; set; }

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
        /// Check if an user have been existed in database
        /// </summary>
        /// <param name="email">Input email</param>
        /// <returns>
        /// Task[ActionResult] with JSON that contains the value of:
        /// 1: Valid
        /// 0: Invalid
        /// 2: Invalid with already manage a hospital
        /// </returns>
        public async Task<int> CheckValidUserWithEmail(string email)
        {
            int result = 0;
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_CHECK_VALID_USER_IN_CHARGED(email));
            }
            return result;
        }

        /// <summary>
        /// Check if there is  similar hospital with name and address
        /// are equal with given data from user
        /// </summary>
        /// <param name="locationAddress">Location address</param>
        /// <param name="cityId">City ID</param>
        /// <param name="districtId">District ID</param>
        /// <param name="wardId">Ward ID</param>
        /// <returns> 1: Not duplicated, 0: Duplicated</returns>
        public async Task<int> CheckValidHospitalWithAddress(string address,
            int cityId, int districtId, int wardId)
        {
            int result = 0;
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_CHECK_NOT_DUPLICATED_HOSPITAL(
                    cityId, districtId, wardId, address));
            }
            return result;
        }

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
        /// <returns></returns>
        public async Task<int> InsertHospitalAsync(HospitalModel model)
        {
            int result = 0;

            #region Prepare data

            // Full address
            model.FullAddress = string.Format("{0} {1}, {2}, {3}, {4}",
                model.LocationAddress, model.StreetAddress, model.WardName,
                model.DistrictName, model.CityName);

            // Phone number
            string phoneNumber = model.PhoneNo;
            if (!string.IsNullOrEmpty(model.PhoneNo2))
            {
                phoneNumber += Constants.Slash + model.PhoneNo2;
            }
            if (!string.IsNullOrEmpty(model.PhoneNo3))
            {
                phoneNumber += Constants.Slash + model.PhoneNo3;
            }
            model.PhoneNo = phoneNumber;

            // Holiday time
            string[] holidayTime = model.HolidayStartTime.Split(char.Parse(Constants.Minus));
            string holidayStartTime = holidayTime[0].Trim();
            model.HolidayStartTime = holidayStartTime;
            string holidayEndTime = holidayTime[1].Trim();
            model.HolidayEndTime = holidayEndTime;

            // Ordinary time
            string[] OrdinaryTime = model.OrdinaryStartTime.Split(char.Parse(Constants.Minus));
            string ordinaryStartTime = OrdinaryTime[0].Trim();
            model.OrdinaryStartTime = ordinaryStartTime;
            string ordinaryEndTime = OrdinaryTime[1].Trim();
            model.OrdinaryEndTime = ordinaryEndTime;

            // Speciality list
            string speciality = string.Empty;
            if ((model.SelectedSpecialities != null) && (model.SelectedSpecialities.Count != 0))
            {
                for (int n = 0; n < model.SelectedSpecialities.Count; n++)
                {
                    if (n == (model.SelectedSpecialities.Count - 1))
                    {
                        speciality += model.SelectedSpecialities[n];
                    }
                    else
                    {
                        speciality += model.SelectedSpecialities[n] +
                            Constants.VerticalBar.ToString();
                    }
                }
            }

            // Service list
            string service = string.Empty;
            if ((model.SelectedServices != null) && (model.SelectedServices.Count != 0))
            {
                for (int n = 0; n < model.SelectedServices.Count; n++)
                {
                    if (n == (model.SelectedServices.Count - 1))
                    {
                        service += model.SelectedServices[n];
                    }
                    else
                    {
                        service += model.SelectedServices[n] +
                            Constants.VerticalBar.ToString();
                    }
                }
            }

            // Facility list
            string facility = string.Empty;
            if ((model.SelectedFacilities != null) && (model.SelectedFacilities.Count != 0))
            {
                for (int n = 0; n < model.SelectedFacilities.Count; n++)
                {
                    if (n == (model.SelectedFacilities.Count - 1))
                    {
                        facility += model.SelectedFacilities[n];
                    }
                    else
                    {
                        facility += model.SelectedFacilities[n] +
                            Constants.VerticalBar.ToString();
                    }
                }
            }

            // Person in charged
            if (model.PersonInCharged == null)
            {
                model.PersonInCharged = string.Empty;
            }

            #endregion

            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_INSERT_HOSPITAL(model.HospitalName,
                    model.HospitalTypeID, model.FullAddress, model.CityID, model.DistrictID,
                    model.WardID, model.PhoneNo, model.Fax, model.HospitalEmail, model.Website,
                    model.HolidayStartTime, model.HolidayEndTime, model.OrdinaryStartTime,
                    model.OrdinaryEndTime, model.Coordinate, model.IsAllowAppointment,
                    model.CreatedPerson, model.FullDescription, model.PersonInCharged,
                    model.PhotoFilesPath, model.TagsInput, model.AverageCuringTime, speciality,
                    service, facility));
            }
            return result;
        }

        /// <summary>
        /// Load specific hospital in database
        /// </summary>
        /// <param name="hospitalId">Hospital ID</param>
        /// <returns></returns>
        public async Task<HospitalModel> LoadSpecificHospital(int hospitalId)
        {
            // Create new Hospital model to store data that returned from database
            HospitalModel model = new HospitalModel();

            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                
                #region Load single hospital data
                
                model = await Task.Run(() =>
                    (from h in data.SP_LOAD_SPECIFIC_HOSPITAL(hospitalId)
                     select new HospitalModel()
                     {
                         HospitalID = h.Hospital_ID,
                         HospitalName = h.Hospital_Name,
                         HospitalTypeID = (h.Hospital_Type != null) ? h.Hospital_Type.Value : 0,
                         FullAddress = h.Address,
                         WardID = (h.Ward_ID != null) ? h.Ward_ID.Value : 0,
                         DistrictID = (h.District_ID != null) ? h.District_ID.Value : 0,
                         CityID = (h.City_ID != null) ? h.City_ID.Value : 0,
                         PhoneNo = h.Phone_Number,
                         Fax = h.Fax,
                         HospitalEmail = h.Email,
                         Website = h.Website,
                         OrdinaryStartTime = h.Ordinary_Start_Time.Value.Hours.ToString(Constants.Format2Digit) +
                            h.Ordinary_Start_Time.Value.Minutes.ToString(Constants.Format2Digit) +
                            h.OrDinary_End_Time.Value.Hours.ToString(Constants.Format2Digit) +
                            h.OrDinary_End_Time.Value.Minutes.ToString(Constants.Format2Digit),
                         HolidayStartTime = h.Holiday_Start_Time.Value.Hours.ToString(Constants.Format2Digit) +
                            h.Holiday_Start_Time.Value.Minutes.ToString(Constants.Format2Digit) +
                            h.Holiday_End_Time.Value.Hours.ToString(Constants.Format2Digit) +
                            h.Holiday_End_Time.Value.Minutes.ToString(Constants.Format2Digit),
                         Coordinate = h.Coordinate,
                         ShortDescription = h.Short_Description,
                         FullDescription = h.Full_Description,
                         IsAllowAppointment = (h.Is_Allow_Appointment != null) ? h.Is_Allow_Appointment.Value : false,
                         IsActive = (h.Is_Active != null) ? h.Is_Active.Value : false,
                         CreatedPerson = (h.Created_Person != null) ? h.Created_Person.Value : 0,
                         CityName = h.City_Name,
                         DistrictName = h.District_Name,
                         WardName = h.Ward_Name
                     }).SingleOrDefault());

                #endregion
                
                #region Load list of persons in charged

                model.SelectedPersonInCharged = await Task.Run(() =>
                    (from u in data.Users
                     where u.Hospital_ID.Equals(hospitalId)
                     select u.User_ID.ToString()).ToList());

                #endregion

                #region Load list of specialities

                model.SelectedSpecialities = await Task.Run(() =>
                    (from hs in data.Hospital_Specialities
                     where hs.Hospital_ID.Equals(hospitalId)
                     select hs.Speciality_ID.ToString()).ToList());

                #endregion

                #region Load list of services

                model.SelectedServices = await Task.Run(() =>
                    (from hs in data.Hospital_Services
                     where hs.Hospital_ID.Equals(hospitalId)
                     select hs.Service_ID.ToString()).ToList());

                #endregion

                #region Load list of facilities

                model.SelectedFacilities = await Task.Run(() =>
                    (from hf in data.Hospital_Facilities
                     where hf.Hospital_ID.Equals(hospitalId)
                     select hf.Facility_ID.ToString()).ToList());

                #endregion

                #region Arrange data

                // Address
                string[] addressList = model.FullAddress.Split(Char.Parse(Constants.Comma));
                model.WardName = addressList[1];
                model.DistrictName = addressList[2];
                model.CityName = addressList[3];
                string[] detailAddress = addressList[0].Split(Char.Parse(Constants.WhiteSpace));
                model.LocationAddress = detailAddress[0];
                for (int n = 1; n < detailAddress.Count(); n++)
                {
                    model.StreetAddress += detailAddress[n] + Constants.WhiteSpace;
                }
                model.StreetAddress = model.StreetAddress.Trim();

                // Phone number
                string[] phoneNumberList = model.PhoneNo.Split(Char.Parse(Constants.Slash));
                int phoneNumberQuantity = phoneNumberList.Count();
                if (phoneNumberQuantity == 2)
                {
                    model.PhoneNo = phoneNumberList[0];
                    if (phoneNumberList[1].Contains(Constants.OpenBracket) ||
                        phoneNumberList[1].Contains(Constants.CloseBracket) ||
                        (phoneNumberList[1].Length <= 8))
                    {
                        model.PhoneNo2 = phoneNumberList[1];
                    }
                    else
                    {
                        model.PhoneNo3 = phoneNumberList[1];
                    }
                    
                }
                if (phoneNumberQuantity == 3)
                {
                    model.PhoneNo = phoneNumberList[0];
                    model.PhoneNo2 = phoneNumberList[1];
                    model.PhoneNo3 = phoneNumberList[2];
                }

                #endregion
            }

            // Return HospitalModel
            return model;
                 
        }
                
        /// <summary>
        /// Update specific hospital
        /// </summary>
        /// <param name="model">Hospital model</param>
        /// <returns></returns>
        public async Task<int> UpdateHospitalAsync(HospitalModel model)
        {
            int result = 0;

            #region Prepare data

            // Full address
            model.FullAddress = string.Format("{0} {1}, {2}, {3}, {4}",
                model.LocationAddress, model.StreetAddress, model.WardName,
                model.DistrictName, model.CityName);

            // Phone number
            string phoneNumber = model.PhoneNo;
            if (!string.IsNullOrEmpty(model.PhoneNo2))
            {
                phoneNumber += Constants.Slash + model.PhoneNo2;
            }
            if (!string.IsNullOrEmpty(model.PhoneNo3))
            {
                phoneNumber += Constants.Slash + model.PhoneNo3;
            }
            model.PhoneNo = phoneNumber;

            // Holiday time
            string[] holidayTime = model.HolidayStartTime.Split(char.Parse(Constants.Minus));
            string holidayStartTime = holidayTime[0].Trim();
            model.HolidayStartTime = holidayStartTime;
            string holidayEndTime = holidayTime[1].Trim();
            model.HolidayEndTime = holidayEndTime;

            // Ordinary time
            string[] OrdinaryTime = model.OrdinaryStartTime.Split(char.Parse(Constants.Minus));
            string ordinaryStartTime = OrdinaryTime[0].Trim();
            model.OrdinaryStartTime = ordinaryStartTime;
            string ordinaryEndTime = OrdinaryTime[1].Trim();
            model.OrdinaryEndTime = ordinaryEndTime;

            // Speciality list
            string speciality = string.Empty;
            if ((model.SelectedSpecialities != null) && (model.SelectedSpecialities.Count != 0))
            {
                for (int n = 0; n < model.SelectedSpecialities.Count; n++)
                {
                    if (n == (model.SelectedSpecialities.Count - 1))
                    {
                        speciality += model.SelectedSpecialities[n];
                    }
                    else
                    {
                        speciality += model.SelectedSpecialities[n] +
                            Constants.VerticalBar.ToString();
                    }
                }
            }

            // Service list
            string service = string.Empty;
            if ((model.SelectedServices != null) && (model.SelectedServices.Count != 0))
            {
                for (int n = 0; n < model.SelectedServices.Count; n++)
                {
                    if (n == (model.SelectedServices.Count - 1))
                    {
                        service += model.SelectedServices[n];
                    }
                    else
                    {
                        service += model.SelectedServices[n] +
                            Constants.VerticalBar.ToString();
                    }
                }
            }

            // Facility list
            string facility = string.Empty;
            if ((model.SelectedFacilities != null) && (model.SelectedFacilities.Count != 0))
            {
                for (int n = 0; n < model.SelectedFacilities.Count; n++)
                {
                    if (n == (model.SelectedFacilities.Count - 1))
                    {
                        facility += model.SelectedFacilities[n];
                    }
                    else
                    {
                        facility += model.SelectedFacilities[n] +
                            Constants.VerticalBar.ToString();
                    }
                }
            }

            #endregion

            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_UPDATE_HOSPITAL(model.HospitalID, model.HospitalName,
                    model.HospitalTypeID, model.FullAddress, model.CityID, model.DistrictID,
                    model.WardID, model.PhoneNo, model.Fax, model.HospitalEmail, model.Website,
                    model.HolidayStartTime, model.HolidayEndTime, model.OrdinaryStartTime,
                    model.OrdinaryEndTime, model.Coordinate, model.IsAllowAppointment, model.CreatedPerson,
                    model.FullDescription, speciality, service, facility));
            }
            return result;
        }

        /// <summary>
        /// Handle Excel file
        /// </summary>
        /// <param name="file">Input file</param>
        /// <param name="userId">User ID</param>
        /// <returns>List[HospitalModel] that contains list of Hospitals</returns>
        public async Task<List<HospitalModel>> HandleExcelFileData(HttpPostedFileBase file, int userId)
        {
            // Create new list of hospitals
            List<HospitalModel> hospitalList = new List<HospitalModel>();

            // Load data from Excel
            hospitalList = ExcelUtil.LoadDataFromExcel(file, userId);

            // Return hospital list
            return hospitalList;
        }

        #endregion
    }
}