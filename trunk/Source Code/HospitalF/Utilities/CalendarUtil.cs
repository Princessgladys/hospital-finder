using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;
using HospitalF.Models;

namespace HospitalF.Utilities
{
    public class CalendarUtil
    {
        private static string userName = "anhdth60434@fpt.edu.vn";
        private static string password = "Arizacmt5891@";
        private static Uri calendarUri = new Uri("https://www.google.com/calendar/feeds/" + userName + "/private/full");
        private static Google.GData.Client.Service service;
        private static void GetService()
        {
            FeedQuery query = new FeedQuery();
            service = new CalendarService("exampleCo-exampleApp-1");

            service.setUserCredentials(userName, password);

            query.Uri = calendarUri;

            AtomFeed calFeed = service.Query(query);
        }
        public static void InsertEntry(Appointment appointment, Doctor doctor, Speciality speciality)
        {
            GetService();
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
            eventLocation.ValueString = speciality.Speciality_Name + " -  Bs." + doctor.Last_Name + " " + doctor.First_Name;
            entry.Locations.Add(eventLocation);

            //start time of event
            DateTime start = DateTime.Parse(appointment.Appointment_Date.ToString() + "" + appointment.Start_Time);
            When eventTime = new When();
            eventTime.StartTime = start;
            //end time of event
            DateTime endTime = DateTime.Parse(appointment.Appointment_Date.ToString() + "" + appointment.End_Time);
            eventTime.EndTime = endTime;
            entry.Times.Add(eventTime);

            //author of event
            AtomPerson author = new AtomPerson(AtomPersonType.Author);
            author.Name = "Nuna_pumkin";
            author.Email = userName;
            entry.Authors.Add(author);


            //add event to google calendar
            AtomEntry insertedEntry = service.Insert(calendarUri, entry);
        }
    }
}