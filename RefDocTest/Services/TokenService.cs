using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RefDocTest.Services
{
    public static class TokenService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static async Task<string> GetAccessTokenAsync(string scope)
        {
            if (string.IsNullOrWhiteSpace(scope))
            {
                throw new InvalidOperationException("Scope is required.");
            }

            var tenantId = GetRequiredConfig("AadTenantId");
            var clientId = GetRequiredConfig("AadClientId");
            var clientSecret = GetRequiredConfig("AadClientSecret");

            var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            var form = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "scope", scope },
                { "grant_type", "client_credentials" }
            };

            using (var content = new FormUrlEncodedContent(form))
            using (var response = await HttpClient.PostAsync(tokenEndpoint, content))
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Token request failed: " + responseBody);
                }

                var serializer = new JavaScriptSerializer();
                var tokenObj = serializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (!tokenObj.TryGetValue("access_token", out var accessTokenObj))
                {
                    throw new InvalidOperationException("access_token not found in token response.");
                }

                return accessTokenObj.ToString();
            }
        }

        public static async Task<string> CallGraphUsersAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException("Access token is required.");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/users?$top=5");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using (var response = await HttpClient.SendAsync(request))
            {
                var body = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Graph API call failed: " + body);
                }

                return body;
            }
        }

        private static string GetRequiredConfig(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value) || value.StartsWith("YOUR_", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Set a valid value for appSetting: " + key);
            }

            return value;
        }
    }
}