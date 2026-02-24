using System;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;

namespace REFDOCTest
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check for error from OIDC authentication
            string error = Request.QueryString["error"];
            if (!string.IsNullOrEmpty(error))
            {
                lblError.Text = "Authentication failed: " + HttpUtility.HtmlEncode(error);
                lblError.Visible = true;
            }

            // If user is already authenticated, redirect to Default
            if (Request.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnMicrosoftLogin_Click(object sender, EventArgs e)
        {
            // Trigger OpenID Connect authentication challenge
            if (!Request.IsAuthenticated)
            {
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                
                // Get return URL if provided
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "~/Default.aspx";
                }

                authenticationManager.Challenge(
                    new AuthenticationProperties { RedirectUri = returnUrl },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }
    }
}
