﻿using System;
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

        #endregion

        #endregion
    }
}