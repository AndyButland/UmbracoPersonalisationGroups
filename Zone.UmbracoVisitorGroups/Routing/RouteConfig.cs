namespace Zone.UmbracoVisitorGroups
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Visitor group criteria resources",
                url: "App_Plugins/UmbracoVisitorGroups/ResourceForCriteria/{criteriaAlias}/{fileName}",
                defaults: new { controller = "VisitorGroupDefinition", action = "ResourceForCriteria" }
            );

            routes.MapRoute(
                name: "Visitor group resources",
                url: "App_Plugins/UmbracoVisitorGroups/Resource/{fileName}",
                defaults: new { controller = "VisitorGroupDefinition", action = "Resource" }
            );

            routes.MapRoute(
                name: "Visitor group methods",
                url: "App_Plugins/UmbracoVisitorGroups/{action}",
                defaults: new { controller = "VisitorGroupDefinition", action = "Index" }
            );
        }
    }
}
