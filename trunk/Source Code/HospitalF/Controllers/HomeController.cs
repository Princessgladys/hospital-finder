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
                                      name = d.District_Name
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
            List<Hospital> hospitalList = null;

            try
            {
                // Load hospital types from database
                List<HospitalType> hospitalTypeList = null;
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    hospitalTypeList = await Task.Run(() => (from ht in data.HospitalTypes
                                                             select ht).ToList());
                }
                ViewBag.HospitalTypes = new SelectList(hospitalTypeList, Constants.HospitalTypeID, Constants.HospitalTypeName);

                // Indicate which button is clicked
                var button = Request[Constants.Button];

                // Normal search form
                if ((button == null) || Constants.NormalSearchForm.Equals(button))
                {
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
                }

                // Advanced search form
                if (Constants.AdvancedSearchForm.Equals(button))
                {
                    // Search hospitals
                    hospitalList = await model.AdvancedSearchHospital(model.CityID, model.DistrictID,
                        model.SpecialityID, model.DiseaseName);
                }

                // Location search form
                if (Constants.LocationSearchForm.Equals(button))
                {
                    // Search hospitals
                    double lat = 0;
                    double lng = 0;
                    if ("1".Equals(Request["LocationType"]))
                    {
                        if (model.Coordinate != null)
                        {
                            if (model.Coordinate.Split(',').Length > 1)
                            {
                                double.TryParse(model.Coordinate.Split(',')[0], out lat);
                                double.TryParse(model.Coordinate.Split(',')[1], out lng);
                            }
                        }
                    }
                    else if ("2".Equals(Request["LocationType"]))
                    {
                        if (!string.IsNullOrEmpty(model.Position))
                        {
                            WebClient client = new WebClient();
                            string jsonResult = client.DownloadString(string.Concat("http://maps.googleapis.com/maps/api/geocode/json?address=", model.Position));
                            // Json.Net is really helpful if you have to deal
                            // with Json from .Net http://json.codeplex.com/
                            JObject jsonGeoInfo = JObject.Parse(jsonResult);
                            lat = jsonGeoInfo["results"].First["geometry"]["location"].Value<double>("lat");
                            lng = jsonGeoInfo["results"].First["geometry"]["location"].Value<double>("lng");
                        }

                    }
                    hospitalList = await model.LocationSearchHospital(lat, lng, model.Radius * 1000);
                    ViewBag.Position = lat + ", " + lng;
                    ViewBag.Radius = model.Radius * 1000;                
                }

                // Transfer list of hospitals to Search Result page
                ViewBag.HospitalList = hospitalList.ToPagedList(page, Constants.PageSize);
                ViewBag.JsonHospitalList = JsonConvert.SerializeObject(hospitalList.ToPagedList(page, Constants.PageSize));

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
            return View();
        }

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <param name="model">HomeModels</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpGet]
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> FilterResult(HomeModels model, FormCollection form)
        {
            try
            {

            }
            catch (Exception)
            {
                Response.Write(ErrorMessage.SEM001);
            }
            return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
        }

        #endregion
    }
}
