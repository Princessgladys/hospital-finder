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
        public static async Task<List<Service>> LoadServiceAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from s in data.Services
                     select s).OrderBy(s => s.Service_Name).ToList());
            }
        }

        /// <summary>
        /// Load a list of facilities in database
        /// </summary>
        /// <returns>List[Facility] that contains a list of services</returns>
        public static async Task<List<Facility>> LoadFacilityAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from f in data.Facilities
                     select f).OrderBy(f => f.Facility_Name).ToList());
            }
        }
    }
}