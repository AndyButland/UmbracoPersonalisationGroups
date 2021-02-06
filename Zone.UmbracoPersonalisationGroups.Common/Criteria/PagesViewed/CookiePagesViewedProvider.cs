namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed
{
    using System.Collections.Generic;
    using System.Linq;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Cookie;

    public class CookiePagesViewedProvider : IPagesViewedProvider
    {
        private readonly ICookieProvider _cookieProvider;

        public CookiePagesViewedProvider()
        {
            _cookieProvider = new HttpContextCookieProvider();
        }

        public IEnumerable<int> GetNodeIdsViewed()
        {
            var cookieValue = _cookieProvider.GetCookieValue(PersonalisationGroupsConfig.Value.CookieKeyForTrackingPagesViewed);

            if (!string.IsNullOrEmpty(cookieValue))
            {
                return ParseCookieValue(cookieValue);
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
