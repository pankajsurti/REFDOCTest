using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

[assembly: OwinStartup(typeof(REFDOCTest.Startup))]

namespace REFDOCTest
{
    public class Startup
    {
        // Entra ID configuration from Web.config
        private static string ClientId => ConfigurationManager.AppSettings["ida:ClientId"];
        private static string TenantId => ConfigurationManager.AppSettings["ida:TenantId"];
        private static string ClientSecret => ConfigurationManager.AppSettings["ida:ClientSecret"];
        private static string RedirectUri => ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string PostLogoutRedirectUri => ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string Authority => $"https://login.microsoftonline.com/{TenantId}/v2.0";

        public void Configuration(IAppBuilder app)
        {
            // Configure cookie authentication (used to persist auth state)
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Login.aspx"),
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                SlidingExpiration = true
            });

            // Configure OpenID Connect authentication with Microsoft Entra ID
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Authority = Authority,
                RedirectUri = RedirectUri,
                PostLogoutRedirectUri = PostLogoutRedirectUri,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                Scope = OpenIdConnectScope.OpenIdProfile + " email",
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    NameClaimType = "name"
                },
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Login.aspx?error=" + context.Exception.Message);
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = context =>
                    {
                        // Set the redirect URI dynamically if needed
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
