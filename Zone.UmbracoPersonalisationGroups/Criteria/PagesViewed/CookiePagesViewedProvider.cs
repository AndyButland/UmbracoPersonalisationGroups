namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;

    public class CookiePagesViewedProvider : IPagesViewedProvider
    {
        public static string GetCookieKeyForTrackingNumberOfVisits()
        {
            const string DefaultCookieKeyForTrackingPagesViewed = "personalisationGroupsPagesViewed";

            // First check if key defined in config
            var cookieKey = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.CookieKeyForTrackingPagesViewed];
            if (string.IsNullOrEmpty(cookieKey))
            {
                // If not, use the convention key
                cookieKey = DefaultCookieKeyForTrackingPagesViewed;
            }

            return cookieKey;
        }

        public IEnumerable<int> GetNodeIdsViewed()
        {
            var cookie = HttpContext.Current.Request.Cookies[GetCookieKeyForTrackingNumberOfVisits()];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                return cookie.Value
                    .Split(',')
                    .Select(x => int.Parse(x));
            }

            return Enumerable.Empty<int>();
        }
    }
}
