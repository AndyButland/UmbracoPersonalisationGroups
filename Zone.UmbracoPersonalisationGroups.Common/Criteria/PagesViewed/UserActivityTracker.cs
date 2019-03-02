namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed
{
    using System;
    using System.Linq;
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

    public static class UserActivityTracker
    {
        public static void TrackPageView(int pageId)
        {
            var httpContext = HttpContext.Current;
            var config = PersonalisationGroupsConfig.Value;
            var key = config.CookieKeyForTrackingNumberOfVisits;
            var cookie = httpContext.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Value = AppendPageIdIfNotPreviouslyViewed(cookie.Value, pageId);
            }
            else
            {
                cookie = new HttpCookie(key) { Value = pageId.ToString(), HttpOnly = true, };
            }

            cookie.Expires = DateTime.Now.AddDays(config.ViewedPagesTrackingCookieExpiryInDays);
            httpContext.Response.Cookies.Add(cookie);
        }

        private static string AppendPageIdIfNotPreviouslyViewed(string viewedPageIds, int pageId)
        {
            var ids = viewedPageIds
                .Split(',')
                .Select(int.Parse);
            if (!ids.Contains(pageId))
            {
                viewedPageIds = viewedPageIds + "," + pageId;
            }

            return viewedPageIds;
        }
    }
}