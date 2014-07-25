using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Models;
using HospitalF.Entities;

namespace HospitalF.Utilities
{
    /// <summary>
    /// Class define methods to handle hospital services and facilities
    /// </summary>
    public class ServiceFacilityUtil
    {
        /// <summary>
        /// Load a list of services in database
        /// </summary>
        /// <returns>List[Service] that contains a list of services</returns>
        public static async Task<IEnumerable<GroupedSelectListItem>> LoadServiceAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from s in data.SP_TAKE_SERVICE_AND_TYPE().ToList()
                     select new GroupedSelectListItem
                     {
                         GroupKey = s.Type_ID.ToString(),
                         GroupName = s.Type_Name,
                         Text = s.Service_Name,
                         Value = s.Service_ID.ToString()
                     }));
            }
        }

        /// <summary>
        /// Load a list of facilities in database
        /// </summary>
        /// <returns>List[Facility] that contains a list of services</returns>
        public static async Task<IEnumerable<GroupedSelectListItem>> LoadFacilityAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from f in data.SP_TAKE_FACILITY_AND_TYPE().ToList()
                     select new GroupedSelectListItem
                     {
                         GroupKey = f.Type_ID.ToString(),
                         GroupName = f.Type_Name,
                         Text = f.Facility_Name,
                         Value = f.Facility_ID.ToString()
                     }));
            }
        }
        /// <summary>
        /// Load list of service of hospital
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public static async Task<List<ServiceEntity>> LoadServiceOfHospitalAsync(int hospitalID)
        {
            List<ServiceEntity> serviceList = new List<ServiceEntity>();
            List<SP_LOAD_SERVICE_IN_HOSPITAL_SERVICEResult> result;
            ServiceEntity service = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result= await Task.Run(() => data.SP_LOAD_SERVICE_IN_HOSPITAL_SERVICE(hospitalID).ToList());
                foreach (SP_LOAD_SERVICE_IN_HOSPITAL_SERVICEResult r in result)
                {
                    service = new ServiceEntity();
                    service.Service_ID = r.Service_ID;
                    service.Service_Name = r.Service_Name;
                    service.Type_ID = r.Service_Type;
                    service.Type_Name = r.Type_Name;
                    serviceList.Add(service);
                }
            }
            return serviceList;
        }

        /// <summary>
        /// Load list of facility of hospital
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public static async Task<List<FacilityEntity>> LoadFacilityOfHospitalAsync(int hospitalID)
        {
            List<FacilityEntity> facilityList = new List<FacilityEntity>();
            List<SP_LOAD_FACILITY_IN_HOSPITAL_FACILITYResult> result;
            FacilityEntity facility = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_LOAD_FACILITY_IN_HOSPITAL_FACILITY(hospitalID).ToList());
                foreach (SP_LOAD_FACILITY_IN_HOSPITAL_FACILITYResult r in result)
                {
                    facility = new FacilityEntity();
                    facility.Facility_ID = r.Facility_ID;
                    facility.Facility_Name = r.Facility_Name;
                    facility.Type_ID = r.Facility_Type;
                    facility.Type_Name = r.Type_Name;
                    facilityList.Add(facility);
                }
            }
            return facilityList;
        }
    }
}