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

        #region VietLP
        [LayoutInjecter(Constants.HospitalUserLayout)]
        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public async Task<ActionResult> DoctorList(string doctorName = "", int page = 1)
        {
            try
            {
                int hospitalId = 0;

                string email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
                int? temp = AccountModel.LoadUserByEmail(email).Hospital_ID;
                if (temp == null)
                {
                    temp = 0;
                }
                hospitalId = (int)temp;

                ViewBag.DoctorList = DoctorModel.LoadDoctorList(hospitalId, doctorName.Trim()).ToPagedList(page, Constants.PageSize);
                ViewBag.DoctorName = doctorName;
                ViewBag.DeactivateStatus = TempData["DeactivateStatus"];
                return View();
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [HttpGet]
        [LayoutInjecter(Constants.HospitalUserLayout)]
        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public async Task<ActionResult> AddDoctor()
        {
            try
            {
                DoctorModel model = new DoctorModel();

                string email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
                model.UploadedPerson = Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);
                int hospitalId = (int)AccountModel.LoadUserByEmail(email).Hospital_ID;
                HospitalEntity hospital = await HomeModel.LoadHospitalById(hospitalId);
                model.HospitalID = hospital.Hospital_ID;
                model.HospitalName = hospital.Hospital_Name;

                List<SelectListItem> genderSelectListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "1", Text = "Nam"},
                                                                    new SelectListItem {Value = "2", Text = "Nữ"}
                                                                };
                ViewBag.GenderTypeList = new SelectList(genderSelectListItem, "Value", "Text", 1);
                List<SelectListItem> daySelectListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "2", Text = "Thứ Hai"},
                                                                    new SelectListItem {Value = "3", Text = "Thứ Ba"},
                                                                    new SelectListItem {Value = "4", Text = "Thứ Tư"},
                                                                    new SelectListItem {Value = "5", Text = "Thứ Năm"},
                                                                    new SelectListItem {Value = "6", Text = "Thứ Sáu"},
                                                                    new SelectListItem {Value = "7", Text = "Thứ Bảy"},
                                                                    new SelectListItem {Value = "8", Text = "Chủ Nhật"},
                                                                };
                ViewBag.DayTypeList = new SelectList(daySelectListItem, "Value", "Text");
                //Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
                return View(model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }


        [HttpPost]
        [LayoutInjecter(Constants.HospitalUserLayout)]
        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public async Task<ActionResult> AddDoctor(DoctorModel model)
        {
            try
            {
                // Upload Photo
                HttpPostedFileBase file = Request.Files["file"];
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        if (file.ContentType.ToLower() == "image/jpg" ||
                            file.ContentType.ToLower() == "image/jpeg" ||
                            file.ContentType.ToLower() == "image/pjpeg" ||
                            file.ContentType.ToLower() == "image/gif" ||
                            file.ContentType.ToLower() == "image/x-png" ||
                            file.ContentType.ToLower() == "image/png")
                        {
                            string filePath = (FileUtil.SaveFileToServer(file, Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]), 1));
                            model.PhotoFilePath = filePath;
                        }
                    }
                }

                if (model.FirstName != null)
                {
                    model.FirstName.Trim();
                }
                if (model.LastName != null)
                {
                    model.LastName.Trim();
                }
                if (model.Degree != null)
                {
                    model.Degree.Trim();
                }
                if (model.Experience != null)
                {
                    model.Experience.Trim();
                }
                if (model.SpecialityList == null)
                {
                    model.SpecialityList = new List<int>();
                }
                if (model.WorkingDay == null)
                {
                    model.WorkingDay = new List<int>();
                }
                ViewBag.AddDoctorStatus = DoctorModel.InsertDoctor(model);
                List<SelectListItem> genderSelectListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "1", Text = "Nam"},
                                                                    new SelectListItem {Value = "2", Text = "Nữ"}
                                                                };
                ViewBag.GenderTypeList = new SelectList(genderSelectListItem, "Value", "Text", 1);
                List<SelectListItem> daySelectListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "2", Text = "Thứ Hai"},
                                                                    new SelectListItem {Value = "3", Text = "Thứ Ba"},
                                                                    new SelectListItem {Value = "4", Text = "Thứ Tư"},
                                                                    new SelectListItem {Value = "5", Text = "Thứ Năm"},
                                                                    new SelectListItem {Value = "6", Text = "Thứ Sáu"},
                                                                    new SelectListItem {Value = "7", Text = "Thứ Bảy"},
                                                                    new SelectListItem {Value = "8", Text = "Chủ Nhật"},
                                                                };
                ViewBag.DayTypeList = new SelectList(daySelectListItem, "Value", "Text");
                //Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
                if ((bool)ViewBag.AddDoctorStatus)
                {
                    ModelState.Clear();
                    return View();
                }
                return View(model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [HttpGet]
        [LayoutInjecter(Constants.HospitalUserLayout)]
        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public async Task<ActionResult> UpdateDoctor(int doctorId = 0)
        {
            try
            {
                DoctorModel model = new DoctorModel();
                int hospitalId = 0;

                string email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
                model.UploadedPerson = Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);
                hospitalId = (int)AccountModel.LoadUserByEmail(email).Hospital_ID;
                HospitalEntity hospital = await HomeModel.LoadHospitalById(hospitalId);
                model.HospitalID = hospital.Hospital_ID;
                model.HospitalName = hospital.Hospital_Name;

                DoctorEntity doctor = DoctorModel.LoadDoctorById(doctorId, hospitalId);
                if (doctor != null)
                {
                    model.DoctorID = doctor.Doctor_ID;
                    model.FirstName = doctor.First_Name;
                    model.LastName = doctor.Last_Name;
                    model.Gender = doctor.Gender == true ? 1 : 2;
                    model.Degree = doctor.Degree;
                    model.Experience = doctor.Experience;
                    model.PhotoFilePath = doctor.Photo != null ? doctor.Photo.File_Path : string.Empty;
                    List<int> selectedSpecilityList = new List<int>();
                    foreach (Speciality specility in doctor.Specialities)
                    {
                        selectedSpecilityList.Add(specility.Speciality_ID);
                    }
                    model.SpecialityList = selectedSpecilityList;
                    if (doctor.Working_Day != null)
                    {
                        model.WorkingDay = doctor.Working_Day.Split(',').Select(n => Convert.ToInt32(n)).ToList<int>();
                    }
                    else
                    {
                        model.WorkingDay = new List<int>();
                    }
                }
                else
                {
                    model.SpecialityList = new List<int>();
                    model.WorkingDay = new List<int>();
                }
                List<SelectListItem> genderSelectListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "1", Text = "Nam"},
                                                                    new SelectListItem {Value = "2", Text = "Nữ"}
                                                                };
                ViewBag.GenderTypeList = new SelectList(genderSelectListItem, "Value", "Text", model.Gender);
                List<SelectListItem> daySelectListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "2", Text = "Thứ Hai"},
                                                                    new SelectListItem {Value = "3", Text = "Thứ Ba"},
                                                                    new SelectListItem {Value = "4", Text = "Thứ Tư"},
                                                                    new SelectListItem {Value = "5", Text = "Thứ Năm"},
                                                                    new SelectListItem {Value = "6", Text = "Thứ Sáu"},
                                                                    new SelectListItem {Value = "7", Text = "Thứ Bảy"},
                                                                    new SelectListItem {Value = "8", Text = "Chủ Nhật"},
                                                                };
                ViewBag.DayTypeList = daySelectListItem;
                //Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = specialityList;
                return View(model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [HttpPost]
        [LayoutInjecter(Constants.HospitalUserLayout)]
        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public async Task<ActionResult> UpdateDoctor(DoctorModel model)
        {
            try
            {
                // Upload Photo
                HttpPostedFileBase file = Request.Files["file"];
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        if (file.ContentType.ToLower() == "image/jpg" ||
                            file.ContentType.ToLower() == "image/jpeg" ||
                            file.ContentType.ToLower() == "image/pjpeg" ||
                            file.ContentType.ToLower() == "image/gif" ||
                            file.ContentType.ToLower() == "image/x-png" ||
                            file.ContentType.ToLower() == "image/png")
                        {
                            string filePath = (FileUtil.SaveFileToServer(file, Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]), 1));
                            model.PhotoFilePath = filePath;
                        }
                    }
                }

                int hospitalId = 0;

                string email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
                hospitalId = (int)AccountModel.LoadUserByEmail(email).Hospital_ID;
                HospitalEntity hospital = await HomeModel.LoadHospitalById(hospitalId);
                model.HospitalID = hospital.Hospital_ID;
                model.HospitalName = hospital.Hospital_Name;

                List<SelectListItem> genderSelectListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "1", Text = "Nam"},
                                                                    new SelectListItem {Value = "2", Text = "Nữ"}
                                                                };
                ViewBag.GenderTypeList = new SelectList(genderSelectListItem, "Value", "Text", model.Gender);
                List<SelectListItem> daySelectListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "2", Text = "Thứ Hai"},
                                                                    new SelectListItem {Value = "3", Text = "Thứ Ba"},
                                                                    new SelectListItem {Value = "4", Text = "Thứ Tư"},
                                                                    new SelectListItem {Value = "5", Text = "Thứ Năm"},
                                                                    new SelectListItem {Value = "6", Text = "Thứ Sáu"},
                                                                    new SelectListItem {Value = "7", Text = "Thứ Bảy"},
                                                                    new SelectListItem {Value = "8", Text = "Chủ Nhật"},
                                                                };
                ViewBag.DayTypeList = daySelectListItem;
                //Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = specialityList;

                if (model.FirstName != null)
                {
                    model.FirstName.Trim();
                }
                if (model.LastName != null)
                {
                    model.LastName.Trim();
                }
                if (model.Degree != null)
                {
                    model.Degree.Trim();
                }
                if (model.Experience != null)
                {
                    model.Experience.Trim();
                }
                if (model.SpecialityList == null)
                {
                    model.SpecialityList = new List<int>();
                }
                if (model.WorkingDay == null)
                {
                    model.WorkingDay = new List<int>();
                }
                ViewBag.UpdateDoctorStatus = DoctorModel.UpdateDoctor(model);

                return View(model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [LayoutInjecter(Constants.HospitalUserLayout)]
        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public ActionResult Feedback(string sFromDate, string sToDate, int feedbackType = 0, int responseType = 0, int page = 1)
        {
            try
            {
                int hospitalId = 0;

                string email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
                hospitalId = (int)AccountModel.LoadUserByEmail(email).Hospital_ID;

                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                if (string.IsNullOrEmpty(sFromDate) || string.IsNullOrEmpty(sToDate))
                {
                    DateTime today = DateTime.Today;
                    fromDate = new DateTime(today.Year, today.Month - 1, 1);
                    toDate = new DateTime(today.Year, today.Month, 1);
                }
                else
                {
                    fromDate = DateTime.ParseExact(sFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(sToDate + " 23:59:59", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }

                ViewBag.FeedbackList = FeedBackModel.LoadHospitalUserFeedback(fromDate, toDate, feedbackType, responseType, hospitalId).ToPagedList(page, Constants.PageSize);

                ViewBag.FromDate = string.Format("{0:dd/MM/yyyy}", fromDate);
                ViewBag.ToDate = string.Format("{0:dd/MM/yyyy}", toDate);

                List<FeedbackType> feebackTypeList = FeedBackModel.LoadFeedbeackTypeList();
                List<SelectListItem> feedbackTypeItemList = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "0", Text = "Tất cả loại phản hồi"},
                                                                    new SelectListItem {Value = feebackTypeList[0].Type_ID.ToString(), Text = feebackTypeList[0].Type_Name},
                                                                    new SelectListItem {Value = feebackTypeList[1].Type_ID.ToString(), Text = feebackTypeList[1].Type_Name},
                                                                    new SelectListItem {Value = feebackTypeList[2].Type_ID.ToString(), Text = feebackTypeList[2].Type_Name},
                                                                };
                ViewBag.FeedbackTypeList = new SelectList(feedbackTypeItemList, "Value", "Text", feedbackType);
                ViewBag.FeedbackType = feedbackType;

                List<SelectListItem> responseItemList = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "0", Text = "Tất cả phản hồi"},
                                                                    new SelectListItem {Value = "1", Text = "Đã duyệt"},
                                                                    new SelectListItem {Value = "2", Text = "Chưa duyệt"}
                                                                };
                ViewBag.ResponseTypeList = new SelectList(responseItemList, "Value", "Text", responseType);
                ViewBag.ResponseType = responseType;
                ViewBag.ApproveStatus = TempData["ApproveStatus"];
                return View();
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public ActionResult ApproveFeedback(int feedbackId = 0)
        {
            int hospitalId = 0;

            string email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
            hospitalId = (int)AccountModel.LoadUserByEmail(email).Hospital_ID;

            TempData["ApproveStatus"] = FeedBackModel.ApproveHospitalUserFeedback(feedbackId, hospitalId);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [LayoutInjecter(Constants.HospitalUserLayout)]
        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public ActionResult SearchQueryStatistic(string sFromDate, string sToDate, int page = 1)
        {
            try
            {
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                if (string.IsNullOrEmpty(sFromDate) || string.IsNullOrEmpty(sToDate))
                {
                    DateTime today = DateTime.Today;
                    fromDate = new DateTime(today.Year, today.Month - 1, 1);
                    toDate = new DateTime(today.Year, today.Month, 1);
                }
                else
                {
                    fromDate = DateTime.ParseExact(sFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(sToDate + " 23:59:59", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                ViewBag.SearchQueryStatistic = DataModel.SearchQueryStatistic(fromDate, toDate).ToPagedList(page, Constants.PageSize + 5);
                ViewBag.FromDate = string.Format("{0:dd/MM/yyyy}", fromDate);
                ViewBag.ToDate = string.Format("{0:dd/MM/yyyy}", toDate);
                return View();
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [Authorize(Roles = Constants.HospitalUserRoleName)]
        public ActionResult DeactivateDoctor(int doctorId)
        {
            int hospitalId = 0;

            string email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
            hospitalId = (int)AccountModel.LoadUserByEmail(email).Hospital_ID;

            TempData["DeactivateStatus"] = DoctorModel.DeactivateDoctor(doctorId, hospitalId);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
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
                #region Prepare data

                int result = 0;
                model.CreatedPerson = Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);

                // Check if cooridnate is null
                if (string.IsNullOrEmpty(model.Coordinate))
                {
                    WebClient client = new WebClient();
                    string geoJsonResult = string.Empty;
                    geoJsonResult = client.DownloadString(string.Concat(
                                    Constants.GeoCodeJsonQuery, model.FullAddress));
                    JObject geoJsonObject = JObject.Parse(geoJsonResult);
                    if (Constants.Ok.Equals(geoJsonObject.Value<string>(Constants.GeoCodeStatus)))
                    {
                        model.Coordinate = string.Format("{0}, {1}",
                            geoJsonObject[Constants.GeoCodeResults].
                                First[Constants.GeoCodeGemometry][Constants.GeoCodeLocation].
                                Value<double>(Constants.GeoCodeLatitude),
                            geoJsonObject[Constants.GeoCodeResults].
                                First[Constants.GeoCodeGemometry][Constants.GeoCodeLocation].
                                Value<double>(Constants.GeoCodeLongitude));
                    }
                }

                #endregion

                #region Handle photo

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

                #endregion

                #region Insert hospital and return to view

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

        #region Update Hospital

        /// <summary>
        /// Update hospital information
        /// </summary>
        /// <param name="hospitalId">Hospital ID</param>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName + Constants.Comma + Constants.HospitalUserRoleName)]
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

        /// <summary>
        /// POST: /Hospital/UpdateHospital
        /// </summary>
        /// <param name="model">Hospital Model</param>
        /// <param name="files">Photo files</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpPost]
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName + Constants.Comma + Constants.HospitalUserRoleName)]
        [ValidateInput(false)]
        public async Task<ActionResult> UpdateHospital(HospitalModel model, List<HttpPostedFileBase> file)
        {
            try
            {
                // Prepare data
                int result = 0;
                string updatedContent = string.Empty;
                model.CreatedPerson = Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.UpdateHospitalAsync(model);
                }

                #region cascading dropdownlist

                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
                ViewBag.WardList = new SelectList(wardList, Constants.WardID, Constants.WardName);
                ViewBag.HospitalTypeList = new SelectList(hospitalTypeList, Constants.TypeID, Constants.TypeName);

                //Load list of specialities
                model.SpecialityList = await SpecialityUtil.LoadSpecialityAsync();

                //Load list of services
                serviceList = await ServiceFacilityUtil.LoadServiceAsync();
                ViewBag.ServiceList = serviceList;

                // Load list of facilitites
                facilityList = await ServiceFacilityUtil.LoadFacilityAsync();
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

                #region Button Upload

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
                            if (record.RecordStatus == 1)
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
                                else
                                {
                                    record.Coordinate = string.Format("{0}, {1}", Constants.hcmLatitude, Constants.hcmLongitude);
                                }
                            }
                        }

                        // Return list of hospital to view
                        // Add to session and return
                        Session.Add(Constants.HospitalExcelSession, hospitalList);
                        return View(hospitalList);
                    }
                }

                #endregion

                #region Button Confirm

                // Add hospital list to data
                if (Constants.ButtonConfirm.Equals(button))
                {
                    if (model != null)
                    {
                        // Create a list to determine which hospital is
                        List<HospitalModel> resultList = new List<HospitalModel>();

                        foreach (HospitalModel record in model)
                        {
                            if (record.RecordStatus == 1)
                            {
                                record.CreatedPerson =
                                    Int32.Parse(User.Identity.Name.Split(Char.Parse(Constants.Minus))[2]);
                                await record.InsertHospitalAsync(record, 0);
                            }
                        }
                    }

                    ViewBag.AddStatus = 1;
                    Session.Add(Constants.HospitalExcelSession, null);
                    return View(new List<HospitalModel>());
                }

                #endregion
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            return View();
        }

        #endregion

        #region Review Hospital Detail

        /// <summary>
        /// Load paritial view Review Hospital Detail
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult ReviewHospitalDetail(int hospitalId)
        {
            List<HospitalModel> hospitalList = (List<HospitalModel>)Session[Constants.HospitalExcelSession];
            foreach (HospitalModel hospital in hospitalList)
            {
                if (hospital.HospitalID == hospitalId)
                {
                    return PartialView(Constants.ReviewHospitalDetailAction, hospital);
                }
            }
            return PartialView(Constants.ReviewHospitalDetailAction);
        }

        #endregion

        #endregion
    }
}
