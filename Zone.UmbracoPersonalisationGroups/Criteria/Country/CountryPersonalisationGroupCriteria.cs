namespace Zone.UmbracoPersonalisationGroups.Criteria.Country
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Umbraco.Core;
    using Zone.UmbracoPersonalisationGroups.Providers.GeoLocation;
    using Zone.UmbracoPersonalisationGroups.Providers.Ip;

    /// <summary>
    /// Implements a personalisation group criteria based on the country derived from the vistor's IP address
    /// </summary>
    public class CountryPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IIpProvider _ipProvider;
        private readonly IGeoLocationProvider _geoLocationProvider;

        public CountryPersonalisationGroupCriteria()
        {
            _ipProvider = new HttpContextIpProvider();
            _geoLocationProvider = new MaxMindGeoLocationProvider();
        }

        public CountryPersonalisationGroupCriteria(IIpProvider ipProvider, IGeoLocationProvider geoLocationProvider)
        {
            _ipProvider = ipProvider;
            _geoLocationProvider = geoLocationProvider;
        }

        public string Name => "Country";

        public string Alias => "country";

        public string Description => "Matches visitor country derived from their IP address to a given list of countries";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            CountrySetting countrySetting;
            try
            {
                countrySetting = JsonConvert.DeserializeObject<CountrySetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            var ip = _ipProvider.GetIp();
            if (!string.IsNullOrEmpty(ip))
            {
                var country = _geoLocationProvider.GetCountryFromIp(ip);
                if (country != null)
                {
                    if (countrySetting.Match == GeoLocationSettingMatch.CouldNotBeLocated)
                    {
                        // We can locate, so return false.
                        return false;
                    }

                    var matchedCountry = countrySetting.Codes
                        .Any(x => string.Equals(x, country.Code, StringComparison.InvariantCultureIgnoreCase));
                    switch (countrySetting.Match)
                    {
                        case GeoLocationSettingMatch.IsLocatedIn:
                            return matchedCountry;
                        case GeoLocationSettingMatch.IsNotLocatedIn:
                            return !matchedCountry;
                        default:
                            return false;
                    }
                }
            }

            return countrySetting.Match == GeoLocationSettingMatch.CouldNotBeLocated;
        }
    }
}
