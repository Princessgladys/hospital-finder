﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using HospitalF.Models;

namespace HospitalF.Utilities
{
    public class SMSUtil
    {
        const string APIKey = "899211D11D10709C5CC8F60BF121A6";//Dang ky tai khoan tai esms.vn de lay key
        const string SecretKey = "470C5EC4CBFF896523B24B637DCFEE";


        /// <summary>
        /// SMSUtil/Send
        /// </summary>
        /// <param name="PhoneNumber">phone number</param>
        /// <param name="HospitalName">hospital name</param>
        /// <param name="confirmCode">confirm code</param>
        public static void Send(string PhoneNumber, string HospitalName, string confirmCode)
        {
            string message = "Ma xac nhan dang ky kham benh tai " + HospitalName + ": " + confirmCode;
            string url = "http://api.esms.vn/MainService.svc/xml/SendMultipleMessage_V2/";
            // declare ascii encoding
            UTF8Encoding encoding = new UTF8Encoding();

            string strResult = string.Empty;

            string customers = "";

            string[] lstPhone = PhoneNumber.Split(',');

            for (int i = 0; i < lstPhone.Count(); i++)
            {
                customers = customers + @"<CUSTOMER>"
                                + "<PHONE>" + lstPhone[i] + "</PHONE>"
                                + "</CUSTOMER>";
            }

            string SampleXml = @"<RQST>"
                               + "<APIKEY>" + APIKey + "</APIKEY>"
                               + "<SECRETKEY>" + SecretKey + "</SECRETKEY>"
                               + "<ISFLASH>0</ISFLASH>"
                               + "<SMSTYPE>3</SMSTYPE>"
                               + "<CONTENT>" + message + "</CONTENT>"
                               + "<CONTACTS>" + customers + "</CONTACTS>"


           + "</RQST>";
            string postData = SampleXml.Trim().ToString();
            // convert xmlstring to byte using ascii encoding
            byte[] data = encoding.GetBytes(postData);
            // declare httpwebrequet wrt url defined above
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            // set method as post
            webrequest.Method = "POST";
            webrequest.Timeout = 500000;
            // set content type
            webrequest.ContentType = "application/x-www-form-urlencoded";
            // set content length
            webrequest.ContentLength = data.Length;
            // get stream data out of webrequest object
            Stream newStream = webrequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // declare & read response from service
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

            // set utf8 encoding
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            // read response stream from response object
            StreamReader loResponseStream =
                new StreamReader(webresponse.GetResponseStream(), enc);
            // read string from stream data
            strResult = loResponseStream.ReadToEnd();
            // close the stream object
            loResponseStream.Close();
            // close the response object
            webresponse.Close();
            // below steps remove unwanted data from response string
            strResult = strResult.Replace("</string>", "");
        }

        #region get random confirm code
        /// <summary>
        /// SMSUtil/GetConfirmCode
        /// </summary>
        /// <returns>confirm code</returns>
        public static string GetConfirmCode()
        {
            string characterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            //The below code will select the random characters from the set
            //and then the array of these characters are passed to string 
            //constructor to make an alphanumeric string
            string randomCode = string.Empty;
            while (ValidateConfirmCode(randomCode) || randomCode == "")
            {
                randomCode = new string(
                 Enumerable.Repeat(characterSet, 8)
                     .Select(set => set[random.Next(set.Length)])
                     .ToArray());
            }
            return randomCode;
        }
        #endregion

        #region check confirm code is exist or not
        /// <summary>
        /// SMSUtil/ValidateConfirmCode
        /// </summary>
        /// <param name="confirmCode">confirm code</param>
        /// <returns>true or false</returns>
        private static bool ValidateConfirmCode(string confirmCode)
        {
            bool isExist = true;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                var result = (from a in data.Appointments
                              where a.Confirm_Code == confirmCode
                              select a).FirstOrDefault();
                if (result == null)
                {
                    isExist = false;
                }
            }
            return isExist;
        }
        #endregion
    }
}