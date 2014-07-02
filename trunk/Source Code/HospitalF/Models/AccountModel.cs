using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalF.Constant;

namespace HospitalF.Models
{
    /// <summary>
    /// Class define properties for /Account/Login View
    /// </summary>
    public class AccountModel
    {
        public static bool CheckLogin(string username, string password)
        {
            try
            {
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    User user = (from u in data.Users
                                 where u.Email.Equals(username.Trim()) && u.Password.Equals(password)
                                 select u).SingleOrDefault();
                    if (user != null)
                    {
                        SimpleSessionPersister.Username = user.Email + ";" + user.Last_Name + " " + user.First_Name;
                        SimpleSessionPersister.Role = user.Role.Role_Name;
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}