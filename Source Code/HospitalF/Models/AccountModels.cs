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
    public class AccountModels
    {
        #region SonNX

        #region Properties

        /// <summary>
        /// Get/set value for property Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get/set value for property Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Get/set value for property SecondaryEmail
        /// </summary>
        public string SecondaryEmail { get; set; }

        /// <summary>
        /// Get/set value for property FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get/set value for property LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get/set value for property PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Get/set value for property ConfirmedPerson
        /// </summary>
        public int ConfirmedPerson { get; set; }

        /// <summary>
        /// Get/set value for property HospitalID
        /// </summary>
        public int HospitalID { get; set; }

        #endregion

        #region Method



        #endregion

        #endregion

        #region VietLP

        public static bool IsExistedUser(string email)
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

        public static bool CheckLogin(string email, string password)
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

        public static bool CheckFacebookLogin(JObject jsonUserInfo)
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
            SimpleSessionPersister.Username = email + Constants.Minus + fullName;
            SimpleSessionPersister.Role = Constants.UserRoleName;
            return true;


        }

        public static int LoadUserIdByEmail(string email)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                int userId = (from u in data.Users
                              where u.Email.Equals(email)
                              select u.User_ID).SingleOrDefault();
                return userId;
            }
        }

        #endregion
    }
}