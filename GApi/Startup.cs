using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(GApi.Startup))]

namespace GApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = "GProgectCookies",
                LoginPath = new PathString("")
            });
        }
    }
}
