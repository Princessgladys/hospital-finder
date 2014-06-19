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
        /// POST: /Home/GetDistrictByCity
        /// </summary>
        /// <param name="cityId">City ID</param>
        /// <returns>Task[ActionResult] with JSON contains list of Districts</returns>
        public async Task<ActionResult> GetDistrictByCity(string cityId)
        {
            try
            {
                // Check if city ID is null or not
                if (!String.IsNullOrEmpty(cityId))
                {
                    districtList = await LocationUtil.LoadDistrictInCityAsync(Int16.Parse(cityId));
                    return Json(districtList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                // Move to error page
                return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
            }

            // Return default value
            districtList = new List<DistrictEntity>() { new DistrictEntity { DistrictName = Constants.RequireDistrict}};
            return Json(districtList, JsonRequestBehavior.AllowGet);
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
                catch(Exception)
                {
                    Response.Write(ErrorMessage.SEM001);
                }

                // Move to result page
                return RedirectToAction(Constants.SearchResultMethod, Constants.HomeController);
            }
        }

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
    }
}
