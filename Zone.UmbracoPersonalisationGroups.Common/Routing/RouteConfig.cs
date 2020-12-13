namespace Zone.UmbracoPersonalisationGroups.Common.Routing
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using ClientDependency.Core;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Configures custom routing for controller action method requests
    /// </summary>
    public static class RouteConfig
    {
        private const string CommonNamespace = "Zone.UmbracoPersonalisationGroups.Common.Controllers";

        public static void RegisterRoutes(RouteCollection routes, string versionSpecificNamespace)
        {
            routes.MapRoute(
                name: "Criteria resources",
                url: "App_Plugins/UmbracoPersonalisationGroups/GetResourceForCriteria/{criteriaAlias}/{fileName}",
                defaults: new { controller = "Resource", action = "GetResourceForCriteria" },
                namespaces: new[] { CommonNamespace });

            routes.MapRoute(
                name: "Core resources",
                url: "App_Plugins/UmbracoPersonalisationGroups/GetResource/{fileName}",
                defaults: new { controller = "Resource", action = "GetResource" },
                namespaces: new[] { CommonNamespace });

            routes.MapRoute(
                name: "Criteria methods",
                url: "App_Plugins/UmbracoPersonalisationGroups/Criteria/{action}",
                defaults: new { controller = "Criteria", action = "Index" },
                namespaces: new[] { CommonNamespace });

            routes.MapRoute(
                name: "Geo location methods",
                url: "App_Plugins/UmbracoPersonalisationGroups/GeoLocation/{action}",
                defaults: new { controller = "GeoLocation", action = "Index" },
                namespaces: new[] { CommonNamespace });

            routes.MapRoute(
                name: "Member methods",
                url: "App_Plugins/UmbracoPersonalisationGroups/Member/{action}",
                defaults: new { controller = "Member", action = "Index" },
                namespaces: new[] { versionSpecificNamespace });

            // Add the virtual file writer for the extension type.
            FileWriters.AddWriterForExtension(AppConstants.ResourceExtension, new EmbeddedResourceWriter());
        }
    }
}
