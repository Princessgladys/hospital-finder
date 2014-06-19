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
        public static List<CityEntity> cityList = null;
        public static List<DistrictEntity> districtList = null;
        public static List<SpecialityEntity> specialityList = null;
        public static List<DiseaseEntity> diseaseList = null;

        #region Load District and Disease drop down list

        /// <summary>
        /// POST: /Home/GetDistrictByCity
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
                    districtList = new List<DistrictEntity>() {
                        new DistrictEntity{ DistrictName = Constants.RequireDistrict } };
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
        /// POST: /Home/GetDeseaseBySpeciality
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
                    diseaseList = new List<DiseaseEntity>() {
                        new DiseaseEntity{ DiseaseName = Constants.RequireDisease } };
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
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
                districtList = new List<DistrictEntity>();
                ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
                // Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
                // Load list of disease
                diseaseList = new List<DiseaseEntity>();
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
        /// POST: /Home/Index
        /// </summary>
        /// <param name="model">HomeModels</param>
        /// <returns>Task[ActionResult]</returns>
        [HttpPost]
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> Index(HomeModels model)
        {
            // Add the values of drop down lists to view
            ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
            ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);
            ViewBag.DistrictList = new SelectList(districtList, Constants.DistrictID, Constants.DistrictName);
            ViewBag.DiseaseList = new SelectList(diseaseList, Constants.DiseaseID, Constants.DiseaseName);

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
                    List<HospitalEntity> list = await model.SearchHospital();
                    TempData["list"] = list;
                }
                catch (Exception)
                {
                    Response.Write(ErrorMessage.SEM001);
                }

                // Move to result page
                return RedirectToAction(Constants.SearchResultMethod, Constants.HomeController);
            }
        }

        #endregion 

        #region Display search results

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> SearchResult()
        {
            try
            {
                List<HospitalEntity> list = (List<HospitalEntity>)TempData["list"];
                return View(list);
            }
            catch (Exception)
            {
                Response.Write(ErrorMessage.SEM001);
                return View();
            }
        }

        #endregion
    }
}
