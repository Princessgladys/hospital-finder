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
using System.Threading.Tasks;

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
        public static async Task<List<HospitalModel>> LoadDataFromExcel(HttpPostedFileBase file, int userId)
        {
            List<HospitalModel> hospitalList = new List<HospitalModel>();
            HospitalModel model = null;
            int tempInt = 0;
            bool tempBoolean = false;
            int count = 1;

            // Save file to server
            string fileFullPath = string.Format("{0}{1}",
                WebConfigurationManager.AppSettings[Constants.ExcelFolder],
                FileUtil.SaveFileToServer(file, userId, 0));

            // Create instance of Excel file
            var excelFile = new ExcelQueryFactory(fileFullPath);

            // Read data
            var dataList = await Task.Run(() => 
                from data in excelFile.Worksheet(Constants.TemplateSheet)
                select data);

            foreach (var data in dataList)
            {
                model = new HospitalModel();

                // Take data from file
                model.HospitalName = data[0];
                if (string.IsNullOrEmpty(data[0]))
                {
                    model.RecordStatus = 0;
                    model.HospitalID = count;
                    model.HospitalName = null;
                }
                else
                {
                    Int32.TryParse(data[24], out tempInt);
                    model.HospitalTypeID = tempInt;
                    model.HospitalTypeName = data[1];
                    model.OrdinaryStartTime = data[2];
                    model.HolidayStartTime = data[3];
                    Boolean.TryParse(data[4], out tempBoolean);
                    model.IsAllowAppointment = tempBoolean;
                    Int32.TryParse(data[5], out tempInt);
                    model.AverageCuringTime = tempInt;
                    model.LocationAddress = data[6];
                    model.StreetAddress = data[7];
                    Int32.TryParse(data[21], out tempInt);
                    model.CityID = tempInt;
                    model.CityName = data[8];
                    Int32.TryParse(data[22], out tempInt);
                    model.DistrictID = tempInt;
                    model.DistrictName = data[9];
                    Int32.TryParse(data[23], out tempInt);
                    model.WardID = tempInt;
                    model.WardName = data[10];
                    model.FullAddress = string.Format("{0} {1}, {2}, {3}, {4}", model.LocationAddress,
                        model.StreetAddress, model.WardName, model.DistrictName, model.CityName);
                    model.PhoneNo = data[11];
                    model.PhoneNo2 = data[12];
                    model.PhoneNo3 = data[13];
                    model.Fax = data[14];
                    model.HospitalEmail = data[15];
                    model.Website = data[16];
                    model.SpecialityName = data[17];
                    model.ServiceName = data[18];
                    model.FacilityName = data[19];
                    model.TagsInput = data[20];
                    model.RecordStatus = 1;
                    model.HospitalID = count;
                }

                count++;
                // Add to hospital list
                hospitalList.Add(model);
            }

            // Return list of hospitals
            return hospitalList;
        }
    }
}