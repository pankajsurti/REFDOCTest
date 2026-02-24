# RefDocTest (.NET Framework 4.7.2)

This project is an ASP.NET Web Forms app with:
- Login page (`Login.aspx`)
- Logout page (`Logout.aspx`)
- Default page (`Default.aspx`) with:
  - Editable scope textbox
  - **Get Access Token** button
  - **Call Graph API** button

## Project details
- Framework: **.NET Framework 4.7.2**
- Auth for app pages: **Forms Authentication** (demo credentials from config)
- Token flow for API access token: **OAuth 2.0 client credentials** against Microsoft identity platform

## Configure before running
Open `RefDocTest/Web.config` and set:
- `AadTenantId`
- `AadClientId`
- `AadClientSecret`
- Optional demo login credentials (`DemoUsername`, `DemoPassword`)

## Azure App Registration requirements
For Graph API call (`GET https://graph.microsoft.com/v1.0/users?$top=5`):
- Add **Application** permission such as `User.Read.All`
- Grant **admin consent**
- Use scope textbox default: `https://graph.microsoft.com/.default`

## Run
1. Open `RefDocTest.sln` in Visual Studio.
2. Run with IIS Express.
3. Go to `/Login.aspx` and sign in with demo credentials.
4. Open `/Default.aspx`.
5. Adjust scope if needed and click:
   - **Get Access Token** (token output shown)
   - **Call Graph API** (Graph JSON response shown)

## Notes
- In this sample, the login/logout UI is local Forms auth.
- Token retrieval is app-only (client credentials), intended as a simple starter.
