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
    /// Class define methods to handle locations
    /// </summary>
    public class LocationUtil
    {
        /// <summary>
        /// Load a list of citites in database
        /// </summary>
        /// <returns>List[City] that contains a list of cities</returns>
        public static async Task<List<City>> LoadCityAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from c in data.Cities
                     select c).OrderBy(c => c.City_Name).ToList());
            }
        }

        /// <summary>
        /// Load a list of districts in database
        /// </summary>
        /// <returns>List[District] that contains a list of districts</returns>
        public static async Task<List<District>> LoadAllDistrictAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from d in data.Districts
                     select d
                     ).OrderBy(d => d.District_Name).ToList());
            }
        }

        /// <summary>
        /// Load a list of districts in a specific city
        /// </summary>
        /// <param name="cityId">City ID</param>
        /// <returns>List[District] that contains a list of districts in a city</returns>
        public static async Task<List<District>> LoadDistrictInCityAsync(int cityId)
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from d in data.Districts
                     where cityId.Equals(d.City_ID)
                     select d
                     ).OrderBy(d => d.District_Name).ToList());
            }
        }

        /// <summary>
        /// Load a list of ward in a specific district
        /// </summary>
        /// <param name="districtId">District ID</param>
        /// <returns>List[Ward] that contains a list of wards in a district</returns>
        public static async Task<List<Ward>> LoadWardInDistrictAsync(int districtId)
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from w in data.Wards
                     where districtId.Equals(w.District_ID)
                     select w
                     ).OrderBy(w => w.Ward_Name).ToList());
            }
        }

        /// <summary>
        /// Check if there is  similar hospital with name and address
        /// are equal with given data from user
        /// </summary>
        /// <param name="hospitaName">Hospital name</param>
        /// <param name="address">Full address</param>
        /// <returns> 1: Not duplicated, 0: Duplicated</returns>
        public static async Task<int> CheckValidHospitalWithAddress(string hospitaName, string address)
        {
            int result = 0;
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_CHECK_NOT_DUPLICATED_HOSPITAL(hospitaName, address));
            }
            return result;
        }
    }
}