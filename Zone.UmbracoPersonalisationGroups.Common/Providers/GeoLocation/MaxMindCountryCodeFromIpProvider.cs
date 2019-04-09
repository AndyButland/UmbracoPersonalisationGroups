namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Ip;

    public class MaxMindCountryCodeFromIpProvider : ICountryCodeProvider
    {
        private readonly IIpProvider _ipProvider;
        private readonly IGeoLocationProvider _geoLocationProvider;

        public MaxMindCountryCodeFromIpProvider()
        {
            _ipProvider = new HttpContextIpProvider();
            _geoLocationProvider = new MaxMindGeoLocationProvider();
        }

        public MaxMindCountryCodeFromIpProvider(IIpProvider ipProvider, IGeoLocationProvider geoLocationProvider)
        {
            Mandate.ParameterNotNull(ipProvider, nameof(ipProvider));
            Mandate.ParameterNotNull(geoLocationProvider, nameof(geoLocationProvider));

            _ipProvider = ipProvider;
            _geoLocationProvider = geoLocationProvider;
        }

        public string GetCountryCode()
        {
            var ip = _ipProvider.GetIp();
            if (string.IsNullOrEmpty(ip))
            {
                return string.Empty;
            }

            var country = _geoLocationProvider.GetCountryFromIp(ip);
            return country != null ? country.Code : string.Empty;
        }
    }
}
