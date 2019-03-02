namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.NumberOfVisits
{
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

    public class CookieNumberOfVisitsProvider : INumberOfVisitsProvider
    {
        public int GetNumberOfVisits()
        {
            var cookieKey = PersonalisationGroupsConfig.Value.CookieKeyForTrackingNumberOfVisits;
            var cookie = HttpContext.Current.Request.Cookies[cookieKey];
            if (!string.IsNullOrEmpty(cookie?.Value) && int.TryParse(cookie.Value, out int cookieValue))
            {
                return cookieValue;
            }

            return 0;
        }
    }
}
