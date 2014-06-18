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
        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.HomeLayout)]
        public async Task<ActionResult> Index()
        {
            //try
            //{
                // Load list of city
                List<CityEntity> cityList = await LocationUtil.LoadCityAsync();
                ViewBag.CityList = new SelectList(cityList, "CityID", "CityName");
            //}
            //catch (Exception)
            //{
            //    // Move to error page
            //    return RedirectToAction(Constants.HomeErrorPage, Constants.ErrorController);
            //}

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
            // Add the list of Cities and Specialities to view


            // Check if all validations are correct
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    await model.GIRQueryAnalyzerAsync(model.SearchValue);
                    List<HospitalEntity> list = await model.SearchHospital();
                    TempData["list"] = list;
                }
                catch(Exception)
                {
                    Response.Write(ErrorMessage.SEM001);
                }

                // Move to result page
                return RedirectToAction("Result", Constants.HomeController);
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
