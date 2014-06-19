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
        public static async Task<List<DiseaseEntity>> LoadAllDiseaseAsync()
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

        /// <summary>
        /// Load list of diseases in a speciality
        /// </summary>
        /// <param name="specialityId">Speciality ID</param>
        /// <returns>List[DiseaseEntity] that contains a list of Diseases in a Speciality</returns>
        public static async Task<List<DiseaseEntity>> LoadDiseaseInSpecialityAsync(int specialityId)
        {
            List<SP_LOAD_DISEASE_IN_SPECIALITYResult> result = null;
            // Take diseases in a specific speciality in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                data.SP_LOAD_DISEASE_IN_SPECIALITY(specialityId).ToList());
            }

            List<DiseaseEntity> diseaseList = null;
            // Assign values for each hospital
            foreach (SP_LOAD_DISEASE_IN_SPECIALITYResult re in result)
            {
                DiseaseEntity disease = new DiseaseEntity();
                disease.DiseaseID = re.Disease_ID;
                disease.DiseaseName = re.Disease_Name;
                diseaseList.Add(disease);
            }

            // Return list of diseases
            return diseaseList;
        }
    }
}