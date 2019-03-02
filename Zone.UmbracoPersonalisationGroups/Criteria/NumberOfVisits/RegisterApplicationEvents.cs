namespace Zone.UmbracoPersonalisationGroups.Criteria.NumberOfVisits
{
    using System;
    using System.Web;
    using Umbraco.Core;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.NumberOfVisits;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Registered required Umbraco application events for the number of visits criteria - to track via a cookie
    /// the number of times the visitor has accessed the site
    /// </summary>
    /// <remarks>See: https://our.umbraco.org/Documentation/Reference/Events-v6/Application-Startup</remarks>
    public class RegisterApplicationEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (CriteriaConfigHelpers.IsCriteriaInUse(NumberOfVisitsPersonalisationGroupCriteria.CriteriaAlias))
            {
                UmbracoApplicationBase.ApplicationInit += ApplicationInit;
            }
        }

        private static void ApplicationInit(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            app.PostRequestHandlerExecute += UserActivityTracker.TrackSession;
        }
    }
}
