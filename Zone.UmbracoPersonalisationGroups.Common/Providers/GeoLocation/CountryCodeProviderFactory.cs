namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

    public enum CountryCodeProvider
    {
        MaxMindDatabase,
        CdnHeader
    }

    public class CountryCodeProviderFactory
    {
        public ICountryCodeProvider CreateProvider()
        {
            var config = PersonalisationGroupsConfig.Value;
            switch (config.CountryCodeProvider)
            {
                case CountryCodeProvider.CdnHeader:
                    return new CdnHeaderCountryCodeProvider();
                default:
                    return new MaxMindCountryCodeFromIpProvider();
            }
        }
    }
}
