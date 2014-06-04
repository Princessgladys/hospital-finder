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
        public async Task<ActionResult> Index(HomeModels model)
        {
            // Check if all validations are correct
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {

                }
                catch (Exception)
                {
                    Response.Write(ErrorMessage.SEM001);
                    return View();
                }

                // Move to result page
                return RedirectToAction(string.Empty, Constants.HomeController);
            }
        }
    }
}
