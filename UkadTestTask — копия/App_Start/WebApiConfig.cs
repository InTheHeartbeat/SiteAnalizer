using System.Web.Http;

namespace SiteAnalyzer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // TODO: Add any additional configuration code.

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "GetState",
                routeTemplate: "api/{controller}/{action}/{url}",
                defaults: new { quoteName = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
            
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        }
    }
}