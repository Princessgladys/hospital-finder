using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Constant;

namespace HospitalF.Models
{
    /// <summary>
    /// Class define bussiness method for DataModel
    /// </summary>
    public class DataModel
    {
        #region Properties

        /// <summary>
        /// Get/Set value for property HospitalID
        /// </summary>
        public int ServiceID { get; set; }

        /// <summary>
        /// Get/Set value for property ServiceName
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Get/Set value for property FacilityID
        /// </summary>
        public int FacilityID { get; set; }

        /// <summary>
        /// Get/Set value for property FacilityName
        /// </summary>
        public string FacilityName { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityID
        /// </summary>
        public int SpecialityID { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityName
        /// </summary>
        public string SpecialityName { get; set; }

        /// <summary>
        /// Get/Set value for property IsMainSpeciality
        /// </summary>
        public bool IsMainSpeciality { get; set; }

        /// <summary>
        /// Get/Set value for property TypeID
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Get/Set value for property TypeName
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Get/Set value for property IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get/Set value for property IsActive
        /// </summary>
        public bool IsPostBack { get; set; }

        #endregion

        #region Method

        #region Service

        /// <summary>
        /// Change status of a specific service
        /// </summary>
        /// <param name="serviceId">Service ID</param>
        /// <returns>1: Successful. 0: Failed</returns>
        public async Task<int> ChangeServiceStatusAsync(int serviceId)
        {
            int result = 0;

            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_CHANGE_SERVICE_STATUS(serviceId));
            }

            return result;
        }

        /// <summary>
        /// Load list of service base on input conditions
        /// </summary>
        /// <param name="serviceName">Service name</param>
        /// <param name="serviceType">Service type</param>
        /// <param name="isActive">Status</param>
        /// <returns></returns>
        public async Task<List<SP_TAKE_SERVICE_AND_TYPEResult>> LoadListOfService(
            string serviceName, int serviceType, bool isActive)
        {
            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_TAKE_SERVICE_AND_TYPE(serviceName, serviceType, isActive).ToList());
            }
        }

        /// <summary>
        /// Add new service
        /// </summary>
        /// <param name="model">DataModel</param>
        /// <returns>1: Successful, 0: Failed</returns>
        public async Task<int> AddService(DataModel model)
        {
            // Insert new service to database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_INSERT_SERVICE(model.ServiceName, model.TypeID));
            }
        }

        #endregion

        #region Facility

        /// <summary>
        /// Change status of a specific facility
        /// </summary>
        /// <param name="facilityId">Facility ID</param>
        /// <returns>1: Successful. 0: Failed</returns>
        public async Task<int> ChangeFacilityStatusAsync(int facilityId)
        {
            int result = 0;

            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_CHANGE_FACILITY_STATUS(facilityId));
            }

            return result;
        }

        /// <summary>
        /// Load list of facility base on input conditions
        /// </summary>
        /// <param name="facilityName">Facility name</param>
        /// <param name="facilityType">Facility type</param>
        /// <param name="isActive">Status</param>
        /// <returns></returns>
        public async Task<List<SP_TAKE_FACILITY_AND_TYPEResult>> LoadListOfFacility(
            string facilityName, int facilityType, bool isActive)
        {
            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_TAKE_FACILITY_AND_TYPE(facilityName, facilityType, isActive).ToList());
            }
        }

        #endregion

        #region Speciality

        /// <summary>
        /// Change status of a specific speciality
        /// </summary>
        /// <param name="specialityId">Speciality ID</param>
        /// <returns>1: Successful. 0: Failed</returns>
        public async Task<int> ChangeSpecialityStatusAsync(int specialityId)
        {
            int result = 0;

            // Change service status
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() =>
                    data.SP_CHANGE_SPECIALITY_STATUS(specialityId));
            }

            return result;
        }

        /// <summary>
        /// Load list of speciality base on input conditions
        /// </summary>
        /// <param name="specialityName">Speciality name</param>
        /// <param name="isActive">Status</param>
        /// <returns></returns>
        public async Task<List<SP_TAKE_SPECIALITY_AND_TYPEResult>> LoadListOfSpeciality(
            string specialityName, bool isActive)
        {
            // Search for suitable hospitals in database
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    data.SP_TAKE_SPECIALITY_AND_TYPE(specialityName, isActive).ToList());
            }
        }

        #endregion

        #endregion
    }
}