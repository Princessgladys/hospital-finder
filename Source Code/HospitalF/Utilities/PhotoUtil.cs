﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Models;
using HospitalF.Constant;
using System.IO;

namespace HospitalF.Utilities
{
    /// <summary>
    /// Class define methods to handle photo
    /// </summary>
    public class PhotoUtil
    {
        /// <summary>
        /// Save picture to folder in database
        /// </summary>
        /// <param name="file">Picture file</param>
        /// <param name="action">Current action - method</param>
        /// <param name="filePath">File path</param>
        /// <param name="userId">User ID</param>
        /// <returns>File name</returns>
        public static string SaveImageToServer(HttpPostedFileBase file, string action, string filePath, int userId)
        {
            // Check if input file is null
            if (file == null)
            {
                return null;
            }

            // Handle name of picture to avoid duplicated
            var fileName = HandlePictureFileName(file, filePath, userId);
            // Save the picture to the PersonalPicture folder which located on server
            var path = Path.Combine(filePath, fileName);
            file.SaveAs(path);

            // Return file name
            return fileName;
        }

        /// <summary>
        /// Rename picture's file name according to action, username and handle file in case of duplicated name
        /// </summary>
        /// <param name="file">Uploaded filed</param>
        /// <param name="filePath">Filepath</param>
        /// <param name="userId">User ID</param>
        /// <returns>Filename after modified</returns>
        private static string HandlePictureFileName(HttpPostedFileBase file, string filePath, int userId)
        {
            // Declare variable to take file name
            string fileName = String.Empty;
            // Declare variable to take file name after edited
            string newFileName = String.Empty;
            // Declare variable to store full file path of a file
            string fullPath = String.Empty;
            // Declare variable dedicate a file has been existed or not
            int count = 0;
            // Get file name
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);

            // Set file
            fileName = String.Format("{0}{1}{2}", userId, Constants.Minus, fileNameWithoutExtension);
            // Check if new name is duplicated
            fullPath = string.Format("{0}{1}{2}{3}{4}{5}{6}", filePath, Constants.DoubleReverseSlash,
                fileName, Constants.OpenBracket, count, Constants.CloseBracket, extension);

            for (bool isExisted = true; isExisted == true;)
            {
                if (File.Exists(fullPath))
                {
                    count += 1;
                    fullPath = string.Format("{0}{1}{2}{3}{4}{5}{6}", filePath,
                        Constants.DoubleReverseSlash, fileName, Constants.OpenBracket,
                        count, Constants.CloseBracket, extension);
                }
                else
                {
                    newFileName = String.Format("{0}{1}{2}{3}", fileName,
                        Constants.OpenBracket, count, Constants.CloseBracket);
                    isExisted = false;
                }
            }

            // Return new file name
            return newFileName + extension;
        }
    }
}