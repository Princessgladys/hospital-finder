using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalF.Models
{
    public class CustomIdentity : System.Security.Principal.IIdentity
    {

        public CustomIdentity(string name, string role)
        {
            this.Name = name;
            this.Role = role;
        }

        #region IIdentity Members

        public string Name { get; private set; }
        public string Role { get; private set; }

        public string AuthenticationType
        {
            get { return Role; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(this.Name); }
        }

        #endregion
    }

    public class CustomPrincipal : System.Security.Principal.IPrincipal
    {
        public CustomPrincipal(CustomIdentity identity)
        {
            this.Identity = identity;
        }

        #region IPrincipal Members

        public System.Security.Principal.IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (this.Identity.AuthenticationType == role)
            {
                return true;
            }
            return false;
        }
        #endregion
    }

    public static class SimpleSessionPersister
    {
        static string usernameSessionVar = "username";
        static string role = "user";
        public static string Username
        {
            get
            {
                if (HttpContext.Current == null) return string.Empty;
                var sessionVar = HttpContext.Current.Session[usernameSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set { HttpContext.Current.Session[usernameSessionVar] = value; }
        }

        public static string Role
        {
            get { return role; }
            set { role = value; }
        }
    }
}