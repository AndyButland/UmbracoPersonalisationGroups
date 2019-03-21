namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System;
    using System.Web;
    using Umbraco.Core;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Registered required Umbraco application events for the pages viewed criteria - to track via a cookie
    /// all pages viewed
    /// </summary>
    /// <remarks>See: https://our.umbraco.org/Documentation/Reference/Events-v6/Application-Startup</remarks>
    public class RegisterApplicationEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (CriteriaConfigHelpers.IsCriteriaInUse(PagesViewedPersonalisationGroupCriteria.CriteriaAlias))
            {
                UmbracoApplicationBase.ApplicationInit += ApplicationInit;
            }
        }

        private static void ApplicationInit(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            app.PostRequestHandlerExecute += UserActivityTracker.TrackPageView;
        }
    }
}
