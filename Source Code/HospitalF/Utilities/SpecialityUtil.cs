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
        /// Load all speciality by hospital code
        /// </summary>
        /// <returns>List[SpecialityEntity] that contains a list of speciality</returns>
        public static async Task<List<Speciality>> LoadSpecialityByHospitalIDAsync(int HospitalID)
        {
            List<SP_LOAD_SPECIALITY_BY_HOSPITALIDResult> result = null;
            // Take specalities in a specific hospital in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                data.SP_LOAD_SPECIALITY_BY_HOSPITALID(HospitalID).ToList());
            }

            List<Speciality> specialityList = new List<Speciality>();
            Speciality speciality = null;
            // Assign values for each speciality
            foreach (SP_LOAD_SPECIALITY_BY_HOSPITALIDResult re in result)
            {
                speciality = new Speciality();
                speciality.Speciality_ID = re.Speciality_ID;
                speciality.Speciality_Name = re.Speciality_Name;
                specialityList.Add(speciality);
            }

            // Return list of speciality
            return specialityList;
        }

        /// <summary>
        /// Load all specialities in database
        /// </summary>
        /// <returns>List[Speciality] that contains a list of speciality</returns>
        public static async Task<List<Speciality>> LoadSpecialityAsync()
        {
            // Return list speacialities
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from s in data.Specialities
                     select s).ToList());
            }
        }

        /// <summary>
        /// Load all diseases in database
        /// </summary>
        /// <returns>List[Disease] that contains a list of Diseases</returns>
        public static async Task<List<Disease>> LoadAllDiseaseAsync()
        {
            // Return list of diseases
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from d in data.Diseases
                     select d).ToList());
            }
        }

        /// <summary>
        /// Load list of diseases in a speciality
        /// </summary>
        /// <param name="specialityId">Speciality ID</param>
        /// <returns>List[Disease] that contains a list of Diseases in a Speciality</returns>
        public static async Task<List<Disease>> LoadDiseaseInSpecialityAsync(int specialityId)
        {
            // Return list of diseases
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from d in data.SP_LOAD_DISEASE_IN_SPECIALITY(specialityId)
                     select new Disease()
                     {
                         Disease_ID = d.Disease_ID,
                         Disease_Name = d.Disease_Name
                     }).ToList());
            }
        }
    }
}