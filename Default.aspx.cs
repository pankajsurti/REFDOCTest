using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json.Linq;

namespace REFDOCTest
{
    public partial class Default : System.Web.UI.Page
    {
        private static string ClientId => ConfigurationManager.AppSettings["ida:ClientId"];
        private static string TenantId => ConfigurationManager.AppSettings["ida:TenantId"];
        private static string ClientSecret => ConfigurationManager.AppSettings["ida:ClientSecret"];

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated
            if (!Request.IsAuthenticated)
            {
                // Redirect to login page with return URL
                string returnUrl = HttpUtility.UrlEncode(Request.Url.PathAndQuery);
                Response.Redirect($"~/Login.aspx?ReturnUrl={returnUrl}");
                return;
            }

            // Display the authenticated user's name
            if (!IsPostBack)
            {
                lblUsername.Text = HttpUtility.HtmlEncode(User.Identity.Name);
            }
        }

        protected void btnGetToken_Click(object sender, EventArgs e)
        {
            // Register async task for ASP.NET Web Forms
            RegisterAsyncTask(new PageAsyncTask(async (cancellationToken) =>
            {
                try
                {
                    pnlError.Visible = false;
                    pnlTokenDisplay.Visible = false;

                    string scope = txtScope.Text.Trim();
                    if (string.IsNullOrEmpty(scope))
                    {
                        ShowError("Please enter a scope.");
                        return;
                    }

                    // Validate configuration
                    if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(TenantId) || string.IsNullOrEmpty(ClientSecret))
                    {
                        ShowError("Application configuration is incomplete. Please check ClientId, TenantId, and ClientSecret in Web.config.");
                        return;
                    }

                    // Get access token using client credentials flow
                    var (success, accessToken, errorMessage) = await GetAccessTokenAsync(scope);

                    if (success && !string.IsNullOrEmpty(accessToken))
                    {
                        // Display the JWT token
                        lblAccessToken.Text = HttpUtility.HtmlEncode(accessToken);

                        // Decode and display the claims
                        DecodeFetchToken(accessToken);

                        pnlTokenDisplay.Visible = true;
                    }
                    else
                    {
                        ShowError(errorMessage ?? "Failed to retrieve access token.");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error: {ex.Message}<br/><br/>Stack Trace: {HttpUtility.HtmlEncode(ex.StackTrace)}");
                }
            }));
        }

        private async Task<(bool success, string token, string errorMessage)> GetAccessTokenAsync(string scope)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenEndpoint = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token";

                    var requestData = new Dictionary<string, string>
                    {
                        { "client_id", ClientId },
                        { "client_secret", ClientSecret },
                        { "scope", scope },
                        { "grant_type", "client_credentials" }
                    };

                    var content = new FormUrlEncodedContent(requestData);
                    var response = await httpClient.PostAsync(tokenEndpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var json = JObject.Parse(responseContent);
                        var accessToken = json["access_token"]?.ToString();
                        
                        if (string.IsNullOrEmpty(accessToken))
                        {
                            return (false, null, "Access token not found in response.");
                        }

                        return (true, accessToken, null);
                    }
                    else
                    {
                        // Parse error response
                        string errorDetails = responseContent;
                        try
                        {
                            var errorJson = JObject.Parse(responseContent);
                            var error = errorJson["error"]?.ToString();
                            var errorDescription = errorJson["error_description"]?.ToString();
                            errorDetails = $"Error: {error}<br/>Description: {errorDescription}";
                        }
                        catch
                        {
                            // Use raw response if not JSON
                        }

                        return (false, null, $"Token request failed ({response.StatusCode}):<br/><br/>{errorDetails}");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, null, $"Exception during token request: {ex.Message}<br/><br/>{HttpUtility.HtmlEncode(ex.StackTrace)}");
            }
        }

        private void DecodeFetchToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Create a list of claim key-value pairs
                var claims = jwtToken.Claims.Select(c => new
                {
                    Type = c.Type,
                    Value = c.Value
                }).ToList();

                // Bind to GridView
                gvClaims.DataSource = claims;
                gvClaims.DataBind();
            }
            catch (Exception ex)
            {
                ShowError($"Error decoding token: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message; // Don't encode HTML in error messages since we're adding <br/> tags
            pnlError.Visible = true;
        }
    }
}
