namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System;
    using System.Linq;
    using System.Web;
    using Helpers;
    using Umbraco.Core;
    using Umbraco.Core.Configuration;
    using Umbraco.Web;
    using Zone.UmbracoPersonalisationGroups.Configuration;

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
            var config = UmbracoConfig.For.PersonalisationGroups();

            if (umbracoContext?.PageId == null)
            {
                return;
            }

            var key = config.CookieKeyForTrackingNumberOfVisits;
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

            cookie.Expires = DateTime.Now.AddDays(config.ViewedPagesTrackingCookieExpiryInDays);
            httpContext.Response.Cookies.Add(cookie);
        }

        public static string AppendPageIdIfNotPreviouslyViewed(string viewedPageIds, int pageId)
        {
            var ids = CookiePagesViewedProvider.ParseCookieValue(viewedPageIds);

            if (!ids.Contains(pageId))
            {
                ids.Add(pageId);
            }

            return string.Join(",", ids);
        }
    }
}
