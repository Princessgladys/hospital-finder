using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HospitalF.App_Start;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.Utilities;
using HospitalF.Entities;
using System.Collections.Specialized;

namespace HospitalF.Controllers
{
    public class HospitalController : SecurityBaseController
    {
        // Declare public list items for Drop down lists
        public static List<City> cityList = null;
        public static List<HospitalType> hospitalTypeList = null;
        public static List<District> districtList = null;
        public static List<Ward> wardList = null;
        public static IEnumerable<GroupedSelectListItem> serviceList = null;
        public static IEnumerable<GroupedSelectListItem> facilityList = null;
        public static List<Doctor> doctorList = null;
        public static List<Speciality> specialityList = null;

        #region AnhDTH

        public static int hospitalID = 68;
        public Hospital hospital = null;
        public static HospitalModel model = null;
        public static DoctorModels DoctorModel = null;

        #region load location
        public async Task<List<District>> LoadDistrict(int cityID)
        {
            List<District> list = await LocationUtil.LoadDistrictInCityAsync(cityID);
            District district = null;
            List<District> result = new List<District>();
            foreach (District d in list)
            {
                district = new District();
                district.District_ID = d.District_ID;
                district.District_Name = d.Type + Constants.WhiteSpace + d.District_Name;
                result.Add(district);
            }
            return result;
        }
        public async Task<List<Ward>> LoadWard(int districtID)
        {
            List<Ward> list = await LocationUtil.LoadWardInDistrictAsync(districtID);
            Ward ward = null;
            List<Ward> result = new List<Ward>();
            foreach (Ward w in list)
            {
                ward = new Ward();
                ward.Ward_ID = w.Ward_ID;
                ward.Ward_Name = w.Type + Constants.WhiteSpace + w.Ward_Name;
                result.Add(ward);
            }
            return result;
        }
        #endregion

        //
        // GET: /Hospital/
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> Index()
        {
            model = new HospitalModel();
            hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalID);
            hospitalTypeList = await HospitalUtil.LoadTypeInHospitalTypeAsync(hospitalID);

            //assign value for model attributes
            model.HospitalID = hospitalID;
            model.HospitalName = hospital.Hospital_Name;
            model.FullAddress = hospital.Address;
            model.Website = hospital.Website;
            model.PhoneNo = hospital.Phone_Number;
            model.Fax = hospital.Fax;
            model.HospitalTypeName = model.LoadHospitalTypeInList((int)hospital.Hospital_Type, hospitalTypeList);
            DateTime start;
            DateTime end;
            if (hospital.Ordinary_Start_Time == null && hospital.Ordinary_End_Time == null)
            {
                //startTime = new TimeSpan(0, 0, 0);
                model.OrdinaryStartTime = null;
            }
            else
            {
                start = DateTime.Today + (TimeSpan)hospital.Ordinary_Start_Time;
                end = DateTime.Today + (TimeSpan)hospital.Ordinary_End_Time;
                model.OrdinaryStartTime = start.ToString("HH:mm") + " - " + end.ToString("HH:mm");
            }
            if (hospital.Holiday_Start_Time == null && hospital.Holiday_End_Time == null)
            {
                //startTime = new TimeSpan(0, 0, 0);
                model.HolidayStartTime = null;
            }
            else
            {
                start = DateTime.Today + (TimeSpan)hospital.Holiday_Start_Time;
                end = DateTime.Today + (TimeSpan)hospital.Holiday_End_Time;
                model.HolidayStartTime = start.ToString("HH:mm") + " - " + end.ToString("HH:mm");
            }
            if (hospital.Is_Allow_Appointment == null)
            {
                model.IsAllowAppointment = false;
            }
            else
            {
                model.IsAllowAppointment = true;
            }

            //load speciality of hospital
            specialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
            ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

            //load facility of hospital
            ViewBag.FacilityList = await ServiceFacilityUtil.LoadFacilityOfHospitalAsync(hospitalID);

            //load service of hospital
            ViewBag.ServiceList = await ServiceFacilityUtil.LoadServiceOfHospitalAsync(hospitalID);

            return View(model);
        }

