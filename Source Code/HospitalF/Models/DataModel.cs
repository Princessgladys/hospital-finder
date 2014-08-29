using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Constant;
using HospitalF.Entities;

namespace HospitalF.Models
{
    /// <summary>
    /// Class define bussiness method for DataModel
    /// </summary>
    public class DataModel
    {
        #region Properties

        /// <summary>
        /// Get/Set value for property HospitalID
        /// </summary>
        public int ServiceID { get; set; }

        /// <summary>
        /// Get/Set value for property ServiceName
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Get/Set value for property FacilityID
        /// </summary>
        public int FacilityID { get; set; }

        /// <summary>
        /// Get/Set value for property FacilityName
        /// </summary>
        public string FacilityName { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityID
        /// </summary>
        public int SpecialityID { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityName
        /// </summary>
        public string SpecialityName { get; set; }

        /// <summary>
        /// Get/Set value for property DiseaseID
        /// </summary>
        public int DiseaseID { get; set; }

        /// <summary>
        /// Get/Set value for property DiseaseName
        /// </summary>
        public string DiseaseName { get; set; }

        /// <summary>
        /// Get/Set value for property IsMainSpeciality
        /// </summary>
        public bool IsMainSpeciality { get; set; }

        /// <summary>
        /// Get/Set value for property TypeID
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Get/Set value for property TypeName
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Get/Set value for property IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get/Set value for property Option
        /// </summary>
        public bool Option { get; set; }

        /// <summary>
        /// Get/Set value for property Mode
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        /// Get/Set value for property IsActive
        /// </summary>
        public bool IsPostBack { get; set; }

        #endregion

        #region Method

        #region Service

        /// <summary>
        /// Change status of a specific service
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>1: Successful. 0: Failed</returns>
        public async Task<int> ChangeServiceStatusAsync(int serviceId)
        {
            int result = 0;

            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_CHANGE_SERVICE_STATUS(serviceId));
            }

            return result;
        }

