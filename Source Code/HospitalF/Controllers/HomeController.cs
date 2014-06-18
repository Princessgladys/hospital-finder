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
        public static List<CityEntity> cityList = new List<CityEntity>();

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> Index()
        {
            try
            {
                // Load list of city
                cityList = await LocationUtil.LoadCityAsync();
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
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
            // Check if all validations are correct
            if (!ModelState.IsValid)
            {
                // Add the list of Cities and Specialities to view
                ViewBag.CityList = new SelectList(cityList, Constants.CityID, Constants.CityName);
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
