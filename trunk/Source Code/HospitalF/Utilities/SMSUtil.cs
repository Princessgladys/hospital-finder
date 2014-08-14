using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using HospitalF.Models;
using HospitalF.Constant;

namespace HospitalF.Utilities
{
    public class SMSUtil
    {
        private static string APIKey = WebConfigurationManager.AppSettings[Constants.ESMSAPIKey];//Dang ky tai khoan tai esms.vn de lay key
        private static string SecretKey = WebConfigurationManager.AppSettings[Constants.ESMSSecretKey];


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

        #region send email
        /// <summary>
        /// Handling sending an inform email to user
        /// </summary>
        /// <param name="senderEmail">Sender email address [From]</param>
        /// <param name="senderEmailPassword">Sender email password</param>
        /// <param name="subject">Subject of the email [Subject]</param>
        /// <param name="content">Content of the email [Content]</param>
        /// <param name="receiverEmail">Receiver email address [Tp]</param>
        /// <param name="provider">Email service [Google][Yahoo][HotMail]...</param>
        /// <returns>SmtpClient object</returns>
        public static void SendEmailUsingGmail(string senderEmail, string senderEmailPassword, string subject,
            string content, string receiverEmail, string provider)
        {
            // Declare SmtpClient variable
            SmtpClient client = new SmtpClient(provider, 587);
            // Declare NetworkCredential variable
            NetworkCredential loginInfo = new NetworkCredential(senderEmail, senderEmailPassword);
            // Declare MailMessage variable
            MailMessage msg = new MailMessage();

            // Assign value for MailMessage object
            msg.From = new MailAddress(senderEmail);
            msg.To.Add(new MailAddress(receiverEmail));
            msg.Subject = subject;
            msg.Body = content;
            msg.IsBodyHtml = true;

            // Assign value for SmtpClient object
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = loginInfo;

            // Send email
            client.Send(msg);
        }
        #endregion
    }
}