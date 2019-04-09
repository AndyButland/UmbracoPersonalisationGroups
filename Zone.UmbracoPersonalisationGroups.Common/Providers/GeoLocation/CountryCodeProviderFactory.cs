namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

    public enum CountryCodeProvider
    {
        MaxMindDatabase,
        CloudFlareCdnHeader
    }

    public class CountryCodeProviderFactory
    {
        public ICountryCodeProvider CreateProvider()
        {
            var config = PersonalisationGroupsConfig.Value;
            switch (config.CountryCodeProvider)
            {
                case CountryCodeProvider.CloudFlareCdnHeader:
                    return new CloudFlareCdnHeaderCountryCodeProvider();
                default:
                    return new MaxMindCountryCodeFromIpProvider();
            }
        }
    }
}
