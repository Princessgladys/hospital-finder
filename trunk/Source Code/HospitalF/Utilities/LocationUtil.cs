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
        private async Task<List<CityEntity>> LoadCityAsync()
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
    }
}