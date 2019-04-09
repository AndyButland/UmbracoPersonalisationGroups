namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.Country
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation;

    /// <summary>
    /// Implements a personalisation group criteria based on the country derived from the vistor's IP address
    /// </summary>
    public class CountryPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly ICountryCodeProvider _countryCodeProvider;

        public CountryPersonalisationGroupCriteria()
        {
            var factory = new CountryCodeProviderFactory();
            _countryCodeProvider = factory.CreateProvider();
        }

        public CountryPersonalisationGroupCriteria(ICountryCodeProvider countryCodeProvider)
        {
            Mandate.ParameterNotNull(countryCodeProvider, nameof(countryCodeProvider));
            _countryCodeProvider = countryCodeProvider;
        }

        public string Name => "Country";

        public string Alias => "country";

        public string Description => "Matches visitor country derived from their IP address to a given list of countries";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, nameof(definition));

            CountrySetting countrySetting;
            try
            {
                countrySetting = JsonConvert.DeserializeObject<CountrySetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            var countryCode = _countryCodeProvider.GetCountryCode();
            if (!string.IsNullOrEmpty(countryCode))
            {
                if (countrySetting.Match == GeoLocationSettingMatch.CouldNotBeLocated)
                {
                    // We can't locate, so return false.
                    return false;
                }

                var matchedCountry = countrySetting.Codes
                    .Any(x => string.Equals(x, countryCode, StringComparison.InvariantCultureIgnoreCase));
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

            return countrySetting.Match == GeoLocationSettingMatch.CouldNotBeLocated;
        }
    }
}
