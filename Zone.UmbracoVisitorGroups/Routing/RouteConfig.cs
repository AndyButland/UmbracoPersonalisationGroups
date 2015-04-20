namespace Zone.UmbracoVisitorGroups
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Example",
                url: "App_Plugins/UmbracoVisitorGroups/{action}/{id}",
                defaults: new { controller = "Embedded", action = "Resource", id = UrlParameter.Optional }
            );
        }
    }
}
