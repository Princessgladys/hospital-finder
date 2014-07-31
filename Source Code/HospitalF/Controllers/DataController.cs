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

                // Load list of status
                ViewBag.CurrentStatus = true;

                // Check if page parameter is null
                if (page == null)
                {
                    page = 1;
                }

                // Load list of service
                List<SP_TAKE_SERVICE_AND_TYPEResult> serviceListlList =
                    new List<SP_TAKE_SERVICE_AND_TYPEResult>();
                if (model.ServiceName == null)
                {
                    serviceListlList = await model.LoadListOfService(null, 0, true);
                }
                else
                {
                    serviceListlList =  await model.LoadListOfService(
                        model.ServiceName.Trim(), model.TypeID, model.IsActive);
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
