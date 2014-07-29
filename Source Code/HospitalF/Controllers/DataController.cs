using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HospitalF.App_Start;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.Utilities;
using System.Threading.Tasks;

namespace HospitalF.Controllers
{
    public class DataController : SecurityBaseController
    {
        // Declare public list items for Drop down lists
        public static List<ServiceType> serviceTypeList = null;
        public static List<FacilityType> facilityTypeList = null;

        /// <summary>
        /// GET: /Data/ServiceList
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> ServiceList()
        {
            IPagedList<SP_LOAD_HOSPITAL_LISTResult> pagedHospitalList = null;

            try
            {
                // Load list of service type
                serviceTypeList = await ServiceFacilityUtil.LoadServiceTypeAsync();
                ViewBag.ServiceTypeList = new SelectList(serviceTypeList, Constants.TypeID, Constants.TypeName);

                // Load list of status
                ViewBag.CurrentStatus = true;

                // Declare new hospital list
                pagedHospitalList = new
                    List<SP_LOAD_HOSPITAL_LISTResult>().ToPagedList(1, Constants.PageSize);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }

            return View(pagedHospitalList);
        }

        /// <summary>
        /// GET: /Data/DisplayServiceList
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> DisplayServiceList(DataModel model, int? page)
        {
            // Cacading again drop down list
            ViewBag.ServiceTypeList = new SelectList(serviceTypeList, Constants.TypeID, Constants.TypeName);

            // Check if page parameter is null
            if (page == null)
            {
                page = 1;
            }

            return View();
        }
    }
}
