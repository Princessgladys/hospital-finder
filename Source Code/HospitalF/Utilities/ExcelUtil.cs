using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HospitalF.Models;
using LinqToExcel;
using System.IO;
using System.Web.Configuration;
using HospitalF.Constant;

namespace HospitalF.Utilities
{
    /// <summary>
    /// Class define methods to handle Excel file
    /// </summary>
    public class ExcelUtil
    {
        /// <summary>
        /// Read data from Excel file
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="file">Input file</param>
        /// <returns>List[HospitalModel] that contains list of hospitals</returns>
        public static List<HospitalModel> LoadDataFromExcel(HttpPostedFileBase file, int userId)
        {
            List<HospitalModel> hospitalList = new List<HospitalModel>();

            // Save file to server
            string fileFullPath = string.Format("{0}{1}",
                WebConfigurationManager.AppSettings[Constants.ExcelFolder],
                FileUtil.SaveFileToServer(file, userId, 0));

            // Create instance of Excel file
            var excelFile = new ExcelQueryFactory();
            excelFile.FileName = fileFullPath;

            // Mapping attributes
            excelFile.AddMapping<HospitalModel>(d => d.HospitalName, Constants.HospitalName);
            excelFile.AddMapping<HospitalModel>(d => d.HospitalTypeID, Constants.TypeID);
            excelFile.AddMapping<HospitalModel>(d => d.HospitalTypeName, Constants.HospitalType);
            excelFile.AddMapping<HospitalModel>(d => d.OrdinaryStartTime, Constants.OrdinaryTime);
            excelFile.AddMapping<HospitalModel>(d => d.HolidayStartTime, Constants.HolidayTime);
            excelFile.AddMapping<HospitalModel>(d => d.IsAllowAppointment, Constants.AppointmentOnline);
            excelFile.AddMapping<HospitalModel>(d => d.AverageCuringTime, Constants.AverageCuringTime);
            excelFile.AddMapping<HospitalModel>(d => d.LocationAddress, Constants.LocationAddress);
            excelFile.AddMapping<HospitalModel>(d => d.StreetAddress, Constants.StreetAddress);
            excelFile.AddMapping<HospitalModel>(d => d.CityID, Constants.CityID);
            excelFile.AddMapping<HospitalModel>(d => d.CityName, Constants.City);
            excelFile.AddMapping<HospitalModel>(d => d.DistrictID, Constants.DistrictID);
            excelFile.AddMapping<HospitalModel>(d => d.DistrictName, Constants.District);
            excelFile.AddMapping<HospitalModel>(d => d.WardID, Constants.WardID);
            excelFile.AddMapping<HospitalModel>(d => d.WardName, Constants.Ward);
            excelFile.AddMapping<HospitalModel>(d => d.PhoneNo, Constants.PhoneNo);
            excelFile.AddMapping<HospitalModel>(d => d.PhoneNo2, Constants.AlternativePhone);
            excelFile.AddMapping<HospitalModel>(d => d.PhoneNo3, Constants.MobilePhone);
            excelFile.AddMapping<HospitalModel>(d => d.Fax, Constants.Fax);
            excelFile.AddMapping<HospitalModel>(d => d.HospitalEmail, Constants.Email);
            excelFile.AddMapping<HospitalModel>(d => d.Website, Constants.Website);
            excelFile.AddMapping<HospitalModel>(d => d.SpecialityName, Constants.Speciality);
            excelFile.AddMapping<HospitalModel>(d => d.ServiceName, Constants.Service);
            excelFile.AddMapping<HospitalModel>(d => d.FacilityName, Constants.Facility);

            // Read data
            var dataList = from data in excelFile.Worksheet<HospitalModel>()
                           select data;
            foreach (var data in dataList)
            {
                HospitalModel model = new HospitalModel();
                
            }

            // Return list of hospitals
            return hospitalList;
        }
    }
}