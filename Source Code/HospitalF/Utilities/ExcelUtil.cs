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
            excelFile.AddMapping<HospitalModel>(d => d.HospitalName, "FirstName");

            // Read data
            var dataList = from data in excelFile.Worksheet()
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