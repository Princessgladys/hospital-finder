﻿using System;
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
        /// <param name="hospitalId">Service ID</param>
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
                List<SP_TAKE_SERVICE_AND_TYPEResult> serviceList =
                    new List<SP_TAKE_SERVICE_AND_TYPEResult>();
                if (model.IsPostBack == false)
                {
                    serviceList = await model.LoadListOfService(null, 0, true);
                    ViewBag.CurrentStatus = true;
                }
                else
                {
                    serviceList =  await model.LoadListOfService(
                        model.ServiceName, model.TypeID, model.IsActive);
                    ViewBag.CurrentStatus = model.IsActive;
                }

                // Handle query string
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove(Constants.PageUrlRewriting);
                ViewBag.Query = queryString.ToString();

                // Pass value of previous adding service to view (if any)
                if (TempData[Constants.ProcessData] != null)
                {
                    ViewBag.AddStatus = (int)TempData[Constants.ProcessData];
                }

                // Return value to view
                pagedServiceList = serviceList.ToPagedList(page.Value, Constants.PageSize);
                return View(pagedServiceList);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Load paritial view Add Service
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> AddService()
        {
            try
            {
                // Load list of service type
                serviceTypeList = await ServiceFacilityUtil.LoadServiceTypeAsync();
                ViewBag.ServiceTypeList = new SelectList(serviceTypeList, Constants.TypeID, Constants.TypeName);

                return PartialView(Constants.AddServiceAction);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Add new Service
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> AddService(DataModel model)
        {
            try
            {
                // Prepare data
                int result = 0;
                model.IsPostBack = true;
                model.IsActive = true;

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.AddService(model);
                }

                // Return result
                TempData[Constants.ProcessData] = result;
                return RedirectToAction(Constants.DisplayServiceAction, Constants.DataController, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        #region Facility

        /// <summary>
        /// GET: /Data/ChangeFacilityStatus
        /// </summary>
        /// <param name="facilityId">Facility ID</param>
        /// <returns>
        /// Task[ActionResult] with JSON contains value
        /// indicating update process is successful or not
        /// 1: Successful
        /// 0: Failed
        /// </returns>
        public async Task<ActionResult> ChangeFacilityStatus(int facilityId)
        {
            try
            {
                DataModel model = new DataModel();
                int result = await model.ChangeFacilityStatusAsync(facilityId);
                return Json(new { value = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Data/FacilityList
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> FacilityList(DataModel model, int? page)
        {
            IPagedList<SP_TAKE_FACILITY_AND_TYPEResult> pagedFacilityList = null;

            try
            {
                // Load list of service type
                facilityTypeList = await ServiceFacilityUtil.LoadFacilityTypeAsync();
                ViewBag.FacilityTypeList = new SelectList(facilityTypeList, Constants.TypeID, Constants.TypeName);

                // Check if page parameter is null
                if (page == null)
                {
                    page = 1;
                }

                // Load list of service
                List<SP_TAKE_FACILITY_AND_TYPEResult> facilityList =
                    new List<SP_TAKE_FACILITY_AND_TYPEResult>();
                if (model.IsPostBack == false)
                {
                    facilityList = await model.LoadListOfFacility(null, 0, true);
                    ViewBag.CurrentStatus = true;
                }
                else
                {
                    facilityList = await model.LoadListOfFacility(
                        model.FacilityName, model.TypeID, model.IsActive);
                    ViewBag.CurrentStatus = model.IsActive;
                }

                // Handle query string
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove(Constants.PageUrlRewriting);
                ViewBag.Query = queryString.ToString();

                // Pass value of previous adding facility to view (if any)
                if (TempData[Constants.ProcessData] != null)
                {
                    ViewBag.AddStatus = (int)TempData[Constants.ProcessData];
                }

                // Return value to view
                pagedFacilityList = facilityList.ToPagedList(page.Value, Constants.PageSize);
                return View(pagedFacilityList);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Load paritial view Add Facility
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> AddFacility()
        {
            try
            {
                // Load list of service type
                facilityTypeList = await ServiceFacilityUtil.LoadFacilityTypeAsync();
                ViewBag.FacilityTypeList = new SelectList(facilityTypeList, Constants.TypeID, Constants.TypeName);

                return PartialView(Constants.AddFacilityAction);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Add new Facility
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> AddFacility(DataModel model)
        {
            try
            {
                // Prepare data
                int result = 0;
                model.IsPostBack = true;
                model.IsActive = true;

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await model.AddFacility(model);
                }

                // Return result
                TempData[Constants.ProcessData] = result;
                return RedirectToAction(Constants.DisplayFacilityAction, Constants.DataController, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        #region Speciality

        /// <summary>
        /// GET: /Data/ChangeSpecialityStatus
        /// </summary>
        /// <param name="SpecialityId">Speciality ID</param>
        /// <returns>
        /// Task[ActionResult] with JSON contains value
        /// indicating update process is successful or not
        /// 1: Successful
        /// 0: Failed
        /// </returns>
        public async Task<ActionResult> ChangeSpecialityStatus(int SpecialityId)
        {
            try
            {
                DataModel model = new DataModel();
                int result = await model.ChangeSpecialityStatusAsync(SpecialityId);
                return Json(new { value = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Data/SpecialityList
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> SpecialityList(DataModel model, int? page)
        {
            IPagedList<SP_TAKE_SPECIALITY_AND_TYPEResult> pagedFacilityList = null;

            try
            {
                // Check if page parameter is null
                if (page == null)
                {
                    page = 1;
                }

                // Load list of service
                List<SP_TAKE_SPECIALITY_AND_TYPEResult> specialityList =
                    new List<SP_TAKE_SPECIALITY_AND_TYPEResult>();
                if (model.IsPostBack == false)
                {
                    specialityList = await model.LoadListOfSpeciality(null, true);
                    ViewBag.CurrentStatus = true;
                }
                else
                {
                    specialityList = await model.LoadListOfSpeciality(model.SpecialityName, model.IsActive);
                    ViewBag.CurrentStatus = model.IsActive;
                }

                // Handle query string
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove(Constants.PageUrlRewriting);
                ViewBag.Query = queryString.ToString();

                // Return value to view
                pagedFacilityList = specialityList.ToPagedList(page.Value, Constants.PageSize);
                return View(pagedFacilityList);
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
