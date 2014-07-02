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
                return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
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
                return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
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
                return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
            }
        }

        #endregion

        #region Search hospital

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [Authorize(Roles = "Administrator, User")]
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
                return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
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
        public async Task<ActionResult> SearchResult(HomeModels model, FormCollection form)
        {
            List<Hospital> hospitalList = null;
            // Check if all validations are correct
            if (!ModelState.IsValid)
            {
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
                ViewBag.DiseaseList = new SelectList(diseaseList, Constants.DiseaseID, Constants.DiseaseName);
                return View(model);
            }
            else
            {
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
                    // Analyze input search query using GIR algorithm and search
                    await model.GIRQueryAnalyzerAsync(model.SearchValue);
                    //list = await model.SearchHospital();
                    if (!string.IsNullOrEmpty(model.SearchValue))
                    {
                        await model.GIRQueryAnalyzerAsync(model.SearchValue);
                    }
                    else
                    {
                        await model.GIRQueryAnalyzerAsync(form["SearchValue"]);
                    }
                    //hospitalList = await model.SearchHospital();
                }
                catch (Exception exception)
                {
                    LoggingUtil.LogException(exception);
                    Response.Write(ErrorMessage.SEM001);
                }

                // Move to result page
                return View(hospitalList);
            }
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
            return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
        }

        #endregion
    }
}
