using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using WebSiteWithSecuriteez.Models;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using System.Collections.Generic;
using IdentityServer3.Core.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using Configuration;

namespace WebSiteWithSecuriteez
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.Map("/auth", auth =>
            {
                var idserverFactory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(GetClient())
                    .UseInMemoryScopes(GetScopes());

                idserverFactory.UserService = new Registration<IUserService, IdentityServer3.AspNetIdentity.AspNetIdentityUserService<ApplicationUser, string>>();
                idserverFactory.Register(new Registration<UserManager<ApplicationUser, string>, ApplicationUserManager>());
                idserverFactory.Register(new Registration<IUserStore<ApplicationUser>>(resolver => new UserStore<ApplicationUser>(resolver.Resolve<ApplicationDbContext>())));
                idserverFactory.Register(new Registration<ApplicationDbContext, ApplicationDbContext>());

                var options = new IdentityServerOptions
                {
                    SiteName = "WebSiteWithSecuriteez",
                    SigningCertificate = Certificate.Load(),
                    RequireSsl = false,
                    Factory = idserverFactory,
                    AuthenticationOptions = new AuthenticationOptions()
                };

                auth.UseIdentityServer(options);
            });
        

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        private IEnumerable<Scope> GetScopes()
        {
            return new[]
            {
                new Scope()
                {
                    Name="SiteAccess"
                    , Type = ScopeType.Resource
                    , DisplayName = "Site Access"
                }
            };
        }

        private IEnumerable<Client> GetClient()
        {
            return new[]
            {
                new Client()
                {
                    ClientName = "Ios Securiteez",
                    ClientId = "ios",
                    Flow = Flows.Implicit,

                    AllowedScopes = new List<string> {"SiteAccess" },

                    RequireConsent = true,
                    AllowRememberConsent = true,

                    RedirectUris = new List<string> {"http://www.xamarin.com" }
                }
            };
        }
    }
}