using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Models;
using HospitalF.Entities;
using HospitalF.Constant;

namespace HospitalF.Utilities
{
    public class LocationUtil
    {
        /// <summary>
        /// Load a list of citites in database
        /// </summary>
        /// <returns>List[CityEntity] that contains a list of cities</returns>
        public static async Task<List<CityEntity>> LoadCityAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from c in data.Cities
                     select new CityEntity
                     {
                         CityID = c.City_ID,
                         CityName = c.City_Name,
                     }).OrderBy(c => c.CityName).ToList());
            }
        }

        /// <summary>
        /// Load a list of districts in database
        /// </summary>
        /// <returns>List[DistrictEntity] that contains a list of districts</returns>
        public static async Task<List<DistrictEntity>> LoadAllDistrictAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from d in data.Districts
                     select new DistrictEntity
                     {
                         DistrictID = d.District_ID,
                         DistrictName = d.District_Name,
                     }).OrderBy(d => d.DistrictName).ToList());
            }
        }

        /// <summary>
        /// Load a list of districts in a specific city
        /// </summary>
        /// <param name="cityId">City ID</param>
        /// <returns>List[DistrictEntity] that contains a list of districts in a city</returns>
        public static async Task<List<DistrictEntity>> LoadDistrictInCityAsync(int cityId)
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from d in data.Districts
                     where cityId.Equals(d.City_ID)
                     select new DistrictEntity
                     {
                         DistrictID = d.District_ID,
                         DistrictName = d.District_Name,
                     }).OrderBy(d => d.DistrictName).ToList());
            }
        }
    }
}