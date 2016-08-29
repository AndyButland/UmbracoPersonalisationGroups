namespace Zone.UmbracoPersonalisationGroups.Criteria.NumberOfVisits
{
    using System.Configuration;
    using System.Web;

    public class CookieNumberOfVisitsProvider : INumberOfVisitsProvider
    {
        public static string GetCookieKeyForTrackingNumberOfVisits()
        {
            const string DefaultCookieKeyForTrackingNumberOfVisits = "personalisationGroupsNumberOfVisits";

            // First check if key defined in config
            var cookieKey = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.CookieKeyForTrackingNumberOfVisits];
            if (string.IsNullOrEmpty(cookieKey))
            {
                // If not, use the convention key
                cookieKey = DefaultCookieKeyForTrackingNumberOfVisits;
            }

            return cookieKey;
        }

        public static string GetCookieKeyForTrackingIfSessionAlreadyTracked()
        {
            const string DefaultCookieKeyForTrackingIfSessionAlreadyTracked = "personalisationGroupsNumberOfVisitsSessionStarted";

            // First check if key defined in config
            var cookieKey = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.CookieKeyForTrackingIfSessionAlreadyTracked];
            if (string.IsNullOrEmpty(cookieKey))
            {
                // If not, use the convention key
                cookieKey = DefaultCookieKeyForTrackingIfSessionAlreadyTracked;
            }

            return cookieKey;

        }

        public int GetNumberOfVisits()
        {
            var cookie = HttpContext.Current.Request.Cookies[GetCookieKeyForTrackingNumberOfVisits()];
            int cookieValue;
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value) && int.TryParse(cookie.Value, out cookieValue))
            {
                return cookieValue;
            }

            return 0;
        }
    }
}
