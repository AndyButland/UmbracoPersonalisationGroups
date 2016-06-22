namespace Zone.UmbracoPersonalisationGroups.Criteria.Country
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the country derived from the vistor's IP address
    /// </summary>
    public class CountryPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IIpProvider _ipProvider;
        private readonly ICountryGeoLocationProvider _countryGeoLocationProvider;

        public CountryPersonalisationGroupCriteria()
        {
            _ipProvider = new HttpContextIpProvider();
            _countryGeoLocationProvider = new MaxMindCountryGeoLocationProvider();
        }

        public CountryPersonalisationGroupCriteria(IIpProvider ipProvider, ICountryGeoLocationProvider countryGeoLocationProvider)
        {
            _ipProvider = ipProvider;
            _countryGeoLocationProvider = countryGeoLocationProvider;
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
                var country = _countryGeoLocationProvider.GetCountryFromIp(ip);
                if (!string.IsNullOrEmpty(country))
                {
                    var matchedCountry = countrySetting.Codes
                        .Any(x => string.Equals(x, country, StringComparison.InvariantCultureIgnoreCase));
                    switch (countrySetting.Match)
                    {
                        case CountrySettingMatch.IsLocatedIn:
                            return matchedCountry;
                        case CountrySettingMatch.IsNotLocatedIn:
                            return !matchedCountry;
                        default:
                            return false;
                    }
                }
            }

            return false;
        }
    }
}
