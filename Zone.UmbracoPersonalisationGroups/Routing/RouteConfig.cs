namespace Zone.UmbracoPersonalisationGroups
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using PropertyEditors;

    /// <summary>
    /// Configures custom routing for the <see cref="PersonalisationGroupDefinitionController"/>
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Personalisation group criteria resources",
                url: "App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/{criteriaAlias}/{fileName}",
                defaults: new { controller = "PersonalisationGroupDefinition", action = "ResourceForCriteria" });

            routes.MapRoute(
                name: "Personalisation group resources",
                url: "App_Plugins/UmbracoPersonalisationGroups/Resource/{fileName}",
                defaults: new { controller = "PersonalisationGroupDefinition", action = "Resource" });

            routes.MapRoute(
                name: "Personalisation group methods",
                url: "App_Plugins/UmbracoPersonalisationGroups/{action}",
                defaults: new { controller = "PersonalisationGroupDefinition", action = "Index" });
        }
    }
}
