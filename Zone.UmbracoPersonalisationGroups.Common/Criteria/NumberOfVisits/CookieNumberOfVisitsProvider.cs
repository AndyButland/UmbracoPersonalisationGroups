namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.NumberOfVisits
{
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Cookie;

    public class CookieNumberOfVisitsProvider : INumberOfVisitsProvider
    {
        private readonly ICookieProvider _cookieProvider;

        public CookieNumberOfVisitsProvider()
        {
            _cookieProvider = new HttpContextCookieProvider();
        }

        public int GetNumberOfVisits()
        {
            var cookieValue = _cookieProvider.GetCookieValue(PersonalisationGroupsConfig.Value.CookieKeyForTrackingNumberOfVisits);

            if (!string.IsNullOrEmpty(cookieValue) && int.TryParse(cookieValue, out int cookieNumericValue))
            {
                return cookieNumericValue;
            }

            return 0;
        }
    }
}
