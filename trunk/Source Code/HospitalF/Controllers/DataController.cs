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
        public static List<Speciality> specialityList = null;

        #region Service

        /// <summary>
        /// GET: /Data/ChangeServiceStatus
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>
        /// Task[ActionResult] with JSON contains value
        /// indicating update process is successful or not
        /// 1: Successful
        /// 0: Failed
        /// </returns>
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> ChangeServiceStatus(int serviceId)
        {
            try
            {
                DataModel model = new DataModel();
                int result = await model.ChangeServiceStatusAsync(serviceId);
                TempData[Constants.ProcessStatusData] = result;
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

                #region Load data

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
                    serviceList = await model.LoadListOfService(
                        model.ServiceName, model.TypeID, model.IsActive);
                    ViewBag.CurrentStatus = model.IsActive;
                }

                #endregion

                #region Display notification

                // Pass value of previous adding service to view (if any)
                if (TempData[Constants.ProcessInsertData] != null)
                {
                    ViewBag.AddStatus = (int)TempData[Constants.ProcessInsertData];
                }

                // Pass value of previous updating service to view (if any)
                if (TempData[Constants.ProcessUpdateData] != null)
                {
                    ViewBag.UpdateStatus = (int)TempData[Constants.ProcessUpdateData];
                }

                // Pass value of previous updating service status to view (if any)
                if (TempData[Constants.ProcessStatusData] != null)
                {
                    ViewBag.ChangeStatus = (int)TempData[Constants.ProcessStatusData];
                }

                #endregion

                // Handle query string
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove(Constants.PageUrlRewriting);
                ViewBag.Query = queryString.ToString();

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
                TempData[Constants.ProcessInsertData] = result;
                return RedirectToAction(Constants.DisplayServiceAction, Constants.DataController, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Load paritial view Update Service
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> UpdateService(int serviceId)
        {
            try
            {
                // Load service information
                DataModel model = new DataModel();
                Service service = await model.LoadSerivceById(serviceId);
                if (service != null)
                {
                    model.ServiceName = service.Service_Name;
                    model.TypeID = service.Service_Type.Value;
                    model.ServiceID = serviceId;
                }

                // Load list of service type
                serviceTypeList = await ServiceFacilityUtil.LoadServiceTypeAsync();
                ViewBag.ServiceTypeList = new SelectList(serviceTypeList, Constants.TypeID, Constants.TypeName);

                return PartialView(Constants.UpdateServiceAction, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Update Service
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> UpdateService(DataModel model)
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
                    result = await model.UpdateService(model);
                }

                // Return result
                TempData[Constants.ProcessUpdateData] = result;
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
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> ChangeFacilityStatus(int facilityId)
        {
            try
            {
                DataModel model = new DataModel();
                int result = await model.ChangeFacilityStatusAsync(facilityId);
                TempData[Constants.ProcessStatusData] = result;
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

                #region Load data

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

                #endregion

                #region Display notification

                // Pass value of previous adding facility to view (if any)
                if (TempData[Constants.ProcessInsertData] != null)
                {
                    ViewBag.AddStatus = (int)TempData[Constants.ProcessInsertData];
                }

                // Pass value of previous updating service to view (if any)
                if (TempData[Constants.ProcessUpdateData] != null)
                {
                    ViewBag.UpdateStatus = (int)TempData[Constants.ProcessUpdateData];
                }

                // Pass value of previous updating service status to view (if any)
                if (TempData[Constants.ProcessStatusData] != null)
                {
                    ViewBag.ChangeStatus = (int)TempData[Constants.ProcessStatusData];
                }

                #endregion

                // Handle query string
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove(Constants.PageUrlRewriting);
                ViewBag.Query = queryString.ToString();

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
                TempData[Constants.ProcessInsertData] = result;
                return RedirectToAction(Constants.DisplayFacilityAction, Constants.DataController, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Load paritial view Update Facility
        /// </summary>
        /// <param name="facilityId">Facility ID</param>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> UpdateFacility(int facilityId)
        {
            try
            {
                // Load service information
                DataModel model = new DataModel();
                Facility facility = await model.LoadFacilityById(facilityId);
                if (facility != null)
                {
                    model.FacilityName = facility.Facility_Name;
                    model.TypeID = facility.Facility_Type.Value;
                    model.FacilityID = facilityId;
                }

                // Load list of service type
                facilityTypeList = await ServiceFacilityUtil.LoadFacilityTypeAsync();
                ViewBag.FacilityTypeList = new SelectList(facilityTypeList, Constants.TypeID, Constants.TypeName);

                return PartialView(Constants.UpdateFacilityAction, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Update Facility
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> UpdateFacility(DataModel model)
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
                    result = await model.UpdateFacility(model);
                }

                // Return result
                TempData[Constants.ProcessUpdateData] = result;
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
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> ChangeSpecialityStatus(int SpecialityId)
        {
            try
            {
                DataModel model = new DataModel();
                int result = await model.ChangeSpecialityStatusAsync(SpecialityId);
                TempData[Constants.ProcessStatusData] = result;
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

                #region Load data

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

                #endregion

                #region Display notification

                // Pass value of previous adding facility to view (if any)
                if (TempData[Constants.ProcessInsertData] != null)
                {
                    ViewBag.AddStatus = (int)TempData[Constants.ProcessInsertData];
                }

                // Pass value of previous updating service to view (if any)
                if (TempData[Constants.ProcessUpdateData] != null)
                {
                    ViewBag.UpdateStatus = (int)TempData[Constants.ProcessUpdateData];
                }

                // Pass value of previous updating service status to view (if any)
                if (TempData[Constants.ProcessStatusData] != null)
                {
                    ViewBag.ChangeStatus = (int)TempData[Constants.ProcessStatusData];
                }

                #endregion

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

        /// <summary>
        /// Load paritial view Add Speciality
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult AddSpeciality()
        {
            return PartialView(Constants.AddSpecialityAction);
        }

        /// <summary>
        /// Add new Speciality
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> AddSpeciality(DataModel model)
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
                    result = await model.AddSpeciality(model);
                }

                // Return result
                TempData[Constants.ProcessInsertData] = result;
                return RedirectToAction(Constants.DisplaySpecialityAction, Constants.DataController, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Load paritial view Update Speciality
        /// </summary>
        /// <param name="specialityId">Speciality ID</param>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> UpdateSpeciality(int specialityId)
        {
            try
            {
                // Load service information
                DataModel model = new DataModel();
                Speciality speciality = await model.LoadSpecialityById(specialityId);
                if (speciality != null)
                {
                    model.SpecialityName = speciality.Speciality_Name;
                    model.SpecialityID = specialityId;
                }

                return PartialView(Constants.UpdateSpecialityAction, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Update Speciality
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> UpdateSpeciality(DataModel model)
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
                    result = await model.UpdateSpeciality(model);
                }

                // Return result
                TempData[Constants.ProcessUpdateData] = result;
                return RedirectToAction(Constants.DisplaySpecialityAction, Constants.DataController, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        #region Disease

        /// <summary>
        /// GET: /Data/ChangeDiseaseStatus
        /// </summary>
        /// <param name="diseaseId">Disease ID</param>
        /// <returns>
        /// Task[ActionResult] with JSON contains value
        /// indicating update process is successful or not
        /// 1: Successful
        /// 0: Failed
        /// 2: Disease has been mapped with another speciality
        /// </returns>
        public async Task<ActionResult> ChangeDiseaseStatus(int diseaseId)
        {
            try
            {
                DataModel model = new DataModel();
                int result = await model.ChangeDiseaseStatusAsync(diseaseId);
                TempData[Constants.ProcessStatusData] = result;
                return Json(new { value = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// GET: /Data/DiseaseList
        /// </summary>
        /// <returns>Task[ActionResult]</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> DiseaseList(DataModel model, int? page)
        {
            IPagedList<SP_TAKE_DISEASE_AND_TYPEResult> pagedDiseaseList = null;
            try
            {
                // Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                // Check if page parameter is null
                if (page == null)
                {
                    page = 1;
                }

                #region Load data

                // Load list of service
                List<SP_TAKE_DISEASE_AND_TYPEResult> diseaseList =
                    new List<SP_TAKE_DISEASE_AND_TYPEResult>();
                if (model.IsPostBack == false)
                {
                    diseaseList = await model.LoadListOfDisease(null, true, 1, 0);
                    ViewBag.CurrentStatus = true;
                    ViewBag.CurrentMode = 1;
                    ViewBag.CurrentOption = true;
                }
                else
                {
                    if (model.Option == false)
                    {
                        diseaseList = await model.LoadListOfDisease(
                            model.DiseaseName, model.IsActive, 0, 0);
                    }
                    else
                    {
                        if (model.Mode == 1)
                        {
                            if (model.SpecialityID == 0)
                            {
                                diseaseList = await model.LoadListOfDisease(
                                    model.DiseaseName, model.IsActive, 1, 0);
                            }
                            else
                            {
                                diseaseList = await model.LoadListOfDisease(
                                    model.DiseaseName, model.IsActive, 2, model.SpecialityID);
                            }
                        }
                        else
                        {
                            diseaseList = await model.LoadListOfDisease(
                                    model.DiseaseName, model.IsActive, 3, 0);
                        }
                        
                    }
                    
                    ViewBag.CurrentStatus = model.IsActive;
                    ViewBag.CurrentMode = model.Mode;
                    ViewBag.CurrentOption = model.Option;
                }

                #endregion

                #region Display notification

                // Pass value of previous adding service to view (if any)
                if (TempData[Constants.ProcessInsertData] != null)
                {
                    ViewBag.AddStatus = (int)TempData[Constants.ProcessInsertData];
                }

                // Pass value of previous updating service to view (if any)
                if (TempData[Constants.ProcessUpdateData] != null)
                {
                    ViewBag.UpdateStatus = (int)TempData[Constants.ProcessUpdateData];
                }

                // Pass value of previous updating service status to view (if any)
                if (TempData[Constants.ProcessStatusData] != null)
                {
                    ViewBag.ChangeStatus = (int)TempData[Constants.ProcessStatusData];
                }

                #endregion

                // Handle query string
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(Request.Url.Query);
                queryString.Remove(Constants.PageUrlRewriting);
                ViewBag.Query = queryString.ToString();

                // Return value to view
                pagedDiseaseList = diseaseList.ToPagedList(page.Value, Constants.PageSize);
                return View(pagedDiseaseList);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Load paritial view Add Disease
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public async Task<ActionResult> AddDisease()
        {
            try
            {
                //Load list of specialities
                specialityList = await SpecialityUtil.LoadSpecialityAsync();
                ViewBag.SpecialityList = new SelectList(specialityList, Constants.SpecialityID, Constants.SpecialityName);

                return PartialView(Constants.AddDiseaseAction);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        /// <summary>
        /// Add new Disease
        /// </summary>
        /// <returns>ActionResult</returns>
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [HttpPost]
        public async Task<ActionResult> AddDisease(DataModel model)
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
                    result = await model.AddDiseaseAsync(model.DiseaseName, model.SelectedSpecialities);
                }

                // Return result
                TempData[Constants.ProcessInsertData] = result;
                return RedirectToAction(Constants.DisplayServiceAction, Constants.DataController, model);
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        #endregion

        #region Statistic
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult Statistic(string sFromDate, string sToDate)
        {
            try
            {
                ViewBag.TotalHospitalCount = DataModel.TotalHospitalCount();
                ViewBag.TotalInactiveHospitalCount = DataModel.TotalInactiveHospitalCount();
                ViewBag.TotalMemberHospitalCount = DataModel.TotalMemberHospitalCount();
                ViewBag.TotalNoMemberHospitalCount = ViewBag.TotalHospitalCount - ViewBag.TotalInactiveHospitalCount - ViewBag.TotalMemberHospitalCount;
                ViewBag.TopTenBestRatingHospital = DataModel.TopTenRatingHospital();
                ViewBag.HospitalTypeCount = DataModel.HospitalTypeCount();
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                if (string.IsNullOrEmpty(sFromDate) || string.IsNullOrEmpty(sToDate))
                {
                    DateTime today = DateTime.Today;
                    fromDate = new DateTime(today.Year, today.Month - 1, 1);
                    toDate = new DateTime(today.Year, today.Month, 1);
                }
                else
                {
                    fromDate = DateTime.ParseExact(sFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(sToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                ViewBag.TopTenHospitalAppointment = DataModel.TopTenHospitalAppointment(fromDate, toDate);
                ViewBag.FromDate = string.Format("{0:dd/MM/yyyy}", fromDate);
                ViewBag.ToDate = string.Format("{0:dd/MM/yyyy}", toDate);
                return View();
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult SearchQueryStatistic(string sFromDate, string sToDate, int page = 1)
        {
            try
            {
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                if (string.IsNullOrEmpty(sFromDate) || string.IsNullOrEmpty(sToDate))
                {
                    DateTime today = DateTime.Today;
                    fromDate = new DateTime(today.Year, today.Month - 1, 1);
                    toDate = new DateTime(today.Year, today.Month, 1);
                }
                else
                {
                    fromDate = DateTime.ParseExact(sFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(sToDate + " 23:59:59", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                ViewBag.SearchQueryStatistic = DataModel.SearchQueryStatistic(fromDate, toDate).ToPagedList(page, Constants.PageSize + 5);
                ViewBag.FromDate = string.Format("{0:dd/MM/yyyy}", fromDate);
                ViewBag.ToDate = string.Format("{0:dd/MM/yyyy}", toDate);
                return View();
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }


        }
        #endregion

        #region Feedback
        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult Feedback(string sFromDate, string sToDate, int feedbackType = 0, int responseType = 0, int page = 1)
        {
            try
            {
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                if (string.IsNullOrEmpty(sFromDate) || string.IsNullOrEmpty(sToDate))
                {
                    DateTime today = DateTime.Today;
                    fromDate = new DateTime(today.Year, today.Month - 1, 1);
                    toDate = new DateTime(today.Year, today.Month, 1);
                }
                else
                {
                    fromDate = DateTime.ParseExact(sFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(sToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                ViewBag.FeedbackList = FeedBackModel.LoadAdministratorFeedback(fromDate, toDate, feedbackType, responseType).ToPagedList(page, Constants.PageSize);

                ViewBag.FromDate = string.Format("{0:dd/MM/yyyy}", fromDate);
                ViewBag.ToDate = string.Format("{0:dd/MM/yyyy}", toDate);

                List<FeedbackType> feebackTypeList = FeedBackModel.LoadFeedbeackTypeList();
                List<SelectListItem> feedbackTypeItemList = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "0", Text = "Tất cả loại phản hồi"},
                                                                    new SelectListItem {Value = feebackTypeList[0].Type_ID.ToString(), Text = feebackTypeList[0].Type_Name},
                                                                    new SelectListItem {Value = feebackTypeList[1].Type_ID.ToString(), Text = feebackTypeList[1].Type_Name},
                                                                    new SelectListItem {Value = feebackTypeList[2].Type_ID.ToString(), Text = feebackTypeList[2].Type_Name},
                                                                };
                ViewBag.FeedbackTypeList = new SelectList(feedbackTypeItemList, "Value", "Text", feedbackType);
                ViewBag.FeedbackType = feedbackType;

                List<SelectListItem> responseItemList = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "0", Text = "Tất cả phản hồi"},
                                                                    new SelectListItem {Value = "1", Text = "Đã duyệt"},
                                                                    new SelectListItem {Value = "2", Text = "Chưa duyệt"}
                                                                };
                ViewBag.ResponseTypeList = new SelectList(responseItemList, "Value", "Text", responseType);
                ViewBag.ResponseType = responseType;

                return View();
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult ApproveFeedback(int feedbackId = 0)
        {
            FeedBackModel.ApproveFeedback(feedbackId);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [LayoutInjecter(Constants.AdmidLayout)]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        public ActionResult UserList(string email = "", int userRole = 0, int userStatus = 0, int page = 1)
        {
            try
            {
                string exclusiveEmail = "";
                if (SimpleSessionPersister.Username != null)
                {
                    exclusiveEmail = SimpleSessionPersister.Username.Split(Char.Parse(Constants.Minus))[0];
                }
                ViewBag.UserList = AccountModel.LoadUser(email, userRole, userStatus, exclusiveEmail).ToPagedList(page, Constants.PageSize);
                List<Role> roleList = AccountModel.LoadUserRole();
                ViewBag.RoleTypeList = new SelectList(roleList, "Role_ID", "Role_Name", userRole);
                ViewBag.UserRole = userRole;
                List<SelectListItem> statusItemList = new List<SelectListItem>()
                                                                {
                                                                    new SelectListItem {Value = "0", Text = "Tất cả trạng thái"},
                                                                    new SelectListItem {Value = "1", Text = "Đang hoạt động"},
                                                                    new SelectListItem {Value = "2", Text = "Đã khóa"}
                                                                };
                ViewBag.StatusTypeList = new SelectList(statusItemList, "Value", "Text", userStatus);
                ViewBag.UserStatus = userStatus;
                ViewBag.Email = email;
                return View();
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