        /// <summary>
        /// Load list of service base on input conditions
        /// </summary>
        /// <param name="serviceName">Service name</param>
        /// <param name="serviceType">Service type</param>
        /// <param name="isActive">Status</param>
        /// <returns></returns>
        public async Task<List<SP_TAKE_SERVICE_AND_TYPEResult>> LoadListOfService(
            string serviceName, int serviceType, bool isActive)
        {
            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_TAKE_SERVICE_AND_TYPE(serviceName, serviceType, isActive).ToList());
            }
        }

        /// <summary>
        /// Add new service
        /// </summary>
        /// <param name="model">DataModel</param>
        /// <returns>1: Successful, 0: Failed</returns>
        public async Task<int> AddService(DataModel model)
        {
            // Insert new service to database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_INSERT_SERVICE(model.ServiceName, model.TypeID));
            }
        }

        /// <summary>
        /// Load service by ID
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>Service</returns>
        public async Task<Service> LoadSerivceById(int serviceId)
        {
            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from s in data.Services
                     where s.Service_ID.Equals(serviceId)
                     select s).SingleOrDefault());
            }
        }

        /// <summary>
        /// Update service
        /// </summary>
        /// <param name="model">DataModel</param>
        /// <returns>1: Successful, 0: Failed</returns>
        public async Task<int> UpdateService(DataModel model)
        {
            // Update service information
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_UPDATE_SERVICE(model.ServiceID, model.ServiceName, model.TypeID));
            }
        }

        #endregion

        #region Facility

        /// <summary>
        /// Change status of a specific facility
        /// </summary>
        /// <param name="facilityId">Facility ID</param>
        /// <returns>1: Successful. 0: Failed</returns>
        public async Task<int> ChangeFacilityStatusAsync(int facilityId)
        {
            int result = 0;

            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_CHANGE_FACILITY_STATUS(facilityId));
            }

            return result;
        }

        /// <summary>
        /// Load list of facility base on input conditions
        /// </summary>
        /// <param name="facilityName">Facility name</param>
        /// <param name="facilityType">Facility type</param>
        /// <param name="isActive">Status</param>
        /// <returns></returns>
        public async Task<List<SP_TAKE_FACILITY_AND_TYPEResult>> LoadListOfFacility(
            string facilityName, int facilityType, bool isActive)
        {
            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_TAKE_FACILITY_AND_TYPE(facilityName, facilityType, isActive).ToList());
            }
        }

        /// <summary>
        /// Add new facility
        /// </summary>
        /// <param name="model">DataModel</param>
        /// <returns>1: Successful, 0: Failed</returns>
        public async Task<int> AddFacility(DataModel model)
        {
            // Insert new service to database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_INSERT_FACILITY(model.FacilityName, model.TypeID));
            }
        }

        /// <summary>
        /// Load facility by ID
        /// </summary>
        /// <param name="facilityId">Facility ID</param>
        /// <returns>Facility</returns>
        public async Task<Facility> LoadFacilityById(int facilityId)
        {
            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from f in data.Facilities
                     where f.Facility_ID.Equals(facilityId)
                     select f).SingleOrDefault());
            }
        }

        /// <summary>
        /// Update facility
        /// </summary>
        /// <param name="model">DataModel</param>
        /// <returns>1: Successful, 0: Failed</returns>
        public async Task<int> UpdateFacility(DataModel model)
        {
            // Update service information
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_UPDATE_FACILITY(model.FacilityID, model.FacilityName, model.TypeID));
            }
        }

        #endregion

        #region Speciality

        /// <summary>
        /// Change status of a specific speciality
        /// </summary>
        /// <param name="specialityId">Speciality ID</param>
        /// <returns>1: Successful. 0: Failed</returns>
        public async Task<int> ChangeSpecialityStatusAsync(int specialityId)
        {
            int result = 0;

            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_CHANGE_SPECIALITY_STATUS(specialityId));
            }

            return result;
        }

        /// <summary>
        /// Load list of speciality base on input conditions
        /// </summary>
        /// <param name="specialityName">Speciality name</param>
        /// <param name="isActive">Status</param>
        /// <returns></returns>
        public async Task<List<SP_TAKE_SPECIALITY_AND_TYPEResult>> LoadListOfSpeciality(
            string specialityName, bool isActive)
        {
            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_TAKE_SPECIALITY_AND_TYPE(specialityName, isActive).ToList());
            }
        }

        /// <summary>
        /// Add new speciality
        /// </summary>
        /// <param name="model">DataModel</param>
        /// <returns>1: Successful, 0: Failed</returns>
        public async Task<int> AddSpeciality(DataModel model)
        {
            // Insert new service to database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_INSERT_SPECIALITY(model.SpecialityName));
            }
        }

        /// <summary>
        /// Load speciality by ID
        /// </summary>
        /// <param name="specialityId">Speciality ID</param>
        /// <returns>Speciality</returns>
        public async Task<Speciality> LoadSpecialityById(int specialityId)
        {
            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from s in data.Specialities
                     where s.Speciality_ID.Equals(specialityId)
                     select s).SingleOrDefault());
            }
        }

        /// <summary>
        /// Update speciality
        /// </summary>
        /// <param name="model">DataModel</param>
        /// <returns>1: Successful, 0: Failed</returns>
        public async Task<int> UpdateSpeciality(DataModel model)
        {
            // Update service information
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_UPDATE_SPECIALITY(model.SpecialityID, model.SpecialityName));
            }
        }

        #endregion

        #region Disease

        /// <summary>
        /// Load list of disease base on input conditions
        /// </summary>
        /// <param name="diseaseName">Disease name</param>
        /// <param name="isActive">Status</param>
        /// <param name="mode">Mode</param>
        /// <param name="specialityID">Speciality ID</param>
        /// <returns></returns>
        public async Task<List<SP_TAKE_DISEASE_AND_TYPEResult>> LoadListOfDisease(
            string diseaseName, bool isActive, int mode, int specialityID)
        {
            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_TAKE_DISEASE_AND_TYPE(diseaseName, isActive, mode, specialityID).ToList());
            }
        }

        #endregion

        #endregion

        #region Static Method
        public static int TotalUserCount()
        {
            int count = 0;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                count = (from h in data.Users
                         select h.User_ID).Count();
            }
            return count;
        }

        public static int TotalHospitalCount()
        {
            int count = 0;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                count = (from h in data.Hospitals
                         select h.Hospital_ID).Count();
            }
            return count;
        }

        public static int TotalInactiveHospitalCount()
        {
            int count = 0;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                count = (from h in data.Hospitals
                         where h.Is_Active == false
                         select h.Hospital_ID).Count();
            }
            return count;
        }

        public static int TotalMemberHospitalCount()
        {
            int count = 0;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                count = (from h in data.Hospitals
                         from u in data.Users
                         where h.Is_Active == true && h.Hospital_ID == u.Hospital_ID
                         select h.Hospital_ID).Distinct().Count();
            }
            return count;
        }

        public static Hospital BestRatingHospital()
        {
            Hospital hospital = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                hospital = (from h in data.Hospitals
                            where h.Is_Active == true
                            select h).OrderByDescending(h => h.Ratings).FirstOrDefault();
            }
            return hospital;
        }

        public static List<Hospital> TopTenRatingHospital()
        {
            List<Hospital> hospitals = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                hospitals = (from h in data.Hospitals
                            where h.Is_Active == true
                            select h).OrderByDescending(x => x.Rating).Take(10).ToList<Hospital>();
            }
            return hospitals;
        }

        public static Hospital BestRatingCountHospital()
        {
            Hospital hospital = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                hospital = (from h in data.Hospitals
                            where h.Is_Active == true
                            select h).OrderByDescending(h => h.Rating_Count).FirstOrDefault();
            }
            return hospital;
        }

        public static List<Hospital> TopTenHospitalAppointment(DateTime fromDate, DateTime toDate)
        {
            List<Hospital> hospitals = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                hospitals = (from h in data.SP_LOAD_TOP_10_HOSPITAL_APPOINTMENT(fromDate, toDate)
                             select new Hospital()
                             {
                                 Hospital_ID = h.Hospital_ID,
                                 Hospital_Name = h.Hospital_Name,
                                 Rating_Count = h.Appointment_Count
                             }).ToList<Hospital>();
            }
            return hospitals;
        }

        public static Dictionary<string, int> HospitalTypeCount()
        {
            Dictionary<string, int> hospitalTypes = new Dictionary<string, int>();
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                var temp = (from ht in data.SP_LOAD_HOSPITAL_TYPE_COUNT()
                            select new
                            {
                                Type_Name = ht.Type_Name,
                                HospitalType_Count = ht.HospitalType_Count
                            });
                foreach (var t in temp)
                {
                    hospitalTypes.Add(t.Type_Name, (int)t.HospitalType_Count);
                }
            }
            return hospitalTypes;
        }

        public static bool StoreSearchQuery(string searchQuery, int resultCount)
        {
            int check = -1;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                check = data.SP_INSERT_SEARCH_QUERY(searchQuery, resultCount, DateTime.Now);
            }
            return (check >= 0);
        }

        public static List<SentenceDictionaryEntity> SearchQueryStatistic(DateTime fromDate, DateTime toDate)
        {
            List<SentenceDictionaryEntity> sdeList = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                sdeList = (from sd in data.SP_LOAD_SEARCH_QUERY_STATISTIC(fromDate, toDate)
                           select new SentenceDictionaryEntity()
                           {
                               Sentence = sd.Sentence,
                               Search_Time_Count = sd.Search_Time_Count,
                               Result_Count = sd.Result_Count
                           }).ToList<SentenceDictionaryEntity>();
            }
            return sdeList;
        }
        #endregion
    }
}