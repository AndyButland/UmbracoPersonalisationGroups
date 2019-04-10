namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.RequestHeaders;

    public class CdnHeaderCountryCodeProvider : ICountryCodeProvider
    {
        private readonly IRequestHeadersProvider _requestHeadersProvider;

        public CdnHeaderCountryCodeProvider()
        {
            _requestHeadersProvider = new HttpContextRequestHeadersProvider();
        }

        public CdnHeaderCountryCodeProvider(IRequestHeadersProvider requestHeadersProvider)
        {
            Mandate.ParameterNotNull(requestHeadersProvider, nameof(requestHeadersProvider));
            _requestHeadersProvider = requestHeadersProvider;
        }

        public string GetCountryCode()
        {
            var headers = _requestHeadersProvider.GetHeaders();
            var headerName = PersonalisationGroupsConfig.Value.CdnCountryCodeHttpHeaderName;
            return headers?[headerName] ?? string.Empty;
        }
    }
}
