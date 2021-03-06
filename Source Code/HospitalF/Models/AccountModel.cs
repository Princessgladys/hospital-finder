﻿using System;
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
using System.Collections.Generic;
using HospitalF.Entities;

namespace HospitalF.Models
{
    /// <summary>
    /// Class define properties for /Account/Login View
    /// </summary>
    public class AccountModel
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

        /// <summary>
        /// Auto generate a random password string
        /// </summary>
        /// <returns>Random passwords tring</returns>
        private string AutoGeneratePassword()
        {
            Random random = new Random();
            int randomNum = 0;
            int passwordLength = 12;
            string password = string.Empty;

            for (int n = 0; n < passwordLength; n++)
            {
                randomNum = random.Next(0, Constants.AllAlphaNumericCharacter.Count());
                password += Constants.AllAlphaNumericCharacter[randomNum];
            }

            // Return password string
            return password;
        }

        /// <summary>
        /// Add new hospital user
        /// </summary>
        /// <param name="model">Account Model</param>
        /// <returns>1: Success, 0: Failed</returns>
        public async Task<int> InsertHospitalUserAsync(AccountModel model)
        {
            int result = 0;
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                result = await Task.Run(() => data.SP_INSERT_HOSPITAL_USER(model.Email,
                    model.SecondaryEmail, AutoGeneratePassword(), model.FirstName, model.LastName,
                    model.PhoneNumber, model.ConfirmedPerson));
            }
            return result;
        }

        #endregion

        #endregion

        #region VietLP

        public static User LoadUserByEmail(string email)
        {

            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                User user = (from u in data.Users
                             where u.Email.Equals(email.Trim())
                             select u).SingleOrDefault();
                return user;
            }

        }

        public static bool CheckLogin(string email, string password)
        {

            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                User user = (from u in data.Users
                             where u.Email.Equals(email.Trim()) && u.Password.Equals(password) && u.Is_Active == true
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
            User user = LoadUserByEmail(email);
            if (user == null)
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
            else if (user.Is_Active == false)
            {
                return false;
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

        public static List<UserEntity> LoadUser(string email, int userRole, int userStatus, string exclusiveEmail)
        {
            List<UserEntity> userList = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                userList = (from u in data.Users
                            where u.Email != exclusiveEmail &&
                                  (userRole == 0 || u.Role_ID == userRole) &&
                                  (userStatus == 0 || u.Is_Active == (userStatus == 1 ? true : false)) &&
                                  u.Email.Contains(email)
                            select new UserEntity()
                            {
                                User_ID = u.User_ID,
                                Email = u.Email,
                                User_Role_Name = u.Role.Role_Name,
                                Is_Active = u.Is_Active
                            }).ToList<UserEntity>();
            }
            return userList;
        }

        public static List<Role> LoadUserRole()
        {
            List<Role> roleList = null;
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                roleList = (from r in data.Roles
                            select r).ToList<Role>();
            }
            return roleList;
        }

        public static bool ActivateUser(int userId)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                User user = (from u in data.Users
                             where u.User_ID == userId
                             select u).SingleOrDefault();
                if (user != null)
                {
                    user.Is_Active = true;
                    data.SubmitChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool DeactivateUser(int userId)
        {
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                User user = (from u in data.Users
                             where u.User_ID == userId
                             select u).SingleOrDefault();
                if (user != null)
                {
                    user.Is_Active = false;
                    data.SubmitChanges();
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}