        #region search doctor
        public async Task<ActionResult> SearchDoctor(string SpecialityID, string DoctorName, string HospitalID)
        {
            try
            {
                int tempSpecialityID, tempHospitalID;
                doctorList = new List<Doctor>();
                if (SpecialityID == "")
                {
                    SpecialityID = "0";
                }
                if (!String.IsNullOrEmpty(SpecialityID) && Int32.TryParse(SpecialityID, out tempSpecialityID)
                    && Int32.TryParse(HospitalID, out tempHospitalID))
                {
                    doctorList = await HospitalUtil.SearchDoctor(DoctorName, tempSpecialityID, tempHospitalID);
                    ViewBag.DoctorList = doctorList;
                }
                return PartialView("SearchResult");
            }
            catch (Exception ex)
            {
                LoggingUtil.LogException(ex);
                return RedirectToAction(Constants.SystemFailureHospitalUserAction, Constants.ErrorController);
            }

        }
        #endregion

        #region update hospital basic information
        /// <summary>
        /// Get/Hospital/HospitalBasicInforUpdate
        /// </summary>
        /// <param name="hospitalId">hospitalID</param>
        /// <returns>Task<ActionResult></returns>
        [HttpGet]
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> HospitalBasicInforUpdate(int hospitalId)
        {
            //int temphospitaID=Int32.Parse(hospitalId);
            model = new HospitalModel();
            hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalId);

            //assign value for model attributes
            model.HospitalID = hospitalId;
            model.HospitalName = hospital.Hospital_Name;
            model.FullAddress = hospital.Address;
            model.Website = hospital.Website;
            model.PhoneNo = hospital.Phone_Number;
            model.Fax = hospital.Fax;

            TimeSpan startTime;
            TimeSpan endTime;
            DateTime start;
            DateTime end;
            //ordinary time
            if (hospital.Ordinary_Start_Time == null && hospital.Ordinary_End_Time == null)
            {
                //startTime = new TimeSpan(0, 0, 0);
                model.OrdinaryStartTime = null;
            }
            else
            {
                startTime = (TimeSpan)hospital.Ordinary_Start_Time;

                endTime = (TimeSpan)hospital.Ordinary_End_Time;

                start = DateTime.Today + startTime;
                end = DateTime.Today + endTime;
                model.OrdinaryStartTime = start.ToString("HH:mm") + Constants.Minus + end.ToString("HH:mm");
            }
            // holiday time
            if (hospital.Holiday_Start_Time == null && hospital.Holiday_End_Time == null)
            {
                model.HolidayStartTime = null;
            }
            else
            {
                startTime = (TimeSpan)hospital.Holiday_Start_Time;
                endTime = (TimeSpan)hospital.Holiday_End_Time;
                start = DateTime.Today + startTime;
                end = DateTime.Today + endTime;
                model.HolidayStartTime = start.ToString("HH:mm") + Constants.Minus + end.ToString("HH:mm");
            }
            //is allow appointment
            if (hospital.Is_Allow_Appointment == null)
            {
                model.IsAllowAppointment = false;
            }
            else
            {
                model.IsAllowAppointment = true;
            }

            // Load list of cities
            cityList = await LocationUtil.LoadCityAsync();
            model.CityID = (int)hospital.City_ID;
            SelectList cList = new SelectList(cityList, Constants.CityID, Constants.CityName);
            foreach (SelectListItem item in cList)
            {
                item.Selected = item.Value.Contains(hospital.City_ID.ToString());
            }
            ViewBag.CityList = cList;

            // Load list of districts
            //districtList = new List<District>();
            districtList = await LoadDistrict(model.CityID);
            model.DistrictID = (int)hospital.District_ID;
            SelectList dList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
            foreach (SelectListItem item in dList)
            {
                item.Selected = item.Value.Contains(hospital.District_ID.ToString());
            }
            ViewBag.DistrictList = dList;

            // Load list of districts
            //wardList = new List<Ward>();
            wardList = await LoadWard(model.DistrictID);
            model.WardID = (int)hospital.Ward_ID;
            SelectList wList = new SelectList(wardList, Constants.WardID, Constants.WardName);
            foreach (SelectListItem item in wList)
            {
                item.Selected = item.Value.Contains(hospital.Ward_ID.ToString());
            }
            ViewBag.WardList = wList;
            // Load list of hospital types

