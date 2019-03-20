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
                return ParseCookieValue(cookie.Value);
            }

            return Enumerable.Empty<int>();
        }

        public static List<int> ParseCookieValue(string cookieValue)
        {
            return cookieValue.Split(',')
                              .Aggregate(new List<int>(),
                                         (result, value) =>
                                         {
                                             if (int.TryParse(value, out var item))
                                             {
                                                 result.Add(item);
                                             }

                                             return result;
                                         });
        }
        
    }
}
