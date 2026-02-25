using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
        private static string RedirectUri => ConfigurationManager.AppSettings["ida:RedirectUri"];

        protected void Page_Load(object sender, EventArgs e)
        {
            // Handle OAuth callback with authorization code
            string code = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code) && !IsPostBack)
            {
                // Store the authorization code in session for later use
                Session["AuthCode"] = code;
            }

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

                    // Get access token using authorization code flow
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
                // Get authorization code from session
                string authCode = Session["AuthCode"] as string;

                // If no auth code, initiate authorization request
                if (string.IsNullOrEmpty(authCode))
                {
                    // Generate state parameter for CSRF protection
                    string state = Guid.NewGuid().ToString();
                    Session["OAuthState"] = state;
                    Session["RequestedScope"] = scope;

                    // Build authorization URL
                    var authUrl = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/authorize?" +
                        $"client_id={Uri.EscapeDataString(ClientId)}" +
                        $"&response_type=code" +
                        $"&redirect_uri={Uri.EscapeDataString(RedirectUri + "Default.aspx")}" +
                        $"&response_mode=query" +
                        $"&scope={Uri.EscapeDataString(scope)}" +
                        $"&state={Uri.EscapeDataString(state)}" +
                        $"&prompt=consent";

                    // Redirect to authorization endpoint
                    HttpContext.Current.Response.Redirect(authUrl, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    
                    return (false, null, "Redirecting to authorization endpoint...");
                }

                using (var httpClient = new HttpClient())
                {
                    var tokenEndpoint = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token";

                    var requestData = new Dictionary<string, string>
                    {
                        { "client_id", ClientId },
                        { "client_secret", ClientSecret },
                        { "code", authCode },
                        { "redirect_uri", RedirectUri + "Default.aspx" },
                        { "grant_type", "authorization_code" }
                    };

                    var content = new FormUrlEncodedContent(requestData);
                    var response = await httpClient.PostAsync(tokenEndpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var json = JObject.Parse(responseContent);
                        var accessToken = json["access_token"]?.ToString();
                        var refreshToken = json["refresh_token"]?.ToString();
                        
                        if (string.IsNullOrEmpty(accessToken))
                        {
                            return (false, null, "Access token not found in response.");
                        }

                        // Store refresh token for future use
                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            Session["RefreshToken"] = refreshToken;
                        }

                        // Clear the used authorization code
                        Session["AuthCode"] = null;

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

                        // Clear the failed authorization code
                        Session["AuthCode"] = null;

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
