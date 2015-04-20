namespace Zone.UmbracoVisitorGroups
{
    using System.Web.Routing;
    using Umbraco.Core;

    public class UmbracoApplicationEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
