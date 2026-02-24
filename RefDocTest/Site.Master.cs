using System;

namespace RefDocTest
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context?.User?.Identity != null && Context.User.Identity.IsAuthenticated)
            {
                lblUser.Text = "Signed in as: " + Context.User.Identity.Name;
            }
            else
            {
                lblUser.Text = "Not signed in";
            }
        }
    }
}