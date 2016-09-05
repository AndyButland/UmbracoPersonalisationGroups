namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using Helpers;
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
            if (CriteriaConfigHelpers.IsCriteriaInUse(PagesViewedPersonalisationGroupCriteria.CriteriaAlias))
            {
                UmbracoApplicationBase.ApplicationInit += ApplicationInit;
            }
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

            if (umbracoContext?.PageId == null)
            {
                return;
            }

            var key = CookiePagesViewedProvider.GetCookieKeyForTrackingNumberOfVisits();
            var cookie = httpContext.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Value = AppendPageIdIfNotPreviouslyViewed(cookie.Value, umbracoContext.PageId.Value);
            }
            else
            {
                cookie = new HttpCookie(key)
                {
                    Value = umbracoContext.PageId.Value.ToString(),
                    HttpOnly = true,
                };
            }

            int cookieExpiryInDays;
            if (!int.TryParse(ConfigurationManager.AppSettings[AppConstants.ConfigKeys.ViewedPagesTrackingCookieExpiryInDays], out cookieExpiryInDays))
            {
                cookieExpiryInDays = AppConstants.DefaultViewedPagesTrackingCookieExpiryInDays;
            }

            cookie.Expires = DateTime.Now.AddDays(cookieExpiryInDays);
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
