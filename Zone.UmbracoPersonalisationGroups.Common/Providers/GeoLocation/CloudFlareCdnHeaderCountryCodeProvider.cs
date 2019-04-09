namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.RequestHeaders;

    public class CloudFlareCdnHeaderCountryCodeProvider : ICountryCodeProvider
    {
        internal const string CloudFlareCdnCountryHeaderName = "CF-IPCountry";

        private readonly IRequestHeadersProvider _requestHeadersProvider;

        public CloudFlareCdnHeaderCountryCodeProvider()
        {
            _requestHeadersProvider = new HttpContextRequestHeadersProvider();
        }

        public CloudFlareCdnHeaderCountryCodeProvider(IRequestHeadersProvider requestHeadersProvider)
        {
            Mandate.ParameterNotNull(requestHeadersProvider, nameof(requestHeadersProvider));
            _requestHeadersProvider = requestHeadersProvider;
        }

        public string GetCountryCode()
        {
            var headers = _requestHeadersProvider.GetHeaders();
            return headers?[CloudFlareCdnCountryHeaderName] ?? string.Empty;
        }
    }
}