            hospitalTypeList = await HospitalUtil.LoadHospitalTypeAsync();
            SelectList typeList = new SelectList(hospitalTypeList, Constants.HospitalTypeID, Constants.HospitalTypeName);
            foreach (SelectListItem item in typeList)
            {
                item.Selected = item.Value.Contains(hospital.Hospital_Type.ToString());
            }
            ViewBag.HospitalTypeList = typeList;
            return View(model);
        }

        /// <summary>
        /// Post/Hospital/HospitalBasicInforUpdate
        /// </summary>
        /// <param name="model">HospitalModel</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost]
        public async Task<ActionResult> HospitalBasicInforUpdate(HospitalModel model)
        {
            try
            {
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    Hospital oldHospital = await Task.Run(() => (
                        from h in data.Hospitals
                        where h.Hospital_ID == model.HospitalID
                        select h).FirstOrDefault());
                    oldHospital.Address = model.FullAddress == null ? oldHospital.Address : model.FullAddress;
                    oldHospital.City_ID = model.CityID;
                    oldHospital.Ward_ID = model.WardID;
                    oldHospital.District_ID = model.DistrictID;
                    oldHospital.Hospital_Type = model.HospitalTypeID;

                    // Holiday time
                    string[] holidayTime = model.HolidayStartTime.Split(char.Parse(Constants.Minus));
                    string holidayStartTime = holidayTime[0].Trim();
                    model.HolidayStartTime = holidayStartTime;
                    string holidayEndTime = holidayTime[1].Trim();
                    model.HolidayEndTime = holidayEndTime;

                    oldHospital.Holiday_Start_Time = TimeSpan.Parse(model.HolidayStartTime);
                    oldHospital.Holiday_End_Time = TimeSpan.Parse(model.HolidayEndTime);

                    // Ordinary time
                    string[] OrdinaryTime = model.OrdinaryStartTime.Split(char.Parse(Constants.Minus));
                    string ordinaryStartTime = OrdinaryTime[0].Trim();
                    model.OrdinaryStartTime = ordinaryStartTime;
                    string ordinaryEndTime = OrdinaryTime[1].Trim();
                    model.OrdinaryEndTime = ordinaryEndTime;

                    oldHospital.Ordinary_Start_Time = TimeSpan.Parse(model.OrdinaryStartTime);
                    oldHospital.Ordinary_End_Time = TimeSpan.Parse(model.OrdinaryEndTime);

                    //phone number
                    string phoneNumber = model.PhoneNo;
                    if (!string.IsNullOrEmpty(model.PhoneNo2))
                    {
                        phoneNumber += Constants.Slash + model.PhoneNo2;
                    }
                    if (!string.IsNullOrEmpty(model.PhoneNo3))
                    {
                        phoneNumber += Constants.Slash + model.PhoneNo3;
                    }
                    oldHospital.Phone_Number = phoneNumber;

                    oldHospital.Email = model.HospitalEmail;
                    oldHospital.Website = model.Website;
                    oldHospital.Fax = model.Fax;
                    data.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                LoggingUtil.LogException(ex);
                return RedirectToAction(Constants.SystemFailureHospitalUserAction, Constants.ErrorController);
            }
        }
        #endregion

        #region update doctor information
        public async Task<ActionResult> ViewDoctorDetail(string DoctorID)
        {
            int tempDoctorID;
            Doctor doctor = null;
            try
            {
                if (!string.IsNullOrEmpty(DoctorID) && Int32.TryParse(DoctorID, out tempDoctorID))
                {
                    using (LinqDBDataContext data = new LinqDBDataContext())
                    {
                        doctor = await Task.Run(() => (
                            from d in data.Doctors
                            where d.Doctor_ID == tempDoctorID
                            select d).FirstOrDefault());
                        ViewBag.Doctor = doctor;
                        ViewBag.Photo = HospitalModel.LoadPhotoByPhotoID((int)doctor.Photo_ID);
                        //get all speciality of doctor
                        specialityList = await SpecialityUtil.LoadSpecialityAsync();
                        ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                        //get photo file path
                        Photo photo = await Task.Run(() => (
                            from p in data.Photos
                            where p.Photo_ID == doctor.Photo_ID
                            select p).SingleOrDefault());
                        DoctorModel = new DoctorModels();
                        DoctorModel.DoctorID = doctor.Doctor_ID;
                        DoctorModel.FirstName = doctor.First_Name;
                        DoctorModel.LastName = doctor.Last_Name;
                        DoctorModel.Fullname = DoctorModel.LastName + " " + DoctorModel.FirstName;
                        DoctorModel.Degree = doctor.Degree;
                        DoctorModel.Experience = doctor.Experience;
                        DoctorModel.Gender = doctor.Gender == true ? 1 : 0;
                        DoctorModel.PhotoFilePath = photo.File_Path;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingUtil.LogException(ex);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
            return View();
        }
        #endregion

        #endregion

        #region SonNX

        #region AJAX method

        /// <summary>
        /// GET: /Hospital/GetDistrictByCity
        /// </summary>
        /// <param name="cityId">City ID</param>
        /// <returns>Task[ActionResult] with JSON contains list of Districts</returns>
        public async Task<ActionResult> GetDistrictByCity(string cityId)
        {
            try
            {
                int tempCityId = 0;
                // Check if city ID is null or not
                if (!String.IsNullOrEmpty(cityId) && Int32.TryParse(cityId, out tempCityId))
                {
                    districtList = await LocationUtil.LoadDistrictInCityAsync(tempCityId);
                    var result = (from d in districtList
                                  select new
                                  {
                                      id = d.District_ID,
                                      name = d.Type + Constants.WhiteSpace + d.District_Name
                                  });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Return default value
                    districtList = new List<District>();
                    districtList.Add(new District { District_ID = 0 });
                    var result = (from d in districtList
                                  select new
                                  {
                                      id = 0,
                                      name = Constants.DisplayAllDistrict
                                  });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Hospital/GetWardByDistritct
        /// </summary>
        /// <param name="districtId">District ID</param>
        /// <returns>Task[ActionResult] with JSON contains list of Wards</returns>
        public async Task<ActionResult> GetWardByDistritct(string districtId)
        {
            try
            {
                int tempDistrictId = 0;
                // Check if city ID is null or not
                if (!String.IsNullOrEmpty(districtId) && Int32.TryParse(districtId, out tempDistrictId))
                {
                    wardList = await LocationUtil.LoadWardInDistrictAsync(tempDistrictId);
                    var result = (from w in wardList
                                  select new
                                  {
                                      id = w.Ward_ID,
                                      name = w.Type + Constants.WhiteSpace + w.Ward_Name
                                  });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Return default value
                    wardList = new List<Ward>();
                    return Json(wardList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Hospital/ChangeHospitalStatus
        /// </summary>
        /// <param name="hospitalId">Hosptal ID</param>
        /// <returns>
        /// Task[ActionResult] with JSON contains value
        /// indicating update process is successful or not
        /// 1: Successful
        /// 0: Failed
        /// </returns>
        public async Task<ActionResult> ChangeHospitalStatus(int hospitalId)
        {
            try
            {
                HospitalModel model = new HospitalModel();
                int result = await model.ChangeHospitalStatusAsync(hospitalId);
                return Json(new { value = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

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
        public async Task<ActionResult> CheckValidUserWithEmail(string email)
        {
            try
            {
                HospitalModel model = new HospitalModel();
                int result = await model.CheckValidUserWithEmail(email);
                return Json(new { value = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
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
        public async Task<ActionResult> CheckValidHospitalWithAddress(string address,
            int cityId, int districtId, int wardId)
        {
            try
            {
                HospitalModel model = new HospitalModel();
                int result = await model.CheckValidHospitalWithAddress(address,
                    cityId, districtId, wardId);
                return Json(new { value = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        #region Display Hospital List

        /// <summary>
        /// GET: /Hospital/HospitalList
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> HospitalList()
        {
            IPagedList<SP_LOAD_HOSPITAL_LISTResult> pagedHospitalList = null;
            try
            {
                // Load list of cities
                cityList = await LocationUtil.LoadCityAsync();
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);

                // Load list of districts
                districtList = new List<District>();
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);

                // Load list of hospital types
                hospitalTypeList = await HospitalUtil.LoadHospitalTypeAsync();
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.HospitalTypeID, Constants.HospitalTypeName);

                // Load list of status
                ViewBag.CurrentStatus = true;

                // Declare new hospital list
                pagedHospitalList = new
                    List<SP_LOAD_HOSPITAL_LISTResult>().ToPagedList(1, Constants.PageSize);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            return View(pagedHospitalList);
        }

        /// <summary>
        /// GET: /Hospital/DisplayHospitalList
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> DisplayHospitalList(HospitalModel model, int? page)
        {
            try
            {
                // Cacading again drop down list
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.HospitalTypeID, Constants.HospitalTypeName);
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
                ViewBag.CurrentStatus = model.IsActive;

                // Check if page parameter is null
                if (page == null)
                {
                    page = 1;
                }

                // Load list of hospital
                List<SP_LOAD_HOSPITAL_LISTResult> hospitalList =
                    await model.LoadListOfHospital(model.HospitalName,
                    model.CityID, model.DistrictID, model.HospitalTypeID,
                    model.IsActive);

                // Handle query string
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove(Constants.PageUrlRewriting);
                ViewBag.Query = queryString.ToString();

                // Return value to view
                IPagedList<SP_LOAD_HOSPITAL_LISTResult> pagedHospitalList =
                    hospitalList.ToPagedList(page.Value, Constants.PageSize);
                return View(Constants.InitialHospitalListAction, pagedHospitalList);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        #region Add Hospital

        /// <summary>
        /// GET: /Hospital/AddHospital
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> AddHospital()
        {
            try
            {
                // Load list of cities
                cityList = await LocationUtil.LoadCityAsync();
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);

                // Load list of districts
                districtList = new List<District>();
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);

                // Load list of districts
                wardList = new List<Ward>();
                ViewBag.WardList = new SelectList(wardList, Constants.WardID, Constants.WardName);

                // Load list of hospital types
                hospitalTypeList = await HospitalUtil.LoadHospitalTypeAsync();
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.HospitalTypeID, Constants.HospitalTypeName);

                //Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                //Load list of services
                serviceList = await ServiceFacilityUtil.LoadServiceAsync();
                ViewBag.ServiceList = serviceList;

                // Load list of facilitites
                facilityList = await ServiceFacilityUtil.LoadFacilityAsync();
                ViewBag.FacilityList = facilityList;
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            return View();
        }

        /// <summary>
        /// POST: /Hospital/AddHospital
        /// </summary>
        /// <param name="model">Hospital Model</param>
        /// <param name="files">Photo files</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpPost]
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [ValidateInput(false)]
        public async Task<ActionResult> AddHospital(HospitalModel model, List<HttpPostedFileBase> file)
        {
            try
            {
                // Prepare data
                int result = 0;
                model.CreatedPerson = Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.InsertHospitalAsync(model);
                }

                // Assign value for drop down list
                PassValueDropdownlist();

                // Check if insert process is success or not
                if (result == 0)
                {
                    ViewBag.AddHospitalStatus = 0.ToString() + Constants.Minus + model.HospitalName;
                }
                else
                {
                    ViewBag.AddHospitalStatus = 1.ToString() + Constants.Minus + model.HospitalName;
                    ModelState.Clear();
                    return View();
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            return View(model);
        }

        #endregion

        #region Update Hospital

        /// <summary>
        /// Update hospital information
        /// </summary>
        /// <param name="hospitalId">Hospital ID</param>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> UpdateHospital(int hospitalId)
        {
            HospitalModel model = new HospitalModel();
            try
            {
                //  Load hospital in database
                model = await model.LoadSpecificHospital(hospitalId);

                #region cascading dropdownlist

                // Load list of cities
                cityList = await LocationUtil.LoadCityAsync();
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);

                // Load list of districts
                districtList = await LocationUtil.LoadDistrictInCityAsync(model.CityID);
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);

                // Load list of districts
                wardList = await LocationUtil.LoadWardInDistrictAsync(model.DistrictID);
                ViewBag.WardList = new SelectList(wardList, Constants.WardID, Constants.WardName);

                // Load list of hospital types
                hospitalTypeList = await HospitalUtil.LoadHospitalTypeAsync();
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.HospitalTypeID, Constants.HospitalTypeName);

                //Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                //Load list of services
                serviceList = await ServiceFacilityUtil.LoadServiceAsync();
                ViewBag.ServiceList = serviceList;

                // Load list of facilitites
                facilityList = await ServiceFacilityUtil.LoadFacilityAsync();
                ViewBag.FacilityList = facilityList;

                #endregion
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            return View(model);
        }

        #endregion

        #region Private method

        /// <summary>
        /// Pass existed value for drop down list on view
        /// </summary>
        private void PassValueDropdownlist()
        {
            ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
            ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
            ViewBag.WardList = new SelectList(wardList, Constants.WardID, Constants.WardName);
            ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.HospitalTypeID, Constants.HospitalTypeName);
            ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
            ViewBag.ServiceList = serviceList;
            ViewBag.FacilityList = facilityList;
        }

        #endregion

        #endregion
    }
}
