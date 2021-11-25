using System.Web.Http;
using Filters;

namespace Controllers.App_Start
{
    public class WebApiConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ModelStateFilter());
            //config.Routes.MapHttpRoute(
            //    name: "",
            //    routeTemplate: "api/{AdminController}/{id}/{key}/{culture}",
            //    defaults: new { id = RouteParameter.Optional,key = RouteParameter.Optional,culture = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //               name: "CyroTechApi",
            //               routeTemplate: "api/{controller}/{id}",
            //               defaults: new { id = RouteParameter.Optional }
            //           ); //this route is for conventional routing

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

           
        }
    }
}