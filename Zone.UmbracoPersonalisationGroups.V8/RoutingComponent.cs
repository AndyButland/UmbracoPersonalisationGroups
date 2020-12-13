namespace Zone.UmbracoPersonalisationGroups.V8
{
    using System.Web.Routing;
    using Umbraco.Core.Composing;
    using Zone.UmbracoPersonalisationGroups.Common.Routing;

    /// <summary>
    /// Hooks into the Umbraco application start-up to register additional routes
    /// </summary>
    public class RoutingComponent : IComponent
    {
        public void Initialize()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes, "Zone.UmbracoPersonalisationGroups.V8.Controllers");
        }

        public void Terminate()
        {
        }
    }
}
