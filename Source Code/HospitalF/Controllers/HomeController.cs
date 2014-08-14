using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.Entities;
using HospitalF.App_Start;
using HospitalF.Utilities;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using PagedList;
using System.Collections.Specialized;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace HospitalF.Controllers
{
    public class HomeController : SecurityBaseController
    {
        // Declare public list items for Drop down lists
        public static List<City> cityList = null;
        public static List<District> districtList = null;
        public static List<Speciality> specialityList = null;
        public static List<Disease> diseaseList = null;

        #region AJAX calling methods

        /// <summary>
        /// GET: /Home/GetDistrictByCity
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
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Home/GetDeseaseBySpeciality
        /// </summary>
        /// <param name="specialityId">SpecialityId ID</param>
        /// <returns>Task[ActionResult] with JSON contains list of Deseases</returns>
        public async Task<ActionResult> GetDeseaseBySpeciality(string specialityId)
        {
            try
            {
                int tempSpecialityId = 0;
                // Check if city ID is null or not
                if (!String.IsNullOrEmpty(specialityId) && Int32.TryParse(specialityId, out tempSpecialityId))
                {
                    diseaseList = await SpecialityUtil.LoadDiseaseInSpecialityAsync(tempSpecialityId);
                    var result = (from d in diseaseList
                                  select new
                                  {
                                      name = d.Disease_Name
                                  });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Return default value
                    diseaseList = new List<Disease>();
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Home/GetDeseaseBySpeciality
        /// </summary>
        /// <returns>ask[ActionResult] with JSON contains list of Setences</returns>
        public async Task<ActionResult> LoadSuggestSentence()
        {
            try
            {
                // Return list of sentences
                List<string> sentenceDic = await DictionaryUtil.LoadSuggestSentenceAsync();
                return Json(sentenceDic, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        #region Search hospital

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> Index()
        {
            try
            {
                // Load list of cities
                cityList = await LocationUtil.LoadCityAsync();
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
                // Load list of districts
                districtList = new List<District>();
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
                // Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
                // Load list of disease
                diseaseList = new List<Disease>();
                ViewBag.DiseaseList = new SelectList(diseaseList, Constants.DiseaseID, Constants.DiseaseName);

                List<SelectListItem> locationTypeListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "2", Text = "Nhập vị trí"},
                                                                    new SelectListItem {Value = "1", Text = "Vị trí hiện tại", }
                                                                };
                ViewBag.LocationTypeList = new SelectList(locationTypeListItem, "Value", "Text", 2);
                List<SelectListItem> radiusListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "0.3", Text = "300 mét"},
                                                                    new SelectListItem {Value = "0.5", Text = "500 mét"},
                                                                    new SelectListItem {Value = "1", Text = "1 km"},
                                                                    new SelectListItem {Value = "3", Text = "3 km"},
                                                                    new SelectListItem {Value = "5", Text = "5 km"},
                                                                    new SelectListItem {Value = "10", Text = "10 km"},
                                                                    new SelectListItem {Value = "15", Text = "15 km"},
                                                                    new SelectListItem {Value = "20", Text = "20 km"}
                                                                };
                ViewBag.RadiusList = new SelectList(radiusListItem, "Value", "Text", 0.3);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            return View();
        }

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <param name="model">HomeModels</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpGet]
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> SearchResult(HomeModels model, int page = 1)
        {
            List<HospitalEntity> hospitalList = null;
            IPagedList<HospitalEntity> pagedHospitalList = null;
            try
            {
                // Indicate which button is clicked
                string button = Request[Constants.Button];

                // Normal search form
                if ((string.IsNullOrEmpty(button)) || Constants.NormalSearchForm.Equals(button))
                {
                    ViewBag.SearchValue = model.SearchValue;
                    // Check if input search query is null or empty
                    if (!string.IsNullOrEmpty(model.SearchValue))
                    {
                        // Check if input search value is understandable
                        string[] suggestSentence = StringUtil.CheckVocabulary(model.SearchValue);
                        if (Constants.False.Equals(suggestSentence[0]))
                        {
                            ViewBag.SuggestionSentence = suggestSentence[1];
                        }
                        // Analyze to GIR query
                        await model.GIRQueryAnalyzerAsync(model.SearchValue);
                    }

                    // Search hospitals
                    hospitalList = await model.NormalSearchHospital();
                    pagedHospitalList = hospitalList.ToPagedList(page, Constants.PageSize);
                    // Search Query Statistic
                    DataModel.StoreSearchQuery(model.SearchValue, hospitalList.Count);
                }

                // Advanced search form
                if (Constants.AdvancedSearchForm.Equals(button))
                {
                    // Load list of cities
                    cityList = await LocationUtil.LoadCityAsync();
                    ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
                    // Load list of districts
                    districtList = new List<District>();
                    ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
                    // Load list of specialities
                    specialityList = await SpecialityUtil.LoadSpecialityAsync();
                    ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
                    // Load list of disease
                    diseaseList = new List<Disease>();
                    ViewBag.DiseaseList = new SelectList(diseaseList, Constants.DiseaseID, Constants.DiseaseName);

                    ViewBag.DiseaseName = model.DiseaseName;
                    hospitalList = await model.AdvancedSearchHospital(model.CityID, model.DistrictID,
                        model.SpecialityID, model.DiseaseName);
                    pagedHospitalList = hospitalList.ToPagedList(page, Constants.PageSize);

                    ViewBag.SearchType = Constants.AdvancedSearchForm;
                }

                // Location search form
                if (Constants.LocationSearchForm.Equals(button))
                {
                    ViewBag.SearchType = Constants.LocationSearchForm;
                    List<SelectListItem> locationTypeListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "2", Text = "Nhập vị trí"},
                                                                    new SelectListItem {Value = "1", Text = "Vị trí hiện tại", }
                                                                };
                    ViewBag.LocationTypeList = new SelectList(locationTypeListItem, "Value", "Text", 2);
                    List<SelectListItem> radiusListItem = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "0.3", Text = "300 mét"},
                                                                    new SelectListItem {Value = "0.5", Text = "500 mét"},
                                                                    new SelectListItem {Value = "1", Text = "1 km"},
                                                                    new SelectListItem {Value = "3", Text = "3 km"},
                                                                    new SelectListItem {Value = "5", Text = "5 km"},
                                                                    new SelectListItem {Value = "10", Text = "10 km"},
                                                                    new SelectListItem {Value = "15", Text = "15 km"},
                                                                    new SelectListItem {Value = "20", Text = "20 km"}
                                                                };
                    ViewBag.RadiusList = new SelectList(radiusListItem, "Value", "Text", 0.3);

                    // Search hospitals
                    double lat = 0;
                    double lng = 0;
                    WebClient client = new WebClient();
                    string coordinate = model.Coordinate;
                    string position = model.Position;
                    double radius = model.Radius;
                    if (!(0 < radius && radius <= 20))
                    {
                        radius = 10;
                    }

                    if (model.LocationType == 1)
                    {
                        if (coordinate != null)
                        {
                            if (coordinate.Split(',').Length > 1)
                            {
                                double.TryParse(coordinate.Split(',')[0], out lat);
                                double.TryParse(coordinate.Split(',')[1], out lng);
                            }
                        }
                    }
                    else if (model.LocationType == 2)
                    {

                        if (!string.IsNullOrEmpty(position))
                        {
                            string geoJsonResult = client.DownloadString(string.Concat("http://maps.googleapis.com/maps/api/geocode/json?address=", position));
                            // Json.Net is really helpful if you have to deal
                            // with Json from .Net http://json.codeplex.com/
                            JObject geoJsonObject = JObject.Parse(geoJsonResult);
                            if (geoJsonObject.Value<string>("status").Equals("OK"))
                            {
                                lat = geoJsonObject["results"].First["geometry"]["location"].Value<double>("lat");
                                lng = geoJsonObject["results"].First["geometry"]["location"].Value<double>("lng");
                            }
                        }

                    }

                    hospitalList = await HomeModels.LocationSearchHospital(lat, lng, radius * 1000);
                    pagedHospitalList = hospitalList.ToPagedList(page, Constants.PageSize);
                    string distanceMatrixUrl = string.Concat("http://maps.googleapis.com/maps/api/distancematrix/json?origins=", lat, ",", lng, "&destinations=");
                    int index = 0;
                    foreach (HospitalEntity hospital in pagedHospitalList)
                    {
                        distanceMatrixUrl += (index == 0 ? string.Empty : "|") + hospital.Coordinate.Split(',')[0].Trim() + "," + hospital.Coordinate.Split(',')[1].Trim();
                        index = -1;
                    }
                    string dMatrixJsonResult = client.DownloadString(distanceMatrixUrl);
                    JObject dMatrixJsonObject = JObject.Parse(dMatrixJsonResult);
                    if (dMatrixJsonObject.Value<string>("status").Equals("OK"))
                    {
                        index = 0;
                        foreach (HospitalEntity hospital in pagedHospitalList)
                        {
                            hospital.Distance = dMatrixJsonObject["rows"].First["elements"].ElementAt(index++)["distance"].Value<double>("value");
                        }

                        model.Coordinate = lat + ", " + lng;
                    }

                }

                // Transfer list of hospitals to Search Result page

                ViewBag.HospitalList = pagedHospitalList;
                ViewBag.JsonHospitalList = JsonConvert.SerializeObject(pagedHospitalList);

                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove("page");
                ViewBag.Query = queryString.ToString();

                if (hospitalList.Count == 0)
                {
                    ViewBag.SearchValue = model.SearchValue;
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            // Move to result page
            return View(model);
        }

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <param name="model">HomeModels</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpGet]
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> FilterResult(int searchType = 2, int page = 1)
        {
            try
            {
                if (searchType == 1)
                {
                }
                else if (searchType == 2)
                {
                    List<HospitalEntity> hospitalList = await HomeModels.LocationSearchHospital(10.8525022, 106.6226, 16000);
                    List<HospitalEntity> filteredHospitalList = (from h in hospitalList
                                                                 orderby h.Rating descending
                                                                 select h).ToList<HospitalEntity>();
                    IPagedList<HospitalEntity> pagedHospitalList = filteredHospitalList.ToPagedList(page, Constants.PageSize);
                    ViewBag.HospitalList = pagedHospitalList;
                    ViewBag.JsonHospitalList = JsonConvert.SerializeObject(pagedHospitalList);
                }
                else if (searchType == 3)
                {
                }
                return View(Constants.SearchResultAction);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> Hospital(int hospitalId = 0)
        {
            try
            {
                HospitalEntity hospital = null;
                if (hospitalId > 0)
                {
                    hospital = await HomeModels.LoadHospitalById(hospitalId);
                    if (hospital != null)
                    {
                        hospital.Services = HomeModels.LoadServicesByHospitalId(hospitalId);
                        hospital.Facilities = HomeModels.LoadFacillitiesByHospitalId(hospitalId);
                        hospital.Specialities = HomeModels.LoadSpecialitiesByHospitalId(hospitalId);
                        ViewBag.SpecialityList = new SelectList(hospital.Specialities, Constants.SpecialityID, Constants.SpecialityName);
                        ViewBag.RateActionStatus = TempData["RateActionStatus"];
                        ViewBag.RateActionMessage = TempData["RateActionMessage"];
                        ViewBag.HospitalEntity = hospital;
                        ViewBag.Photos = HomeModels.LoadPhotosByHospitalId(hospitalId);
                    }
                    else
                    {
                        return RedirectToAction(Constants.IndexAction, Constants.HomeController);
                    }
                }
                else
                {
                    return RedirectToAction(Constants.IndexAction, Constants.HomeController);
                }

            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
            return View();
        }

        [HttpPost]
        public ActionResult RateHospital(int id = 0, int score = 0)
        {
            try
            {
                if (Session["RATING_TIME"] == null)
                {
                    Session["RATING_TIME"] = 0;
                }

                int ratingTime = (int)Session["RATING_TIME"];
                Session["RATING_TIME"] = ++ratingTime;

                if (ratingTime > 3)
                {
                    RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();

                    if (String.IsNullOrEmpty(recaptchaHelper.Response))
                    {
                        TempData["RateActionStatus"] = false;
                        TempData["RateActionMessage"] = "Vui lòng nhập mã bảo mật bên dưới.";

                        return RedirectToAction(Constants.HospitalAction, Constants.HomeController, new { hospitalId = id, redirect = "yes" });
                    }

                    RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();

                    if (recaptchaResult != RecaptchaVerificationResult.Success)
                    {
                        TempData["RateActionStatus"] = false;
                        TempData["RateActionMessage"] = "Vui lòng nhập lại mã bảo mật bên dưới.";

                        return RedirectToAction(Constants.HospitalAction, Constants.HomeController, new { hospitalId = id, redirect = "yes" });
                    }
                }

                string email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];

                int userId = AccountModels.LoadUserIdByEmail(email);

                bool check = HomeModels.RateHospital(userId, id, score);
                if (!check)
                {
                    TempData["RateActionStatus"] = false;
                    TempData["RateActionMessage"] = "Vui lòng thử lại sau ít phút.";
                }
                TempData["RateActionStatus"] = true;
                return RedirectToAction(Constants.HospitalAction, Constants.HomeController, new { hospitalId = id, redirect = "yes" });

            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        public async Task<ActionResult> FeedBack(int hospitalID)
        {
            string email = string.Empty;
            FeedBackModels model = new FeedBackModels();
            Hospital hospital;
            List<FeedbackType> feebackType = FeedBackModels.LoadFeedbackType();
            ViewBag.FeedbackTypeList = new SelectList(feebackType, Constants.FeedbackType, Constants.FeedbackTypeName);
            hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalID);
            model.HospitalName = hospital.Hospital_Name;
            model.HospitalID = hospitalID;
            // login user is normal user
            if (User.IsInRole("2"))
            {
                model.Email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
            }
            else if (User.IsInRole("3"))
            {
                model.Email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
            }
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> FeedBack(FeedBackModels model)
        {
            try
            {
                int result = 0;

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.InsertFeedbackAsync(model);
                }

                // Check returned result
                if (result == 1)
                {

                    ViewBag.AddFeedbackStatus = 1.ToString();
                    return RedirectToAction(Constants.HospitalAction, Constants.HomeController, new { hospitalId = model.HospitalID, redirect = "yes" });
                }
                else
                {
                    ViewBag.AddFeedbackStatus = 0.ToString();
                    ModelState.Clear();
                    return View();
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #region search doctor
        public async Task<ActionResult> SearchDoctor(string SpecialityID, string DoctorName, string HospitalID)
        {
            try
            {
                int tempSpecialityID, tempHospitalID;
               List<Doctor> doctorList = new List<Doctor>();
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
                return PartialView("SearchDoctor");
            }
            catch (Exception ex)
            {
                LoggingUtil.LogException(ex);
                return RedirectToAction(Constants.SystemFailureHospitalUserAction, Constants.ErrorController);
            }

        }
        #endregion
    }
}
