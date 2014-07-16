using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using HospitalF.Constant;
using Newtonsoft.Json.Linq;
using HospitalF.Utilities;
using HospitalF.Controllers;

namespace HospitalF.Models
{
    /// <summary>
    /// Class define properties for /Account/Login View
    /// </summary>
    public class AccountModel
    {
        public static bool IsExistedUser(string email)
        {
            try
            {
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    User user = (from u in data.Users
                                 where u.Email.Equals(email.Trim())
                                 select u).SingleOrDefault();
                    if (user != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return false;
            }
        }

        public static bool CheckLogin(string email, string password)
        {
            try
            {
                using (LinqDBDataContext data = new LinqDBDataContext())
                {
                    User user = (from u in data.Users
                                 where u.Email.Equals(email.Trim()) && u.Password.Equals(password)
                                 select u).SingleOrDefault();
                    if (user != null)
                    {
                        SimpleSessionPersister.Username = user.Email + Constants.Minus +
                            user.Last_Name + " " + user.First_Name + Constants.Minus + user.User_ID;
                        SimpleSessionPersister.Role = user.Role.Role_Name;
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return false;
            }
        }

        public static bool CheckFacebookLogin(JObject jsonUserInfo)
        {
            try
            {
                string email = jsonUserInfo.Value<string>("email");
                if (!IsExistedUser(email))
                {
                    string firstName = jsonUserInfo.Value<string>("first_name");
                    string lastName = jsonUserInfo.Value<string>("last_name");
                    using (TransactionScope ts = new TransactionScope())
                    {

                        using (LinqDBDataContext data = new LinqDBDataContext())
                        {
                            User newUser = new User()
                            {
                                Email = email,
                                First_Name = firstName,
                                Last_Name = lastName,
                                Role_ID = Constants.UserRoleId,
                                Is_Active = true
                            };
                            data.Users.InsertOnSubmit(newUser);
                            data.SubmitChanges();
                            ts.Complete();
                        }
                    }
                }

                string fullName = jsonUserInfo.Value<string>("name");
                SimpleSessionPersister.Username = email + ";" + fullName;
                SimpleSessionPersister.Role = Constants.UserRoleName;
                return true;
            }
            catch (Exception exception)
            {
                LoggingUtil.LogException(exception);
                return false;
            }
        }
    }
}