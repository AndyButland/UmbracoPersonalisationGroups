namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed
{
    using System;
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Cookie;

    public static class UserActivityTracker
    {
        public static void TrackPageView(int pageId)
        {
            var cookieProvoder = new HttpContextCookieProvider();
            var config = PersonalisationGroupsConfig.Value;
            var key = config.CookieKeyForTrackingPagesViewed;
            var cookie = cookieProvoder.GetCookie(key);
            if (cookie != null)
            {
                cookie.Value = AppendPageIdIfNotPreviouslyViewed(cookie.Value, pageId);
            }
            else
            {
                cookie = new HttpCookie(key) { Value = pageId.ToString(), HttpOnly = true, };
            }

            cookie.Expires = DateTime.Now.AddDays(config.ViewedPagesTrackingCookieExpiryInDays);
            cookieProvoder.SetCookie(cookie);
        }

        internal static string AppendPageIdIfNotPreviouslyViewed(string viewedPageIds, int pageId)
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