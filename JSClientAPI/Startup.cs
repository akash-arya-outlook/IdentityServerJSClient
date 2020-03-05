using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JSClientAPI.Startup))]

namespace JSClientAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://identityserverhostdev.webjet.com.au/",
                ValidationMode = ValidationMode.ValidationEndpoint,

                RequiredScopes = new[] { "CommissionGuiApi" }
            });

            // configure web api

            var config = new HttpConfiguration();
            var cors = new EnableCorsAttribute("*", "*", "*");
            
            config.EnableCors(cors);
            config.MapHttpAttributeRoutes();
            // require authentication for all controllers

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new AuthorizeAttribute());

            app.UseWebApi(config);
        }
    }
}
