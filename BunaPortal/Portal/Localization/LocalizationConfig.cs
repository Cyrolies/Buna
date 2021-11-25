using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.Mvc;
using System.Threading;
using DALEFModel;

namespace CyroTechPortal
{
    public static class LocalizationConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Account", // Route name
                "Account/{action}", // URL with parameters
                new { controller = "Account", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                Constants.ROUTE_NAME, // Route name
                string.Format("{{{0}}}/{{controller}}/{{action}}/{{id}}", Constants.ROUTE_PARAMNAME_LANG), // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public static void RegisterResourceProvider(Func<ILocalizationResourceProvider> initializer)
        {
            try
            {
                Localizer.RegisterResourceProvider(initializer);
                Localizer.RegisterCultureNameResolver(() => Thread.CurrentThread.CurrentUICulture.Name);
              //  AdminController manager = new AdminController();
                //This adds the language resource list to session to speed things up
                //not filtering by culture becuase user can change language preference
              //  List<EntityResource> resource = manager.GetEntityResourceItems();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RegisterResourceProvider(Func<ILocalizationResourceProvider> initializer, Func<string> cultureNameResolver)
        {
            Localizer.RegisterResourceProvider(initializer);
            Localizer.RegisterCultureNameResolver(cultureNameResolver);
        }

        public static void RegisterModelProviders()
        {
            // register the model metadata provider
            ModelMetadataProviders.Current = new LocalizableDataAnnotationsModelMetadataProvider();

            // register the model validation provider
            var provider = ModelValidatorProviders.Providers.Where(p => p.GetType() == typeof(DataAnnotationsModelValidatorProvider)).FirstOrDefault();
            if (provider != null)
            {
                ModelValidatorProviders.Providers.Remove(provider);
            }
            provider = new LocalizableDataAnnotationsModelValidatorProvider();
            ModelValidatorProviders.Providers.Add(provider);
        }
    }
}
