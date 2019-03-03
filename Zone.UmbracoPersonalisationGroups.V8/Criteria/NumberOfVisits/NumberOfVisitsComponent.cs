namespace Zone.UmbracoPersonalisationGroups.V8.Criteria.NumberOfVisits
{
    using System;
    using System.Web;
    using Umbraco.Core.Composing;
    using Umbraco.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.NumberOfVisits;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Registered required Umbraco application events for the number of visits criteria - to track via a cookie
    /// the number of times the visitor has accessed the site
    /// </summary>
    public class NumberOfVisitsComponent : IComponent
    {
        public void Initialize()
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

        public void Terminate()
        {
        }
    }
}
