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
    public class DoctorController : Controller
    {

        public static DoctorModel DoctorModel = null;

        //
        // GET: /Doctor/
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get/UpdateDoctor
        /// </summary>
        /// <returns></returns>
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> UpdateDoctor()
        {
            int doctorID=1;
            int hospitalID = 68;
            Doctor doctor = new Doctor();
            DoctorModel=new DoctorModel();
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                doctor = await Task.Run(()=>(
                    from d in data.Doctors
                    where d.Doctor_ID==doctorID
                    select d).FirstOrDefault());
            }
            DoctorModel.DoctorID = doctor.Doctor_ID;
            DoctorModel.Fullname = doctor.Last_Name + " " + doctor.First_Name;
            DoctorModel.Experience = doctor.Experience;
            DoctorModel.Degree = doctor.Degree;
            //DoctorModel.WorkingDay = doctor.Working_Day;
            //specialiy of doctor
            List<Speciality> SpecialityList = null;
            SpecialityList=await SpecialityUtil.LoadSpecialityInDoctorSpeciality(doctorID);
            List<String> specialityList=new List<string>();
            foreach (Speciality sp in SpecialityList)
            {
                specialityList.Add(sp.Speciality_ID.ToString());
            }
            SpecialityList=await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
            ViewBag.SpecialityList = new SelectList(SpecialityList, Constants.SpecialityID, Constants.SpecialityName);
            //DoctorModel.SelectedSpecialities = specialityList;

            return View(DoctorModel);
        }

        public ActionResult UpdateDoctor(DoctorModel model)
        {
            return RedirectToAction(Constants.IndexAction,Constants.HospitalController);
        }

    }
}
