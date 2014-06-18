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
    /// Class defines methods for to handle Speciality and Disease
    /// </summary>
    public class SpecialityUtil
    {
        /// <summary>
        /// Load all specialities in database
        /// </summary>
        /// <returns>List[SpecialityEntity] that contains a list of speciality</returns>
        public static async Task<List<SpecialityEntity>> LoadSpecialityAsync()
        {
            // Return list speacialities
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from s in data.Specialities
                     select new SpecialityEntity
                     {
                         SpecialityID = s.Speciality_ID,
                         SpecialityName = s.Speciality_Name
                     }).ToList());
            }
        }

        /// <summary>
        /// Load all diseases in database
        /// </summary>
        /// <returns>List[DiseaseEntity] that contains a list of Diseases</returns>
        public static async Task<List<DiseaseEntity>> LoadDiseaseAsync()
        {
            // Return list of diseases
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from d in data.Diseases
                     select new DiseaseEntity
                     {
                         DiseaseID = d.Disease_ID,
                         DiseaseName = d.Disease_Name
                     }).ToList());
            }
        }
    }
}