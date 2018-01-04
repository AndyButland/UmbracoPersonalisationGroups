namespace Zone.UmbracoPersonalisationGroups.Criteria.NumberOfVisits
{
    using System.Web;
    using Umbraco.Core.Configuration;
    using Zone.UmbracoPersonalisationGroups.Configuration;

    public class CookieNumberOfVisitsProvider : INumberOfVisitsProvider
    {
        public int GetNumberOfVisits()
        {
            var cookieKey = UmbracoConfig.For.PersonalisationGroups().CookieKeyForTrackingNumberOfVisits;
            var cookie = HttpContext.Current.Request.Cookies[cookieKey];
            int cookieValue;
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value) && int.TryParse(cookie.Value, out cookieValue))
            {
                return cookieValue;
            }

            return 0;
        }
    }
}
