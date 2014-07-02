using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.Entities;
using HospitalF.App_Start;

namespace HospitalF.Controllers
{
    public class HospitalUserHomeController : SecurityBaseController
    {
        /// <summary>
        /// GET: /HospitalUserHome/Index
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> Index()
        {
            return View();
        }

    }
}
