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
    }
}