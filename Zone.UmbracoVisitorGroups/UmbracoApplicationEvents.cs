namespace Zone.UmbracoVisitorGroups
{
    using System.Web.Routing;
    using Umbraco.Core;

    /// <summary>
    /// Hooks into the Umbraco application start-up to register additional routes
    /// </summary>
    public class UmbracoApplicationEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
