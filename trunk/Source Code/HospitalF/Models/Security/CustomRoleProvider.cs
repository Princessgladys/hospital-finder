using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace HospitalF.Models.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        #region Properties

        private int _cacheTimeoutInMinutes = 30;

        #endregion

        public override void Initialize(string name, NameValueCollection config)
        {
            // Set Properties
            int value;
            if (!string.IsNullOrEmpty(config["cacheTimeoutInMinutes"]) && Int32.TryParse(config["cacheTimeoutInMinutes"], out value))
            {
                _cacheTimeoutInMinutes = value;
            }

            // Call base method
            base.Initialize(name, config);
        }

        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured HospitalF.
        /// </summary>
        public override string[] GetRolesForUser(string email)
        {
            //Return if the user is not authenticated
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            //Return if present in Cache
            var cacheKey = string.Format("Roles_{0}", email);
            if (HttpRuntime.Cache[cacheKey] != null)
            {
                return (string[])HttpRuntime.Cache[cacheKey];
            }

            //Get role from database
            var role = new string[] { };
            using (LinqDBDataContext db = new LinqDBDataContext() )
            {
                User user = db.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    role = new []{user.Role.Role_Name};
                }
                
                //Store in cache
                HttpRuntime.Cache.Insert(cacheKey, role, null, DateTime.Now.AddMinutes(_cacheTimeoutInMinutes), Cache.NoSlidingExpiration);

                //return role value
                if (role != null)
                {
                    return role.ToArray();
                }
                else return new string[] { };
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured HospitalF.
        /// </summary>
        /// <returns>
        /// true if the specified user is in the specified role for the configured HospitalF; otherwise, false.
        /// </returns>
        public override bool IsUserInRole(string email, string roleName)
        {
            return this.GetRolesForUser(email).Contains(roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}