using System;
using System.Web;

namespace REFDOCTest
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Application startup code
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Session startup code
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Called at the beginning of each request
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Called when authentication is needed
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Handle unhandled errors
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Session cleanup code
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Application shutdown code
        }
    }
}
