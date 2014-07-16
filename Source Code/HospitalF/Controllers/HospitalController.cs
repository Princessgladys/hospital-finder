﻿using System;
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
    public class HospitalController : Controller
    {
        public static List<Doctor> doctorList = null;
        //public static List<Speciality> specialityList = null;
        //public static List<Service> serviceList = null;
        //public static List<Facility> facilityList = null;
        //public static List<HospitalType> typeList = null;
        public static int hospitalID = 25;
        public Hospital hospital = null;
        public static HospitalModel model = null;
        //
        // GET: /Hospital/
        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> Index()
        {
            model = new HospitalModel();
            List<HospitalType> typeList = await HospitalUtil.LoadTypeInHospitalTypeAsync(hospitalID);
            hospital = await HospitalUtil.LoadHospitalByHospitalIDAsync(hospitalID);
            model.HospitalID = hospitalID;
            model.HospitalName = hospital.Hospital_Name;
            model.Address = hospital.Address;
            model.Website = hospital.Website;
            model.PhoneNo = hospital.Phone_Number;
            model.Fax = hospital.Fax;
            model.TypeName = model.LoadHospitalTypeInList((int)hospital.Hospital_Type, typeList);
            //load doctor of hospital
            //model.DoctorList = await HospitalUtil.LoadDoctorInDoctorHospitalAsync(hospitalID);
            //load speciality of hospital
            model.SpecialityList = await SpecialityUtil.LoadSpecialityByHospitalIDAsync(hospitalID);
            ViewBag.SpecialityList = new SelectList(model.SpecialityList, Constants.SpecialityID, Constants.SpecialityName);
            //load facility of hospital
            model.FacilityList = await HospitalUtil.LoadFacilityInHospitalFacilityAsync(hospitalID);
            //load service of hospital
            model.ServiceList = await HospitalUtil.LoadServiceInHospitalServiceAsync(hospitalID);
            return View(model);
        }
        public async Task<ActionResult> SearchDoctor(string SpecialityID, string DoctorName, string HospitalID)
        {
            try
            {
                int tempSpecialityID, tempHospitalID;
                doctorList = new List<Doctor>();
                Photo photo = null;
                if (SpecialityID == "")
                {
                    SpecialityID = "0";
                }
                if (!String.IsNullOrEmpty(SpecialityID) && Int32.TryParse(SpecialityID, out tempSpecialityID)
                    && Int32.TryParse(HospitalID, out tempHospitalID))
                {
                    doctorList = await HospitalUtil.SearchDoctor(DoctorName, tempSpecialityID, tempHospitalID);
                    ViewBag.DoctorList = doctorList;
                }
                return PartialView("SearchResult");
            }
            catch (Exception ex)
            {
                LoggingUtil.LogException(ex);
                return RedirectToAction(Constants.SystemFailureHospitalUserAction, Constants.ErrorController);
            }

        }

        [LayoutInjecter(Constants.HospitalUserLayout)]
        public async Task<ActionResult> ViewDoctorDetail(string doctorID)
        {
            int tempDoctorID;
            Doctor doctor = null;
            if (!string.IsNullOrEmpty(doctorID) && Int32.TryParse(doctorID, out tempDoctorID))
            {
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    doctor = await Task.Run(() => (
                        from d in data.Doctors
                        where d.Doctor_ID == tempDoctorID
                        select d).FirstOrDefault());
                    ViewBag.Doctor = doctor;
                }
            }
            return View();
        }
    }
}
