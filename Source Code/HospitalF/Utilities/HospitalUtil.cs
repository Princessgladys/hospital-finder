using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Models;

namespace HospitalF.Utilities
{
    public class HospitalUtil
    {
        #region Load hospital by hospital ID
        public static async Task<Hospital> LoadHospitalByHospitalIDAsync(int hospitalID)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from h in data.Hospitals
                     where h.Hospital_ID == hospitalID
                     select h).FirstOrDefault());
            }
        }
        #endregion


        #region load avatar of doctor
        public static async Task<Photo> LoadPhotoOfDoctorAsync(int photoID)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() => (
                    from p in data.Photos
                    where p.Photo_ID == photoID
                    select p).FirstOrDefault());
            }
        }
        #endregion

        #region Load Doctor of hospital
        public static async Task<List<Doctor>> LoadDoctorInDoctorHospitalAsync(int hospitalID)
        {
            List<SP_LOAD_DOCTOR_IN_DOCTOR_HOSPITALResult> result = null;
            List<Doctor> doctorList = new List<Doctor>();
            Doctor doctor = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_LOAD_DOCTOR_IN_DOCTOR_HOSPITAL(hospitalID).ToList());
            }

            foreach (SP_LOAD_DOCTOR_IN_DOCTOR_HOSPITALResult re in result)
            {
                doctor = new Doctor();
                doctor.Doctor_ID = re.Doctor_ID;
                doctor.First_Name = re.First_Name;
                doctor.Last_Name = re.Last_Name;
                doctor.Degree = re.Degree;
                doctor.Experience = re.Experience;
                doctor.Photo_ID = re.Photo_ID;
                doctor.Working_Day = re.Working_Day;
                doctorList.Add(doctor);
            }
            return doctorList;
        }
        #endregion

        #region load all facilities of hospital
        public static async Task<List<Facility>> LoadFacilityInHospitalFacilityAsync(int hospitalID)
        {
            List<SP_LOAD_FACILITY_IN_HOSPITAL_FACILITYResult> result = null;
            List<Facility> facilityList = null;
            Facility facility = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_LOAD_FACILITY_IN_HOSPITAL_FACILITY(hospitalID).ToList());
            }
            foreach (SP_LOAD_FACILITY_IN_HOSPITAL_FACILITYResult re in result)
            {
                facility = new Facility();
                facility.Facility_ID = re.Facility_ID;
                facility.Facility_Name = re.Facility_Name;
                facilityList.Add(facility);
            }
            return facilityList;
        }
        #endregion

        #region loadd all services of hospital
        public static async Task<List<Service>> LoadServiceInHospitalServiceAsync(int hospitalID)
        {
            List<SP_LOAD_SERVICE_IN_HOSPITAL_SERVICEResult> result = null;
            List<Service> serviceList = null;
            Service service = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_LOAD_SERVICE_IN_HOSPITAL_SERVICE(hospitalID).ToList());
            }
            foreach (SP_LOAD_SERVICE_IN_HOSPITAL_SERVICEResult re in result)
            {
                service = new Service();
                service.Service_ID = re.Service_ID;
                service.Service_Name = re.Service_Name;
                serviceList.Add(service);
            }
            return serviceList;
        }
        #endregion

        #region load type of hospital
        public static async Task<List<HospitalType>> LoadTypeInHospitalTypeAsync(int hospitalID)
        {
            List<HospitalType> typeList = new List<HospitalType>();
            HospitalType type = null;
            List<SP_LOAD_TYPE_OF_HOSPITALResult> result = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_LOAD_TYPE_OF_HOSPITAL(hospitalID).ToList());
            }
            foreach (SP_LOAD_TYPE_OF_HOSPITALResult re in result)
            {
                type = new HospitalType();
                type.Type_ID = re.Type_ID;
                type.Type_Name = re.Type_Name;
                typeList.Add(type);
            }
            return typeList;
        }
        #endregion

        #region load doctor
        /// <summary>
        /// Load all doctor in Doctor_Speciality
        /// </summary>
        /// <param name="SpecialityID"></param>
        /// <returns>List[DoctorEnity] that contains list of doctor with appropriate Speciality code</returns>
        public static async Task<List<Doctor>> LoadDoctorInDoctorSpecialityAsyn(int SpecialityID)
        {
            List<Doctor> doctorList = new List<Doctor>();
            Doctor doctor = null;
            List<SP_LOAD_DOCTOR_BY_SPECIALITYIDResult> result = null;
            // Take doctor in specific speciality in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                data.SP_LOAD_DOCTOR_BY_SPECIALITYID(SpecialityID).ToList());
            }
            // Assign value for each doctor
            foreach (SP_LOAD_DOCTOR_BY_SPECIALITYIDResult r in result)
            {
                doctor = new Doctor();
                doctor.Doctor_ID = r.Doctor_ID;
                doctor.First_Name = r.First_Name;
                doctor.Last_Name = r.Last_Name;
                doctor.Degree=r.Degree;
                doctor.Experience=r.Experience;
                doctor.Working_Day=r.Working_Day;
                doctor.Photo_ID=r.Photo_ID;
                doctorList.Add(doctor);
            }
            Appointment app = new Appointment();
            return doctorList;
        }
        #endregion

        #region search doctor by doctor name and speciality
        public static async Task<List<Doctor>> SearchDoctor(string DoctorName, int SpecialityID, int HospitalID)
        {
            List<Doctor> doctorList = new List<Doctor>();
            Doctor doctor = null;
            List<SP_SEARCH_DOCTORResult> result = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(()=>
                    data.SP_SEARCH_DOCTOR(DoctorName,SpecialityID,HospitalID).ToList());
            }
            foreach (SP_SEARCH_DOCTORResult r in result)
            {
                doctor = new Doctor();
                doctor.Doctor_ID = r.Doctor_ID;
                doctor.First_Name = r.First_Name;
                doctor.Last_Name = r.Last_Name;
                doctor.Degree = r.Degree;
                doctor.Experience = r.Experience;
                doctor.Working_Day = r.Working_Day;
                doctor.Photo_ID = r.Photo_ID;
                doctorList.Add(doctor);
            }
            return doctorList;
        }
        #endregion
    }
}