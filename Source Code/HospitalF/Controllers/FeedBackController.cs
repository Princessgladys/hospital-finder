using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalF.App_Start;
using HospitalF.Constant;
using HospitalF.Models;
using HospitalF.Utilities;

namespace HospitalF.Controllers
{
    public class FeedBackController : SecurityBaseController
    {

        public async Task<ActionResult> SystemFeedBack()
        {
            List<FeedbackType> feebackType = await FeedBackModels.LoadFeedbackTypeAsync();
            ViewBag.FeedbackTypeList = new SelectList(feebackType, Constants.FeedbackType, Constants.FeedbackTypeName);
            return PartialView("FeedBack");
        }

        public async Task<ActionResult> HospitalFeedBack(int hospitalID)
        {
            if (hospitalID != 0)
            {
                string email = string.Empty;
                FeedBackModels model = new FeedBackModels();
                Hospital hospital;
                List<FeedbackType> feebackType = await FeedBackModels.LoadFeedbackTypeAsync();
                ViewBag.FeedbackTypeList = new SelectList(feebackType, Constants.FeedbackType, Constants.FeedbackTypeName);
                hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalID);
                model.FeedbackType = 3;
                model.HospitalName = hospital.Hospital_Name;
                model.HospitalID = hospitalID;
                string name = User.Identity.Name;
                // login user is normal user
                if (User.IsInRole("User"))
                {
                    model.Email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
                }
                else if (User.IsInRole("Hospital User"))
                {
                    model.Email = User.Identity.Name.Split(Char.Parse(Constants.Minus))[0];
                }
                return PartialView("FeedBack", model);
            }
            else
            {
                return RedirectToAction("SystemFeedBack");
            }
        }

        [HttpPost]
        public async Task<ActionResult> FeedBack(FeedBackModels model)
        {
            try
            {
                string url = Request.Url.AbsoluteUri;
                int result = 0;

                // Return list of dictionary words
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    result = await FeedBackModels.InsertFeedbackAsync(model);
                }

                // Check returned result
                if (result == 1)
                {
                    ViewBag.AddFeedbackStatus = 1;
                    return RedirectToAction(Constants.HospitalAction, Constants.HomeController, new { hospitalId = model.HospitalID, redirect = "yes" });
                }
                else
                {
                    ViewBag.AddFeedbackStatus = 0;
                    ModelState.Clear();
                    return View();
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return RedirectToAction(Constants.SystemFailureHomeAction, Constants.ErrorController);
            }
        }

        [Authorize(Roles = Constants.HospitalUserRoleName)]
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> DisplayHospitalUserFeedBackList()
        {
            List<Feedback> list = null;
            string[] userInfo = HospitalF.Models.SimpleSessionPersister.Username.
                       Split(Char.Parse(Constants.Minus));
            int hospitalID = Int32.Parse(userInfo[3]);
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                list = await Task.Run(() => (from f in data.Feedbacks where f.Hospital_ID == hospitalID select f).ToList());
                ViewBag.FeedbackList = list;
                List<FeedbackType> TypeList = await Task.Run(() => (from ft in data.FeedbackTypes
                                                                    where ft.Type_ID == 3 || ft.Type_ID == 4
                                                                    select ft).ToList());
                ViewBag.TypeList = TypeList;
            }
            return View();
        }

        [Authorize(Roles = Constants.AdministratorRoleName)]
        [LayoutInjecter(Constants.AdmidLayout)]
        public async Task<ActionResult> DisplayFeedbackList()
        {
            List<Feedback> list = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                list = await Task.Run(() => (from f in data.Feedbacks where f.Hospital_ID == null select f).ToList());
                ViewBag.FeedbackList = list;
                List<FeedbackType> TypeList = await Task.Run(() => (from ft in data.FeedbackTypes
                                                                    where ft.Type_ID != 3 || ft.Type_ID != 4
                                                                    select ft).ToList());
                ViewBag.TypeList = TypeList;
            }
            return View();
        }
    }
}
