namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using Umbraco.Core;
    using Umbraco.Web;

    /// <summary>
    /// Registered required Umbraco application events for the pages viewed criteria - to track via a cookie
    /// all pages viewed
    /// </summary>
    /// <remarks>See: https://our.umbraco.org/Documentation/Reference/Events-v6/Application-Startup</remarks>
    public class RegisterApplicationEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (IsCriteriaInUse())
            {
                UmbracoApplicationBase.ApplicationInit += ApplicationInit;
            }
        }

        private bool IsCriteriaInUse()
        {
            var includeCriteria = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.IncludeCriteria];
            if (!string.IsNullOrEmpty(includeCriteria))
            {
                return includeCriteria
                    .Split(',')
                    .Contains(PagesViewedPersonalisationGroupCriteria.CriteriaAlias);
            }

            var excludeCriteria = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.ExcludeCriteria];
            if (!string.IsNullOrEmpty(excludeCriteria))
            {
                return !excludeCriteria
                    .Split(',')
                    .Contains(PagesViewedPersonalisationGroupCriteria.CriteriaAlias);
            }

            return true;
        }

        private void ApplicationInit(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            app.PostRequestHandlerExecute += TrackPageView;
        }

        private void TrackPageView(object sender, EventArgs e)
        {
            var httpContext = HttpContext.Current;
            var umbracoContext = UmbracoContext.Current;

            if (umbracoContext == null || !umbracoContext.PageId.HasValue)
            {
                return;
            }

            var cookie = httpContext.Request.Cookies[CookiePagesViewedProvider.CookieKey];
            if (cookie != null)
            {
                cookie.Value = AppendPageIdIfNotPreviouslyViewed(cookie.Value, umbracoContext.PageId.Value);
            }
            else
            {
                cookie = new HttpCookie(CookiePagesViewedProvider.CookieKey);
                cookie.Value = umbracoContext.PageId.Value.ToString();
            }

            cookie.Expires = DateTime.Now.AddDays(30);
            httpContext.Response.Cookies.Add(cookie);
        }

        private string AppendPageIdIfNotPreviouslyViewed(string viewedPageIds, int pageId)
        {
            var ids = viewedPageIds
                .Split(',')
                .Select(x => int.Parse(x));
            if (!ids.Contains(pageId))
            {
                viewedPageIds = viewedPageIds + "," + pageId;
            }

            return viewedPageIds;
        }
    }
}
