using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;
using HospitalF.Constant;
using HospitalF.Models;

namespace HospitalF.Utilities
{
    public class GoogleCalendarUtil
    {
        #region User credential
        private static string userName = "anhdth60434@fpt.edu.vn";
        private static string password = "Arizacmt5891@";
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
        /// GoogleCalendarUtil/InsertEntry
        /// </summary>
        /// <param name="appointment">Appointment need to sync to Calendar</param>
        /// <param name="doctor">Doctor of appointment</param>
        /// <param name="speciality">Speciality of appointment</param>
        /// <param name="gmailList">list of hospital user's gmail address string</param>
        public static void InsertEntry(Appointment appointment, Doctor doctor, Speciality speciality, List<string> gmailList)
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
            foreach (string str in gmailList)
            {
                Uri calendarUri = GetCalendarUri(service, str);
                insertedEntry = service.Insert(calendarUri, entry);
            }
        }
        #endregion
    }
}