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
    public class HomeController : Controller
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
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Return default value
                    districtList = new List<District>() {
                        new District{ District_Name = Constants.RequireDistrict } };
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                // Move to error page
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
                    return Json(diseaseList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Return default value
                    diseaseList = new List<Disease>() {
                        new Disease{ Disease_Name = Constants.RequireDisease } };
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                // Move to error page
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
            catch (Exception)
            {
                // Move to error page
                return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
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
            catch (Exception)
            {
                // Move to error page
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
        public async Task<ActionResult> SearchResult(HomeModels model)
        {
            // Add the values of drop down lists to view
            //ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
            //ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
            //ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
            //ViewBag.DiseaseList = new SelectList(diseaseList, Constants.DiseaseID, Constants.DiseaseName);

            List<Hospital> list = null;
            // Check if all validations are correct
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    // Analyze input search query using GIR algorithm and search
                    await model.GIRQueryAnalyzerAsync(model.SearchValue);
                    list = await model.SearchHospital();
                }
                catch (Exception)
                {
                    Response.Write(ErrorMessage.SEM001);
                }

                // Move to result page
                return View(list);
            }
        }

        #endregion
    }
}
