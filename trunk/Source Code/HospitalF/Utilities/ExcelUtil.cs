using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HospitalF.Models;
using LinqToExcel;
using System.IO;

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
        /// <param name="file">Input file</param>
        /// <returns>List[HospitalModel] that contains list of hospitals</returns>
        public static List<HospitalModel> LoadDataFrom(HttpPostedFileBase file)
        {
            // Create new list of hospitals
            List<HospitalModel> hospitalList = new List<HospitalModel>();

            // Create instance of Excel file
            var excelFile = new ExcelQueryFactory();
            string a = Path.GetFileName(file.FileName);
            excelFile.FileName = Path.GetFullPath(file.FileName);
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