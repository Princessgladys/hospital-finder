using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using HospitalF.Entities;
using HospitalF.Models;

namespace HospitalF.Utilities
{
    public class DoctorUtil
    {
        /// <summary>
        /// Load all doctor in Doctor_Speciality
        /// </summary>
        /// <param name="SpecialityID"></param>
        /// <returns>List[DoctorEnity] that contains list of doctor with appropriate Speciality code</returns>
        public static async Task<List<DoctorEntity>> LoadDoctorInDoctorSpecialityAsyn(int SpecialityID)
        {
            List<DoctorEntity> doctorList = new List<DoctorEntity>();
            DoctorEntity doctor = null;
            List<SP_LOAD_DOCTOR_IN_DOCTOR_SPECIALITYResult> result = null;
            // Take doctor in specific speciality in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result=await Task.Run(()=>
                data.SP_LOAD_DOCTOR_IN_DOCTOR_SPECIALITY(SpecialityID).ToList());
            }
            // Assign value for each doctor
            foreach (SP_LOAD_DOCTOR_IN_DOCTOR_SPECIALITYResult r in result)
            {
                doctor = new DoctorEntity();
                doctor.DoctorID = r.Doctor_ID;
                doctor.FirstName = r.First_Name;
                doctor.LastName = r.Last_Name;
                doctorList.Add(doctor);
            }
            return doctorList;
        }
    }
}