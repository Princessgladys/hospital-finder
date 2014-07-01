using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalF.Models;

namespace HospitalF.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /HospitalUsers/

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            UsersModel model = new UsersModel();
            IEnumerable<RList> role = Enum.GetValues(typeof(RList)).Cast<RList>();
            model.RolesList = from r in role
                                select new SelectListItem
                                {
                                    Text = r.ToString(),
                                    Value = ((int)r).ToString()
                                };
            return View();
        }

    }
}
