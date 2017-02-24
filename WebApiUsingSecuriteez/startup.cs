using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(WebApiUsingSecuriteez.startup))]

namespace WebApiUsingSecuriteez
{
    public class startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServer3.AccessTokenValidation.IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://192.168.7.38/WebSiteWithSecuriteez/auth/",
                RequiredScopes = new[] { "SiteAccess" }
            });
        }

    }
}