using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalF.Models;

namespace HospitalF.Controllers
{
    public class SecurityBaseController : Controller
    {
        //
        // GET: /SecurityBase/

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!string.IsNullOrEmpty(SimpleSessionPersister.Username))
            {
                filterContext.HttpContext.User =
                    new CustomPrincipal(
                        new CustomIdentity(
                            SimpleSessionPersister.Username, SimpleSessionPersister.Role));
            }
            base.OnAuthorization(filterContext);
        }

    }
}
