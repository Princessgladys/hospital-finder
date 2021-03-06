﻿using System;
using System.Collections.Generic;
using HospitalF.Models;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Hospital table
    /// <summary>
    public class HospitalEntity
    {
        #region Hospital Properties

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int Hospital_ID { get; set; }

        /// <summary>
        /// Property for Hospital_Name attribute
        /// <summary>
        public string Hospital_Name { get; set; }

        /// <summary>
        /// Property for Hospital_Type attribute
        /// <summary>
        public int? Hospital_Type { get; set; }

        /// <summary>
        /// Property for Hospital_Type_Name attribute
        /// <summary>
        public string Hospital_Type_Name { get; set; }

        /// <summary>
        /// Property for Address attribute
        /// <summary>
        public string Address { get; set; }

        /// <summary>
        /// Property for Ward_ID attribute
        /// <summary>
        public int? Ward_ID { get; set; }

        /// <summary>
        /// Property for Ward_Name attribute
        /// <summary>
        public string Ward_Name { get; set; }

        /// <summary>
        /// Property for District_ID attribute
        /// <summary>
        public int? District_ID { get; set; }

        /// <summary>
        /// Property for District_Name attribute
        /// <summary>
        public string District_Name { get; set; }

        /// <summary>
        /// Property for City_ID attribute
        /// <summary>
        public int? City_ID { get; set; }

        /// <summary>
        /// Property for City_Name attribute
        /// <summary>
        public string City_Name { get; set; }

        /// <summary>
        /// Property for Speciality_ID attribute
        /// <summary>
        public int? Speciality_ID { get; set; }

        /// <summary>
        /// Property for Speciality_Name attribute
        /// <summary>
        public string Speciality_Name { get; set; }

        /// <summary>
        /// Property for Phone_Number attribute
        /// <summary>
        public string Phone_Number { get; set; }

        /// <summary>
        /// Property for Fax attribute
        /// <summary>
        public string Fax { get; set; }

        /// <summary>
        /// Property for Website attribute
        /// <summary>
        public string Website { get; set; }

        /// <summary>
        /// Property for Email attribute
        /// <summary>
        public string Email { get; set; }

        /// <summary>
        /// Property for Coordinate attribute
        /// <summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// Property for Distance attribute
        /// <summary>
        public double Distance { get; set; }

        /// <summary>
        /// Property for Description attribute
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Property for Ordinary_Start_Time attribute
        /// </summary>
        public TimeSpan? Ordinary_Start_Time { get; set; }

        /// <summary>
        /// Property for Ordinary_End_Time attribute
        /// </summary>
        public TimeSpan? Ordinary_End_Time { get; set; }

        /// <summary>
        /// Property for Holiday_Start_Time attribute
        /// </summary>
        public TimeSpan? Holiday_Start_Time { get; set; }

        /// <summary>
        /// Property for Holiday_End_Time attribute
        /// </summary>
        public TimeSpan? Holiday_End_Time { get; set; }

        /// <summary>
        /// Property for Is_Allow_Appointment attribute
        /// </summary>
        public bool? Is_Allow_Appointment { get; set; }

        /// <summary>
        /// Property for Rating attribute
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Property for Rating_Count attribute
        /// </summary>
        public int? Rating_Count { get; set; }

        /// <summary>
        /// Property for Services attribute
        /// </summary>
        public List<ServiceEntity> Services { get; set; }

        /// <summary>
        /// Property for Facilities attribute
        /// </summary>
        public List<FacilityEntity> Facilities { get; set; }

        /// <summary>
        /// Property for Specialities attribute
        /// </summary>
        public List<Speciality> Specialities { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// </summary>
        public bool? Is_Active { get; set; }

        #endregion
    }
}
