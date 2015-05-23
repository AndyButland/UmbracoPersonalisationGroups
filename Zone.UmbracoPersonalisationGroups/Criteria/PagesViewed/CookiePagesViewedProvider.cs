namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CookiePagesViewedProvider : IPagesViewedProvider
    {
        internal static string CookieKey = "personalisationGroupsPagesViewed";

        public IEnumerable<int> GetNodeIdsViewed()
        {
            var cookie = HttpContext.Current.Request.Cookies[CookieKey];
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
