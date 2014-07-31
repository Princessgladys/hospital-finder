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
using System.Collections.Specialized;

namespace HospitalF.Controllers
{
    public class DataController : SecurityBaseController
    {
        // Declare public list items for Drop down lists
        public static List<ServiceType> serviceTypeList = null;
        public static List<FacilityType> facilityTypeList = null;

        #region Service

        /// <summary>
        /// GET: /Data/ChangeServiceStatus
        /// </summary>
        /// <param name="hospitalId">Hosptal ID</param>
        /// <returns>
        /// Task[ActionResult] with JSON contains value
        /// indicating update process is successful or not
        /// 1: Successful
        /// 0: Failed
        /// </returns>
        public async Task<ActionResult> ChangeServiceStatus(int serviceId)
        {
            try
            {
                DataModel model = new DataModel();
                int result = await model.ChangeServiceStatusAsync(serviceId);
                return Json(new { value = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Data/ServiceList
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> ServiceList(DataModel model, int? page)
        {
            IPagedList<SP_TAKE_SERVICE_AND_TYPEResult> pagedServiceList = null;

            try
            {
                // Load list of service type
                serviceTypeList = await ServiceFacilityUtil.LoadServiceTypeAsync();
                ViewBag.ServiceTypeList = new SelectList(serviceTypeList, Constants.TypeID, Constants.TypeName);

                // Check if page parameter is null
                if (page == null)
                {
                    page = 1;
                }

                // Load list of service
                List<SP_TAKE_SERVICE_AND_TYPEResult> serviceListlList =
                    new List<SP_TAKE_SERVICE_AND_TYPEResult>();
                if (model.IsPostBack == false)
                {
                    serviceListlList = await model.LoadListOfService(null, 0, true);
                    ViewBag.CurrentStatus = true;
                }
                else
                {
                    serviceListlList =  await model.LoadListOfService(
                        model.ServiceName, model.TypeID, model.IsActive);
                    ViewBag.CurrentStatus = model.IsActive;
                }

                // Handle query string
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove(Constants.PageUrlRewriting);
                ViewBag.Query = queryString.ToString();

                // Return value to view
                pagedServiceList = serviceListlList.ToPagedList(page.Value, Constants.PageSize);
                return View(pagedServiceList);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }


        }

        #endregion
    }
}
