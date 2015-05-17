using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin.Security.Providers.LinkedIn;
using Owin;
using System;
using MicheleCaggiano.Models;

namespace MicheleCaggiano
{
  public partial class Startup
  {
    // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
    public void ConfigureAuth(IAppBuilder app)
    {
      // Configure the db context and user manager to use a single instance per request

      // Uncomment if NOT use MongoDB
      // app.CreatePerOwinContext(ApplicationDbContext.Create);

      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

      // Enable the application to use a cookie to store information for the signed in user
      // and to use a cookie to temporarily store information about a user logging in with a third party login provider
      // Configure the sign in cookie
      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        LoginPath = new PathString("/Account/Login"),
        Provider = new CookieAuthenticationProvider
        {
          OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
              validateInterval: TimeSpan.FromMinutes(30),
              regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        }
      });

      app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

      // Uncomment the following lines to enable logging in with third party login providers
      app.UseMicrosoftAccountAuthentication(
          clientId: "0000000040125C00",
          clientSecret: "CK7zAJAo8B3ieZ0fQU8y5DygMcJ-4z1V");

      app.UseLinkedInAuthentication(
          clientId: "77sw8tvgwd086k",
          clientSecret: "6gcIrGxsDNx2QC0D");

      //app.UseTwitterAuthentication(
      //   consumerKey: "",
      //   consumerSecret: "");

      app.UseFacebookAuthentication(
         appId: "815158465184694",
         appSecret: "943a5961c55de9f0eba1315c7431cc6f");

      app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
      {
        ClientId = "462401695550-d336jg3euvd74o1qjidno9ros0qvg4rg.apps.googleusercontent.com",
        ClientSecret = "kHEecH5VoLT7HLFqRfUX_EUn"
      });
    }
  }
}