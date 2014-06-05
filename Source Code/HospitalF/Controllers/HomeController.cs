using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.Entities;

namespace HospitalF.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                // Load the list of Cities and Specialities


                // Add the list of Cities and Specialities to view


            }
            catch (Exception)
            {
                Response.Write(ErrorMessage.SEM001);
                return View();
            }

            return View();
        }

        /// <summary>
        /// POST: /Home/Index
        /// </summary>
        /// <param name="model">HomeModels</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
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
                
                    await model.GIRQueryAnalyzerAsync(model.SearchValue);
                                

                // Move to result page
                return RedirectToAction(string.Empty, Constants.HomeController);
            }
        }
    }
}
