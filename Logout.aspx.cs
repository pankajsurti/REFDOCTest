using System;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace REFDOCTest
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sign out from both cookie and OpenID Connect
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            
            authenticationManager.SignOut(
                CookieAuthenticationDefaults.AuthenticationType,
                OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }
    }
}
