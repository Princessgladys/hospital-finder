using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;
using HospitalF.Constant;
using HospitalF.Models;

namespace HospitalF.Utilities
{
    public class GoogleUtil
    {
        #region User credential
        private static string userName = WebConfigurationManager.AppSettings[Constants.GmailAccount];
        private static string password = WebConfigurationManager.AppSettings[Constants.GmailPassword];
        #endregion

        #region private method
        /// <summary>
        /// GoogleCalendarUtil/GetCalendarService
        /// </summary>
        /// <returns>Calendar service</returns>
        private static CalendarService GetCalendarService()
        {
            CalendarService service = new CalendarService("exampleCo-exampleApp-1");
            service.setUserCredentials(userName, password);
            return service;
        }

        /// <summary>
        /// GoogleCalendarUtil/GetCalendarUri
        /// </summary>
        /// <param name="service">CalendarService</param>
        /// <param name="gmail">Gmail address string</param>
        /// <returns>Uri</returns>
        private static Uri GetCalendarUri(CalendarService service, string gmail)
        {
            EventQuery query = new EventQuery();
            Uri calendarUri = new Uri("https://www.google.com/calendar/feeds/" + gmail + "/private/full");
            query.Uri = calendarUri;

            EventFeed calFeed = service.Query(query);
            return calendarUri;
        }
        #endregion

        #region insert event into calendar
        /// <summary>
        /// GoogleUtil/InsertEventToCalendar
        /// </summary>
        /// <param name="appointment">Appointment need to sync to Calendar</param>
        /// <param name="doctor">Doctor of appointment</param>
        /// <param name="speciality">Speciality of appointment</param>
        /// <param name="gmailList">list of hospital user's gmail address string</param>
        public static async void InsertEventToCalendar(Appointment appointment, Doctor doctor, Speciality speciality, List<string> gmailList)
        {
            EventEntry entry = new EventEntry();
            //title of event
            entry.Title.Text = appointment.Patient_Full_Name;

            //Desrcription of event
            entry.Content.Type = "html";
            string description = string.Empty;
            if (appointment.Health_Insurance_Code != null)
            {
                description = appointment.Symptom_Description + "</br> Mã BHYT" + appointment.Health_Insurance_Code;
            }
            else
            {
                description = appointment.Symptom_Description;
            }
            entry.Content.Content = description;

            //Location of event
            Where eventLocation = new Where();
            string where = string.Empty;
            string doctorName = string.Empty;
            if (doctor != null)
            {
                doctorName = Constants.WhiteSpace + Constants.Minus + Constants.WhiteSpace +
                             doctor.Last_Name + Constants.WhiteSpace + doctor.First_Name;
            }
            eventLocation.ValueString = speciality.Speciality_Name + doctorName;
            entry.Locations.Add(eventLocation);

            //start time of event
            DateTime start = DateTime.Parse(appointment.Appointment_Date.ToString()).Add(TimeSpan.Parse(appointment.Start_Time.ToString()));
            When eventTime = new When();
            eventTime.StartTime = start;

            //end time of event
            DateTime endTime = DateTime.Parse(appointment.Appointment_Date.ToString()).Add(TimeSpan.Parse(appointment.End_Time.ToString()));
            eventTime.EndTime = endTime;
            entry.Times.Add(eventTime);

            //author of event
            AtomPerson author = new AtomPerson(AtomPersonType.Author);
            author.Name = "HospitalF-Admin";
            author.Email = userName;
            entry.Authors.Add(author);


            //add event to google calendar
            CalendarService service = GetCalendarService();
            AtomEntry insertedEntry;
            foreach (string gmail in gmailList)
            {
                try
                {
                    Uri calendarUri = GetCalendarUri(service, gmail);
                    insertedEntry = service.Insert(calendarUri, entry);
                }
                catch (Exception ex)
                {
                    LoggingUtil.LogException(ex);
                    SendEmailToRemind(gmail);
                }
            }
        }
        #endregion

        #region send email

        #region send email to remind share calendar

