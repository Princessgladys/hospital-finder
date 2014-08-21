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
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

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
        public static HospitalModel HospitalModel = null;

        #region load location
        /// <summary>
        /// Hospital/Loadcity
        /// </summary>
        /// <returns>Task<List<City>></returns>
        public async Task<List<City>> LoadCity()
        {
            List<City> list = await LocationUtil.LoadCityAsync();
            City city = null;
            List<City> result = new List<City>();
            foreach (City c in list)
            {
                city = new City();
                city.City_ID = c.City_ID;
                city.City_Name = c.Type + Constants.WhiteSpace + c.City_Name;
                result.Add(city);
            }
            return result;
        }

        /// <summary>
        /// Hospital/LoadDistrict
        /// </summary>
        /// <param name="cityID">cityID</param>
        /// <returns>Task<List<District>></returns>
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
        /// <summary>
        /// Hospital/LoadWard
        /// </summary>
        /// <param name="districtID">districtID</param>
        /// <returns>Task<List<Ward>></returns>
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
            HospitalModel = new HospitalModel();
            hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalID);
            hospitalTypeList = await HospitalUtil.LoadTypeInHospitalTypeAsync(hospitalID);

            //assign value for model attributes
            HospitalModel.HospitalID = hospitalID;
            HospitalModel.HospitalName = hospital.Hospital_Name;
            HospitalModel.FullAddress = hospital.Address;
            HospitalModel.Website = hospital.Website;
            HospitalModel.PhoneNo = hospital.Phone_Number;
            HospitalModel.Fax = hospital.Fax;
            HospitalModel.HospitalTypeName = HospitalModel.LoadHospitalTypeInList((int)hospital.Hospital_Type, hospitalTypeList);
            DateTime start;
            DateTime end;
            if (hospital.Ordinary_Start_Time == null && hospital.Ordinary_End_Time == null)
            {
                //startTime = new TimeSpan(0, 0, 0);
                HospitalModel.OrdinaryStartTime = null;
            }
            else
            {
                start = DateTime.Today + (TimeSpan)hospital.Ordinary_Start_Time;
                end = DateTime.Today + (TimeSpan)hospital.Ordinary_End_Time;
                HospitalModel.OrdinaryStartTime = start.ToString("HH:mm") + " - " + end.ToString("HH:mm");
            }
            if (hospital.Holiday_Start_Time == null && hospital.Holiday_End_Time == null)
            {
                //startTime = new TimeSpan(0, 0, 0);
                HospitalModel.HolidayStartTime = null;
            }
            else
            {
                start = DateTime.Today + (TimeSpan)hospital.Holiday_Start_Time;
                end = DateTime.Today + (TimeSpan)hospital.Holiday_End_Time;
                HospitalModel.HolidayStartTime = start.ToString("HH:mm") + " - " + end.ToString("HH:mm");
            }
            if (hospital.Is_Allow_Appointment == null)
            {
                HospitalModel.IsAllowAppointment = false;
            }
            else
            {
                HospitalModel.IsAllowAppointment = true;
            }

            //load speciality of hospital
            specialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
            ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

            //load facility of hospital
            ViewBag.FacilityList = await ServiceFacilityUtil.LoadFacilityOfHospitalAsync(hospitalID);

            //load service of hospital
            ViewBag.ServiceList = await ServiceFacilityUtil.LoadServiceOfHospitalAsync(hospitalID);

            return View(HospitalModel);
        }

        #region search doctor
        public async Task<ActionResult> SearchDoctor(string SpecialityID, string DoctorName, string HospitalID)
        {
            try
            {
                int tempSpecialityID, tempHospitalID;
                doctorList = new List<Doctor>();
                ViewBag.Hospital = Int32.Parse(HospitalID);
                if (SpecialityID == "")
                {
                    SpecialityID = "0";
                }
                if (!String.IsNullOrEmpty(SpecialityID) && Int32.TryParse(SpecialityID, out tempSpecialityID)
                    && Int32.TryParse(HospitalID, out tempHospitalID))
                {
                    doctorList = await HospitalUtil.SearchDoctor(DoctorName, tempSpecialityID, tempHospitalID);
                    ViewBag.DoctorList = doctorList;
                    ViewBag.HospitalID = tempHospitalID;
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
            HospitalModel = new HospitalModel();
            hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalId);

            //assign value for model attributes
            HospitalModel.HospitalID = hospitalId;
            HospitalModel.HospitalName = hospital.Hospital_Name;
            HospitalModel.FullAddress = hospital.Address;
            HospitalModel.HospitalEmail = hospital.Email;
            HospitalModel.Website = hospital.Website;
            HospitalModel.PhoneNo = hospital.Phone_Number;
            HospitalModel.Fax = hospital.Fax;

            TimeSpan startTime;
            TimeSpan endTime;
            DateTime start;
            DateTime end;
            //ordinary time
            if (hospital.Ordinary_Start_Time == null && hospital.Ordinary_End_Time == null)
            {
                //startTime = new TimeSpan(0, 0, 0);
                HospitalModel.OrdinaryStartTime = null;
            }
            else
            {
                startTime = (TimeSpan)hospital.Ordinary_Start_Time;

                endTime = (TimeSpan)hospital.Ordinary_End_Time;

                start = DateTime.Today + startTime;
                end = DateTime.Today + endTime;
                HospitalModel.OrdinaryStartTime = start.ToString("HH:mm") + Constants.Minus + end.ToString("HH:mm");
            }
            // holiday time
            if (hospital.Holiday_Start_Time == null && hospital.Holiday_End_Time == null)
            {
                HospitalModel.HolidayStartTime = null;
            }
            else
            {
                startTime = (TimeSpan)hospital.Holiday_Start_Time;
                endTime = (TimeSpan)hospital.Holiday_End_Time;
                start = DateTime.Today + startTime;
                end = DateTime.Today + endTime;
                HospitalModel.HolidayStartTime = start.ToString("HH:mm") + Constants.Minus + end.ToString("HH:mm");
            }
            //is allow appointment
            if (hospital.Is_Allow_Appointment == null)
            {
                HospitalModel.IsAllowAppointment = false;
            }
            else
            {
                HospitalModel.IsAllowAppointment = true;
            }

            // Load list of cities
            cityList = await LoadCity();
            HospitalModel.CityID = (int)hospital.City_ID;
            //foreach (City c in cityList)
            //{
            //    if (c.City_ID == hospital.City_ID)
            //    {
            //        model.CityName = c.City_Name;
            //    }
            //}
            ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);

            // Load list of districts
            districtList = await LoadDistrict(HospitalModel.CityID);
            HospitalModel.DistrictID = (int)hospital.District_ID;
            //foreach (District d in districtList)
            //{
            //    if (d.District_ID == hospital.District_ID)
            //    {
            //        model.DistrictName = d.District_Name;
            //    }
            //}
            ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);

            // Load list of districts
            //wardList = new List<Ward>();
            wardList = await LoadWard(HospitalModel.DistrictID);
            HospitalModel.WardID = (int)hospital.Ward_ID;
            //foreach (Ward w in wardList)
            //{
            //    if (w.Ward_ID == hospital.Ward_ID)
            //    {
            //        model.WardName = w.Ward_Name;
            //    }
            //}
            ViewBag.WardList = new SelectList(wardList, Constants.WardID, Constants.WardName);
            // Load list of hospital types

            hospitalTypeList = await HospitalUtil.LoadHospitalTypeAsync();
            SelectList typeList = new SelectList(hospitalTypeList, Constants.TypeID, Constants.TypeName);
            HospitalModel.HospitalTypeID = (int)hospital.Hospital_Type;
            ViewBag.HospitalTypeList = typeList;
            return View(HospitalModel);
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
                    //full address
                    if (model.StreetAddress != null)
                    {
                        model.FullAddress = string.Format("{0} {1}, {2}, {3}, {4}",
                        model.LocationAddress, model.StreetAddress, model.WardName,
                        model.DistrictName, model.CityName);
                        oldHospital.Address = model.FullAddress;
                    }

                    oldHospital.City_ID = model.CityID;
                    oldHospital.Ward_ID = model.WardID;
                    oldHospital.District_ID = model.DistrictID;
                    oldHospital.Hospital_Type = model.HospitalTypeID;
                    oldHospital.Is_Allow_Appointment = model.IsAllowAppointment;

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

        #region update speciality-service-facility

        /// <summary>
        /// Get/SpecialityServiceFacilityUpdate
        /// </summary>
        /// <param name="hospitalID">hospitalID</param>
        /// <returns>Task<ActionResult></returns>
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> SpecialityServiceFacilityUpdate(int hospitalID)
        {
            HospitalModel = new HospitalModel();
            //Load list of specialities
            List<Speciality> specialitySelectList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
            List<String> selectList = new List<string>();
            foreach (Speciality sp in specialitySelectList)
            {
                selectList.Add(sp.Speciality_ID.ToString());
            }
            specialityList = await SpecialityUtil.LoadSpecialityAsync();
            MultiSelectList list = new MultiSelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName, selectList);
            foreach (var speciality in list)
            {
                foreach (var selectValue in selectList)
                {
                    if (speciality.Value.Equals(selectValue))
                    {
                        speciality.Selected = true;
                    }
                }
            }

            ViewBag.SpecialityList = list;
            HospitalModel.SelectedSpecialities = selectList;

            //Load list of services
            serviceList = await ServiceFacilityUtil.LoadServiceAsync();
            List<ServiceEntity> serviceSelectList = await ServiceFacilityUtil.LoadServiceOfHospitalAsync(hospitalID);
            selectList = new List<string>();
            foreach (ServiceEntity se in serviceSelectList)
            {
                selectList.Add(se.Service_ID.ToString());
            }
            ViewBag.ServiceList = serviceList;
            HospitalModel.SelectedServices = selectList;

            // Load list of facilitites
            facilityList = await ServiceFacilityUtil.LoadFacilityAsync();
            List<FacilityEntity> facilitySelectList = await ServiceFacilityUtil.LoadFacilityOfHospitalAsync(hospitalID);
            selectList = new List<string>();
            foreach (FacilityEntity fe in facilitySelectList)
            {
                selectList.Add(fe.Facility_ID.ToString());
            }
            HospitalModel.SelectedFacilities = selectList;
            ViewBag.FacilityList = facilityList;
            return View(HospitalModel);
        }

        /// <summary>
        /// Post/SpecialityServiceFacilityUpdate
        /// </summary>
        /// <param name="model">HospitalModel</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost]
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> SpecialityServiceFacilityUpdate(HospitalModel model)
        {
            try
            {
                string speciality = string.Empty;
                int result = 0;
                List<Speciality> specialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(model.HospitalID);
                string selectedvalue;
                bool flag = false;
                if ((model.SelectedSpecialities != null) && (model.SelectedSpecialities.Count != 0))
                {
                    for (int n = 0; n < model.SelectedSpecialities.Count; n++)
                    {
                        selectedvalue = model.SelectedSpecialities[n];
                        for (int index = 0; index < specialityList.Count; index++)
                        {
                            if (specialityList[index].Speciality_ID == Int32.Parse(selectedvalue))
                            {
                                flag = true;
                            }
                        }
                        if (flag == false)
                        {
                            if (n == (model.SelectedSpecialities.Count - 1))
                            {
                                speciality += selectedvalue;
                            }
                            else
                            {
                                speciality += selectedvalue +
                                    Constants.VerticalBar.ToString();
                            }
                        }
                    }
                }

                // Service list
                string service = string.Empty;
                flag = false;
                List<ServiceEntity> serviceList = await ServiceFacilityUtil.LoadServiceOfHospitalAsync(model.HospitalID);
                if ((model.SelectedServices != null) && (model.SelectedServices.Count != 0))
                {
                    for (int n = 0; n < model.SelectedServices.Count; n++)
                    {
                        selectedvalue = model.SelectedServices[n];
                        for (int index = 0; index < serviceList.Count; index++)
                        {
                            if (serviceList[index].Service_ID == Int32.Parse(selectedvalue))
                            {
                                flag = true;
                            }
                        }
                        if (flag == false)
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
                }
                // Facility list
                string facility = string.Empty;
                flag = false;
                List<FacilityEntity> facilityList = await ServiceFacilityUtil.LoadFacilityOfHospitalAsync(model.HospitalID);
                if ((model.SelectedFacilities != null) && (model.SelectedFacilities.Count != 0))
                {
                    for (int n = 0; n < model.SelectedFacilities.Count; n++)
                    {
                        selectedvalue = model.SelectedFacilities[n];
                        for (int index = 0; index < facilityList.Count; index++)
                        {
                            if (facilityList[index].Facility_ID == Int32.Parse(selectedvalue))
                            {
                                flag = true;
                            }
                        }
                        if (flag == false)
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
                }
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = data.SP_UPDATE_SPECIALITY_SERVICE_FACILITY(model.HospitalID, speciality, service, facility);
                }
                if (result != 0)
                {
                    //ViewBag.AddHospitalStatus = 0.ToString() + Constants.Minus + model.HospitalName;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.AddHospitalStatus = 1.ToString() + Constants.Minus + model.HospitalName;
                    ModelState.Clear();
                    return View();
                }
            }
            catch (Exception ex)
            {
                LoggingUtil.LogException(ex);
                return RedirectToAction(Constants.SystemFailureHospitalUserAction, Constants.ErrorController);
            }
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
        /// <param name="hospitalName">Hospital name</param>
        /// <param name="address">Full Address</param>
        /// <returns> 1: Not duplicated, 0: Duplicated</returns>
        public async Task<ActionResult> CheckValidHospitalWithAddress(string hospitalName, string address)
        {
            try
            {
                HospitalModel model = new HospitalModel();
                int result = await LocationUtil.CheckValidHospitalWithAddress(hospitalName, address);
                return Json(new { value = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Save uploaded pictures
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveUploadFile()
        {
            // Add file to server and add list of file to session
            List<string> filePath = new List<string>();
            string fName = string.Empty;

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //Save file content goes here
                fName = file.FileName;
                if (file != null && file.ContentLength > 0)
                {
                    filePath.Add(FileUtil.SaveFileToServer(file,
                        Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]), 1));
                }
            }

            // Add to session and return
            Session.Add(Constants.FileInSession, filePath);
            return Json(new { Message = fName });
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
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.TypeID, Constants.TypeName);

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
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.TypeID, Constants.TypeName);
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
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.TypeID, Constants.TypeName);

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
        /// <returns>Task[ActionResult]</returns>
        [HttpPost]
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [ValidateInput(false)]
        public async Task<ActionResult> AddHospital(HospitalModel model)
        {
            try
            {
                // Prepare data
                int result = 0;
                model.CreatedPerson = Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);

                // Take file path from session
                List<string> filePath = new List<string>();
                if (Session[Constants.FileInSession] != null)
                {
                    filePath = (List<string>)Session[Constants.FileInSession];
                    for (int n = 0; n < filePath.Count; n++)
                    {
                        if (n == (filePath.Count - 1))
                        {
                            model.PhotoFilesPath += filePath[n];
                        }
                        else
                        {
                            model.PhotoFilesPath += filePath[n] +
                                Constants.VerticalBar.ToString();
                        }
                    }
                    // Set photo session to null value
                    Session[Constants.FileInSession] = null;
                }
                else
                {
                    model.PhotoFilesPath = string.Empty;
                }

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.InsertHospitalAsync(model, 1);
                }

                // Assign value for drop down list
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
                ViewBag.WardList = new SelectList(wardList, Constants.WardID, Constants.WardName);
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.TypeID, Constants.TypeName);
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
                ViewBag.ServiceList = serviceList;
                ViewBag.FacilityList = facilityList;

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
        [Authorize(Roles = Constants.AdministratorRoleName+","+Constants.HospitalUserRoleName)]
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
                var districtResult = (from d in districtList
                                      select new
                                      {
                                          District_ID = d.District_ID,
                                          District_Name = d.Type + Constants.WhiteSpace + d.District_Name
                                      });
                ViewBag.DistrictList = new SelectList(districtResult, Constants.DistrictID, Constants.DistrictName);

                // Load list of districts
                wardList = await LocationUtil.LoadWardInDistrictAsync(model.DistrictID);
                var wardResult = (from w in wardList
                                  select new
                                  {
                                      Ward_ID = w.Ward_ID,
                                      Ward_Name = w.Type + Constants.WhiteSpace + w.Ward_Name
                                  });
                ViewBag.WardList = new SelectList(wardResult, Constants.WardID, Constants.WardName);

                // Load list of hospital types
                hospitalTypeList = await HospitalUtil.LoadHospitalTypeAsync();
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.TypeID, Constants.TypeName);

                //Load list of specialities
                model.SpecialityList = await SpecialityUtil.LoadSpecialityAsync();
                //ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                //Load list of services
                serviceList = await ServiceFacilityUtil.LoadServiceAsync();
                foreach (var service in serviceList)
                {
                    foreach (var selectValue in model.SelectedServices)
                    {
                        if (service.Value.Equals(selectValue))
                        {
                            service.Selected = true;
                        }
                    }
                }
                ViewBag.ServiceList = serviceList;

                // Load list of facilitites
                facilityList = await ServiceFacilityUtil.LoadFacilityAsync();
                foreach (var facility in facilityList)
                {
                    foreach (var selectValue in model.SelectedFacilities)
                    {
                        if (facility.Value.Equals(selectValue))
                        {
                            facility.Selected = true;
                        }
                    }
                }
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

        /// <summary>
        /// POST: /Hospital/UpdateHospital
        /// </summary>
        /// <param name="model">Hospital Model</param>
        /// <param name="files">Photo files</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpPost]
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName + "," + Constants.HospitalUserRoleName)]
        [ValidateInput(false)]
        public async Task<ActionResult> UpdateHospital(HospitalModel model, List<HttpPostedFileBase> file)
        {
            try
            {
                // Prepare data
                int result = 0;
                string updatedContent = string.Empty;
                bool flag=false;
                model.CreatedPerson = Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.UpdateHospitalAsync(model);
                    updatedContent = await model.CheckModelIsUpdated(model);
                    string currentEmail = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
                    if (User.IsInRole("3"))
                    {
                        List<string> adminGmailList = (from u in data.Users where u.Role_ID == 1 select u.Email).ToList();
                        //GoogleUtil.SendEmailToAdmin(currentEmail, adminGmailList, model.HospitalName, updatedContent);
                    }
                    else
                    {
                        List<string> hospitalUserGmailList = (from u in data.Users
                                                              where u.Role_ID == 3
                                                                  && u.Hospital_ID == model.HospitalID
                                                              select u.Email).ToList();
                        //GoogleUtil.SendEmailToHospitalUser(currentEmail, hospitalUserGmailList, model.HospitalName, updatedContent);
                    }
                }

                #region cascading dropdownlist

                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
                ViewBag.WardList = new SelectList(wardList, Constants.WardID, Constants.WardName);
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.TypeID, Constants.TypeName);

                //Load list of specialities
                model.SpecialityList = await SpecialityUtil.LoadSpecialityAsync();
                //ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                //Load list of services
                serviceList = await ServiceFacilityUtil.LoadServiceAsync();
                if (model.SelectedServices != null)
                {
                    foreach (var service in serviceList)
                    {
                        foreach (var selectValue in model.SelectedServices)
                        {
                            if (service.Value.Equals(selectValue))
                            {
                                service.Selected = true;
                            }
                        }
                    }
                }
                ViewBag.ServiceList = serviceList;

                // Load list of facilitites
                facilityList = await ServiceFacilityUtil.LoadFacilityAsync();
                if (model.SelectedFacilities != null)
                {
                    foreach (var facility in facilityList)
                    {
                        foreach (var selectValue in model.SelectedFacilities)
                        {
                            if (facility.Value.Equals(selectValue))
                            {
                                facility.Selected = true;
                            }
                        }
                    }
                }
                ViewBag.FacilityList = facilityList;

                #endregion

                // Check if insert process is success or not
                if (result == 0)
                {
                    ViewBag.UpdateHospitalStatus = 0.ToString() + Constants.Minus + model.HospitalName;
                }
                else
                {
                    ViewBag.UpdateHospitalStatus = 1.ToString() + Constants.Minus + model.HospitalName;
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

        #region Import Excel

        /// <summary>
        /// GET: /Hospital/ImportExcel
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult ImportExcel()
        {
            return View(new List<HospitalModel>());
        }

        /// <summary>
        /// POST: /Hospital/ImportExcel
        /// </summary>
        /// <param name="model">List of Hospital [HospitalModel]</param>
        /// <param name="file">Upload file</param>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> ImportExcel(List<HospitalModel> model, HttpPostedFileBase file)
        {
            try
            {
                // Indicate which button is clicked
                string button = Request[Constants.Button];

                // Upload file
                if (Constants.ButtonUpload.Equals(button))
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        WebClient client = new WebClient();
                        HospitalModel mdel = new HospitalModel();
                        List<HospitalModel> hospitalList = await mdel.HandleExcelFileData(file,
                            Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]));
                        string geoJsonResult = string.Empty;

                        // Handle coordinate
                        foreach (HospitalModel record in hospitalList)
                        {
                            if (!string.IsNullOrEmpty(record.HospitalName))
                            {
                                geoJsonResult = client.DownloadString(string.Concat(
                                    Constants.GeoCodeJsonQuery, record.HospitalName, Constants.Comma, record.FullAddress));
                                JObject geoJsonObject = JObject.Parse(geoJsonResult);
                                if (Constants.Ok.Equals(geoJsonObject.Value<string>(Constants.GeoCodeStatus)))
                                {
                                    record.Coordinate = string.Format("{0}, {1}",
                                        geoJsonObject[Constants.GeoCodeResults].
                                            First[Constants.GeoCodeGemometry][Constants.GeoCodeLocation].
                                            Value<double>(Constants.GeoCodeLatitude),
                                        geoJsonObject[Constants.GeoCodeResults].
                                            First[Constants.GeoCodeGemometry][Constants.GeoCodeLocation].
                                            Value<double>(Constants.GeoCodeLongitude));
                                }
                            } 
                        }

                        // Return list of hospital to view
                        return View(hospitalList);
                    }
                }

                // Add hospital list to data
                if (Constants.ButtonConfirm.Equals(button))
                {
                    if (model != null)
                    {
                        foreach (HospitalModel record in model)
                        {
                            if (record.RecordStatus != 0)
                            {
                                record.CreatedPerson =
                                    Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);
                                await record.InsertHospitalAsync(record, 0);
                            }
                        }
                    }

                    ViewBag.AddStatus = 1;
                    return View(new List<HospitalModel>());
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            return View();
        }

        #endregion

        #endregion
    }
}
