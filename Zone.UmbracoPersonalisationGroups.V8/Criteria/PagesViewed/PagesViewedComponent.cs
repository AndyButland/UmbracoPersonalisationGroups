namespace Zone.UmbracoPersonalisationGroups.V8.Criteria.PagesViewed
{
    using System;
    using System.Web;
    using Umbraco.Core.Composing;
    using Umbraco.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Registered required Umbraco application events for the pages viewed criteria - to track via a cookie
    /// all pages viewed
    /// </summary>
    public class PagesViewedComponent : IComponent
    {
        public void Initialize()
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

        public void Terminate()
        {
        }
    }
}
