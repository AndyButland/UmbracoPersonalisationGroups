namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

    public class CookiePagesViewedProvider : IPagesViewedProvider
    {
        public IEnumerable<int> GetNodeIdsViewed()
        {
            var cookie = HttpContext.Current.Request.Cookies[PersonalisationGroupsConfig.Value.CookieKeyForTrackingNumberOfVisits];
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
