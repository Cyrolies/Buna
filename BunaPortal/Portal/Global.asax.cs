using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;


namespace CyroTechPortal
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //	"Default", // Route name
            //	"{controller}/{action}/{id}", // URL with parameters
            //	new { controller = "Home", action = "Home", id = UrlParameter.Optional } // Parameter defaults
            //																			 // new string[] { "CyroTechPortal" }//Namesapce
            //);
            routes.MapRoute(
               "Hitec",
               "Hitec",
               new { controller = "Home", action = "Hitec" }
               ); 

            routes.MapRoute(
                "ThornFarm",
                "ThornFarm",
                new { controller = "Home", action = "ThornFarm" }
                );

            routes.MapRoute(
            "Default",                                              // Route name
            "{controller}/{action}/{id}",                           // URL with parameters
            new { controller = "Home", action = "Home", id = "" }  // Parameter defaults
        );

            //routes.MapHttpRoute(
            //    name: "Default",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //); //this route is for conventional routing

        }

		protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            // Lets MVC know that anytime there is a JQueryDataTablesModel as a parameter in an action to use the
            // JQueryDataTablesModelBinder when binding the model.
            ModelBinders.Binders.Add(typeof(JQueryDataTablesModel), new JQueryDataTablesModelBinder());
                       
            // register the localization routes
            // specify the localiztion resource provider (and culture name resolver)
            LocalizationConfig.RegisterResourceProvider(() => new LocalizationDbResourceProvider());
            //// register the localizable model providers
            LocalizationConfig.RegisterModelProviders();

           
        }

        protected void Application_End()
        {
            FormsAuthentication.SignOut();
            if (Session != null)
            {
                Session.Abandon();
                Session.Timeout = 1;
            }
        }
        protected void Session_Start()
        {
           
        }

        protected void Session_End()
        {
			
		}
    }
}