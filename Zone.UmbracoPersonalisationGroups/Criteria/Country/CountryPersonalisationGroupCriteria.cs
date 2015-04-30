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

        public string Name
        {
            get { return "Country"; }
        }

        public string Alias
        {
            get { return "country"; }
        }

        public string Description
        {
            get { return "Matches visitor country derived from their IP address to a given list of countries"; }
        }

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            IList<CountrySetting> definedCountries;
            try
            {
                definedCountries = JsonConvert.DeserializeObject<IList<CountrySetting>>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException(string.Format("Provided definition is not valid JSON: {0}", definition));
            }

            var ip = _ipProvider.GetIp();
            if (!string.IsNullOrEmpty(ip))
            {
                var country = _countryGeoLocationProvider.GetCountryFromIp(ip);
                if (!string.IsNullOrEmpty(country))
                {
                    return definedCountries
                        .Any(x => string.Equals(x.Code, country, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return false;
        }
    }
}
