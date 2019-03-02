namespace Zone.UmbracoPersonalisationGroups.Routing
{
    using System.Web.Routing;
    using ClientDependency.Core;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Helpers;

    /// <summary>
    /// Configures custom routing for controller action method requests
    /// </summary>
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Common.Routing.RouteConfig.RegisterRoutes(routes);

            // Add the virtual file writer for the extension type.
            FileWriters.AddWriterForExtension(AppConstants.ResourceExtension, new EmbeddedResourceWriter());
        }
    }
}
