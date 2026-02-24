using System;
using RefDocTest.Services;

namespace RefDocTest
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.Text = Context?.User?.Identity != null && Context.User.Identity.IsAuthenticated
                ? "You are signed in."
                : "You are not signed in. Login is required before token/API actions.";
        }

        protected void btnGetToken_Click(object sender, EventArgs e)
        {
            ExecuteSafely(async () =>
            {
                var scope = txtScope.Text?.Trim();
                var tokenResult = await TokenService.GetAccessTokenAsync(scope);
                txtOutput.Text = tokenResult;
            });
        }

        protected void btnCallGraph_Click(object sender, EventArgs e)
        {
            ExecuteSafely(async () =>
            {
                var scope = txtScope.Text?.Trim();
                var token = await TokenService.GetAccessTokenAsync(scope);
                var graphResponse = await TokenService.CallGraphUsersAsync(token);
                txtOutput.Text = graphResponse;
            });
        }

        private void ExecuteSafely(Func<System.Threading.Tasks.Task> action)
        {
            lblMessage.Text = string.Empty;

            if (Context?.User?.Identity == null || !Context.User.Identity.IsAuthenticated)
            {
                lblMessage.Text = "Please login first.";
                return;
            }

            try
            {
                action().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }
}