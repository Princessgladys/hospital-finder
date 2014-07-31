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

            routes.MapRoute("GetDistrictByCity-Hospital",
                            "hospital/getdistrictbycity/",
                            new { controller = "Hospital", action = "GetDistrictByCity" },
                            new[] { "HospitalF.Controllers" }
            );

            routes.MapRoute("ChangeHospitalStatus",
                            "hospital/changehospitalstatus/",
                            new { controller = "Hospital", action = "ChangeHospitalStatus" },
                            new[] { "HospitalF.Controllers" }
            );

            routes.MapRoute("GetWardByDistritct",
                            "hospital/getwardbydistritct/",
                            new { controller = "Hospital", action = "GetWardByDistritct" },
                            new[] { "HospitalF.Controllers" }
            );

            routes.MapRoute("HospitalBasicInforUpdate",
                            "hospital/hospitalbasicinforupdate/",
                            new { controller = "Hospital", action = "HospitalBasicInforUpdate" },
                            new[] { "HospitalF.Controllers" });

            routes.MapRoute("CheckValidHospitalWithAddress",
                            "hospital/checkvalidhospitalwithaddress/",
                            new { controller = "Hospital", action = "CheckValidHospitalWithAddress" },
                            new[] { "HospitalF.Controllers" });

            routes.MapRoute("CheckValidUserWithEmail",
                            "hospital/checkvaliduserwithemail/",
                            new { controller = "Hospital", action = "CheckValidUserWithEmail" },
                            new[] { "HospitalF.Controllers" });

            routes.MapRoute("AddAccount",
                            "account/addcccount/",
                            new { controller = "Account", action = "AddAccount" },
                            new[] { "HospitalF.Controllers" });

            routes.MapRoute("ChangeServiceStatus",
                            "data/changeservicestatus/",
                            new { controller = "Data", action = "ChangeServiceStatus" },
                            new[] { "HospitalF.Controllers" });

            routes.MapRoute("ChangeFacilityStatus",
                            "data/changefacilitystatus/",
                            new { controller = "Data", action = "ChangeFacilityStatus" },
                            new[] { "HospitalF.Controllers" });

            routes.MapRoute("ChangeSpecialityStatus",
                            "data/changespecialitystatus/",
                            new { controller = "Data", action = "ChangeSpecialityStatus" },
                            new[] { "HospitalF.Controllers" });
        }
    }
}