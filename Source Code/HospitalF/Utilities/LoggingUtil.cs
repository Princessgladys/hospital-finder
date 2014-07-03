using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.IO;
using HospitalF.Constant;

namespace HospitalF.Utilities
{
    /// <summary>
    /// Class define methods for logging errors and exceptions
    /// </summary>
    public class LoggingUtil
    {
        public static void LogException(Exception exception)
        {
            // Read configutation detail from Web.config
            string partitionPath = WebConfigurationManager.
                AppSettings[Constants.LoggingPartition];

            // Assign default path if partitionPath is null
            if (partitionPath == null)
            {
                partitionPath = Constants.DefaultLoggingPartition;
            }

            // Check if logging folder is existed or not
            if (!Directory.Exists(partitionPath))
            {
                Directory.CreateDirectory(partitionPath);
            }

            // Handle name of today logging file
            string todayDate = string.Format("{0}{1}{2}{3}{4}",
                DateTime.Now.Day.ToString(Constants.DayMonthWith2Number),
                Constants.Minus, DateTime.Now.Month.ToString(Constants.DayMonthWith2Number),
                Constants.Minus, DateTime.Now.Year.ToString());
            string todayLogFile = string.Format("{0}{1}{2}{3}", partitionPath,
                Constants.DoubleReverseSlash, todayDate, Constants.TxtFile);

            StreamWriter file = null;
            // Check if today logging file is existed or not
            if (!File.Exists(todayLogFile))
            {
                File.Create(todayLogFile).Close();
                using (file = new StreamWriter(todayLogFile, true))
                {
                    file.WriteLine(String.Format(Constants.OpenLogFileStatement, todayDate));
                }
            }

            // Write log detail to file
            using (file = new StreamWriter(todayLogFile, true))
            {
                file.WriteLine(string.Format(Constants.LogFileFormat, DateTime.Now,
                    exception.StackTrace.Trim(), exception.Message.Trim()));
            }
        }
    }
}