        /// <summary>
        /// GoogleUtil/SendEmailToRemind
        /// </summary>
        /// <param name="receiveGMail"></param>
        private static async void SendEmailToRemind(string receiveGMail)
        {
            // Declare SmtpClient variable
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            // Declare NetworkCredential variable
            NetworkCredential loginInfo = new NetworkCredential(userName, password);
            // Declare MailMessage variable
            MailMessage msg = new MailMessage();

            // Assign value for MailMessage object
            msg.From = new MailAddress(userName);
            msg.To.Add(new MailAddress(receiveGMail));
            msg.Subject = "[HospitalF] Remind";
            string mailContent = "Hello " + receiveGMail + ",<br/>" +
                "We are writing to let you know that we cannot access to add events on the Google Calendar." +
                "<br/>" + "Check again to make sure that you had given " + userName + " access to add events on your calendar.<br/> Thanks";
            msg.Body = mailContent;
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

        #region send email to report updated content

        /// <summary>
        /// GoogleUtil/SendEmailToAdmin
        /// </summary>
        /// <param name="senderGmail">Gmail of hospital user</param>
        /// <param name="receiveGmailList">List of Gmail of admins</param>
        /// <param name="hospitalName">Hospital was updated</param>
        /// <param name="updatedContent">Updated contents</param>
        public static async void SendEmailToAdmin(string senderGmail, List<string> receiveGmailList, string hospitalName, string updatedContent)
        {
            // Declare SmtpClient variable
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            // Declare NetworkCredential variable
            NetworkCredential loginInfo = new NetworkCredential(userName, password);
            // Declare MailMessage variable
            MailMessage msg = new MailMessage();

            // Assign value for MailMessage object
            msg.From = new MailAddress(senderGmail);
            foreach (string receiveGmail in receiveGmailList)
            {
                msg.To.Add(new MailAddress(receiveGmail));
            }
            msg.Subject = "[HospitalF][" + hospitalName + "] Update information";
            string mailContent = "Hello HospitalF-Admin,<br/>We are writing to let you know that " + senderGmail + " has updated:<br/>";
            string[] content = updatedContent.Split(Char.Parse(Constants.Comma));
            for (int i = 0; i < content.Length; i++)
            {
                if (i == (content.Length - 1) && content!=null)
                {
                    mailContent = mailContent + (i + 1) + ". " + content+".";
                }
                else
                {
                    mailContent = mailContent + "  " + (i + 1) + ". " + content + "</br>";
                }
            }
            msg.Body = mailContent;
            msg.IsBodyHtml = true;

            // Assign value for SmtpClient object
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = loginInfo;

            // Send email
            client.Send(msg);
        }
        
        /// <summary>
        /// GoogleUtil/SendEmailToHospitalUser
        /// </summary>
        /// <param name="senderGmail">Gmail of Admin</param>
        /// <param name="receiveGmailList">List of Gmail of hospital users</param>
        /// <param name="hospitalName">Hospital was updated</param>
        /// <param name="updatedContent">Updated contents</param>
        public static async void SendEmailToHospitalUser(string senderGmail, List<string> receiveGmailList, string hospitalName, string updatedContent)
        {
            // Declare SmtpClient variable
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            // Declare NetworkCredential variable
            NetworkCredential loginInfo = new NetworkCredential(userName, password);
            // Declare MailMessage variable
            MailMessage msg = new MailMessage();

            // Assign value for MailMessage object
            msg.From = new MailAddress(senderGmail);
            foreach (string receiveGmail in receiveGmailList)
            {
                msg.To.Add(new MailAddress(receiveGmail));
            }
            msg.Subject = "[HospitalF][" + hospitalName + "] Update information";
            string mailContent = "Hello "+hospitalName+"-Staff ,<br/>We are writing to let you know that HospitalF-Admin has updated:<br/>";
            string[] content = updatedContent.Split(',');
            for (int i = 0; i < content.Length; i++)
            {
                if (i == content.Length - 1)
                {
                    mailContent = mailContent + (i + 1) + ". " + content + ".</br> Please, check updated information again to make sure that updated contents are valid.<br/>Thanks";
                }
                else
                {
                    mailContent = mailContent + "  " + (i + 1) + ". " + content + "</br>";
                }
            }
            msg.Body = mailContent;
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

        #endregion
    }
}