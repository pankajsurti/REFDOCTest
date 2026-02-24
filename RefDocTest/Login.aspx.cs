using System;
using System.Configuration;
using System.Web.Security;

namespace RefDocTest
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var configuredUser = ConfigurationManager.AppSettings["DemoUsername"];
            var configuredPassword = ConfigurationManager.AppSettings["DemoPassword"];

            var userName = txtUsername.Text?.Trim();
            var password = txtPassword.Text;

            if (string.Equals(userName, configuredUser, StringComparison.Ordinal) &&
                string.Equals(password, configuredPassword, StringComparison.Ordinal))
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            lblError.Text = "Invalid username or password.";
        }
    }
}