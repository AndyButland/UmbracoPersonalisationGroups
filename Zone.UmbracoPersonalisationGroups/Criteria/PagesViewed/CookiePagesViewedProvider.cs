namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Umbraco.Core.Configuration;
    using Zone.UmbracoPersonalisationGroups.Configuration;

    public class CookiePagesViewedProvider : IPagesViewedProvider
    {
        public IEnumerable<int> GetNodeIdsViewed()
        {
            var cookie = HttpContext.Current.Request.Cookies[UmbracoConfig.For.PersonalisationGroups().CookieKeyForTrackingNumberOfVisits];
            if (!string.IsNullOrEmpty(cookie?.Value))
            {
                return cookie.Value
                    .Split(',')
                    .Select(int.Parse);
            }

            return Enumerable.Empty<int>();
        }
    }
}
