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
        
        public static int hospitalID = 25;
        public Hospital hospital = null;
        public static HospitalModel model = null;
        public static DoctorModels DoctorModel = null;
        //
        // GET: /Hospital/
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> Index()
        {
            model = new HospitalModel();
            hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalID);
            hospitalTypeList = await HospitalUtil.LoadTypeInHospitalTypeAsync(hospitalID);
            string hospitalTypeName = model.LoadHospitalTypeInList((int)hospital.Hospital_Type, hospitalTypeList);

            //assign value for model attributes

            ViewBag.hospitalTypeList = new SelectList(hospitalTypeList, Constants.HospitalTypeID, Constants.HospitalTypeName, hospital.Hospital_Type);
            ViewBag.HospitalTypeName = hospitalTypeName;
            ViewBag.HospitalObject = hospital;
model.HospitalID = hospitalID;
            model.HospitalName = hospital.Hospital_Name;
            model.FullAddress = hospital.Address;
            model.Website = hospital.Website;
            model.PhoneNo = hospital.Phone_Number;
            model.Fax = hospital.Fax;
            model.HospitalTypeName = model.LoadHospitalTypeInList((int)hospital.Hospital_Type, hospitalTypeList);
            //load doctor of hospital
            //model.DoctorList = await HospitalUtil.LoadDoctorInDoctorHospitalAsync(hospitalID);
//load speciality of hospital
            specialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
            ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
            //load facility of hospital
            List<Facility> facilityList = await HospitalUtil.LoadFacilityInHospitalFacilityAsync(hospitalID);
            ViewBag.FacilityList = facilityList;
            //load service of hospital
            List<Service> serviceList = await HospitalUtil.LoadServiceInHospitalServiceAsync(hospitalID);
            ViewBag.ServiceList = serviceList;
            return View();
        }
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

        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> ViewDoctorDetail(string id)
        {
            int tempDoctorID;
            Doctor doctor = null;
            try
            {
                if (!string.IsNullOrEmpty(id) && Int32.TryParse(id, out tempDoctorID))
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
                        specialityList = specialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
                        List<Speciality> selectList = await SpecialityUtil.LoadSpecialityInDoctorSpeciality(doctor.Doctor_ID);
                        ViewBag.SpecialityList = specialityList;
                        
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
                        DoctorModel.Gender = doctor.Gender==true?1:0;
                        DoctorModel.PhotoFilePath = photo.File_Path;
                        DoctorModel.SpecialityList = selectList;
                    }
                }
                return View(DoctorModel);
            }
            catch (Exception ex)
            {
                LoggingUtil.LogException(ex);
                return RedirectToAction(Constants.SystemFailureHospitalUserAction, Constants.SystemFailureHospitalUserAction);
            }
        }

        /// <summary>
        /// Get: [Hospital/HospitalBasicInforUpdate]
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="website"></param>
        /// <param name="hospitalType"></param>
        /// <param name="phoneNo"></param>
        /// <param name="fax"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> HospitalBasicInforUpdate(string id, string name,string address,
            string website,string hospitalType, string phoneNo,string fax)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            //else
            //{
                try
                {
                    using (LinqDBDataContext data = new LinqDBDataContext())
                    {
                        Hospital oldHospital = await Task.Run(() => (
                            from h in data.Hospitals
                            where h.Hospital_ID == Int32.Parse(id)
                            select h).FirstOrDefault());
                        if (!oldHospital.Hospital_Name.Equals(name))
                        {
                            oldHospital.Hospital_Name = name;
                        }
                        if (!oldHospital.Website.Equals(website))
                        {
                            oldHospital.Website = website;
                        }
                        if (oldHospital.Hospital_Type != Int32.Parse(hospitalType))
                        {
                            oldHospital.Hospital_Type = Int32.Parse(hospitalType);
                        }
                        if (!oldHospital.Phone_Number.Equals(phoneNo))
                        {
                            oldHospital.Phone_Number = phoneNo;
                        }
                        if (!oldHospital.Fax.Equals(fax))
                        {
                            oldHospital.Fax = fax;
                        }
                        if (!oldHospital.Address.Equals(address))
                        {
                            oldHospital.Address = address;
                        }
                        data.SubmitChanges();
                        hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(Int32.Parse(id));
                        return Json(hospital, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtil.LogException(ex);
                    return RedirectToAction(Constants.SystemFailureHospitalUserAction, Constants.ErrorController);
                }
            //}
        }

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
                return Json(new { value = result}, JsonRequestBehavior.AllowGet);
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
        /// </returns>
        public async Task<ActionResult> CheckValidUser(string email)
        {
            try
            {
                return Json(1, JsonRequestBehavior.AllowGet);
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

                // Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                // Load list of services
                serviceList = await ServiceFacilityUtil.LoadServiceAsync();
                int a = serviceList.Count();
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
        /// Load paritial view EditHospitalDescription
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult EditHospitalDescription()
        {
            return PartialView("EditHospitalDescription");
        }

        /// <summary>
        /// POST: /Hospital/AddHospital
        /// </summary>
        /// <param name="model">Hospital Model</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpPost]
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> AddHospital(HospitalModel model)
        {
            try
            {
                // Prepare data
                int result = 0;

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
                if (model.SelectedSpecialities.Count != 0)
                {
                    foreach (string data in model.SelectedSpecialities)
                    {
                        speciality += specialityList + Constants.VerticalBar.ToString() + data;
                    }
                }

                // Service list
                string service = string.Empty;
                if (model.SelectedServices.Count != 0)
                {
                    foreach (string data in model.SelectedServices)
                    {
                        service += specialityList + Constants.VerticalBar.ToString() + data;
                    }
                }
                

                // Facility list
                string facility = string.Empty;
                if (model.SelectedFacilities.Count != 0)
                {
                    foreach (string data in model.SelectedFacilities)
                    {
                        facility += specialityList + Constants.Minus + data;
                    }
                }       

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.InsertHospitalAsync(model, speciality, service, facility);
                }

                // Check if insert process is success or not
                if (result == 0)
                {
                    ViewBag.InsertStatus = 0.ToString() + Constants.Minus + model.HospitalName;
                }
                else
                {
                    model = new HospitalModel();
                    ViewBag.InsertStatus = 1.ToString() + Constants.Minus + model.HospitalName;
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

        #endregion
    }
}
