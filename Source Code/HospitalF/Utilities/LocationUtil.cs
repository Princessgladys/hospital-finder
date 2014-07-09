﻿using System;
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
    }
}