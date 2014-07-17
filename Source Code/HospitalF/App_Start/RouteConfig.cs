using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HospitalF
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute("GetDistrictByCity",
                            "home/getdistrictbycity/",
                            new { controller = "Home", action = "GetDistrictByCity" },
                            new[] { "HospitalF.Controllers" }
            );

            routes.MapRoute("GetDeseaseBySpeciality",
                            "home/getdeseasebyspeciality/",
                            new { controller = "Home", action = "GetDeseaseBySpeciality" },
                            new[] { "HospitalF.Controllers" }
            );

            routes.MapRoute("LoadSuggestSentence",
                            "home/loadsuggestsentence/",
                            new { controller = "Home", action = "LoadSuggestSentence" },
                            new[] { "HospitalF.Controllers" }
            );

            routes.MapRoute("GetDoctorBySpeciality",
                            "appointment/getdoctorbyspeciality/",
                            new { controller = "Appointment", action = "GetDoctorBySpeciality" },
                            new[] { "HospitalF.Controllers" }
            );

            routes.MapRoute("GetWorkingDay", 
                            "appointment/getworkingday/", 
                            new { controller = "Appointment", action = "GetWorkingDay" },
                            new[]{"HospitalF.Controllers"}
           );

            routes.MapRoute("SearchDoctor",
                            "hospital/searchdoctor/",
                            new { controller = "Hospital", action = "SearchDoctor" },
                            new[]{"HospitalF.Controllers"});

            routes.MapRoute("DisplayHospitalList",
                            "hospital/displayhospitallist/",
                            new { controller = "Hospital", action = "DisplayHospitalList" },
                            new[] { "HospitalF.Controllers" });
        }
    }
}