namespace Zone.UmbracoPersonalisationGroups.Criteria.NumberOfVisits
{
    using System.Web;

    public class CookieNumberOfVisitsProvider : INumberOfVisitsProvider
    {
        internal static string CookieKeyForTrackingNumberOfVisits = "personalisationGroupsNumberOfVisits";
        internal static string CookieKeyForTrackingIfSessionAlreadyTracked = "personalisationGroupsNumberOfVisitsSessionStarted";
        
        public int GetNumberOfVisits()
        {
            var cookie = HttpContext.Current.Request.Cookies[CookieKeyForTrackingNumberOfVisits];
            int cookieValue;
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value) && int.TryParse(cookie.Value, out cookieValue))
            {
                return cookieValue;
            }

            return 0;
        }
    }
}
