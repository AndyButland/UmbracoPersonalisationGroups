namespace Zone.UmbracoPersonalisationGroups.Criteria.Region
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Umbraco.Core;
    using Zone.UmbracoPersonalisationGroups.Criteria.Country;
    using Zone.UmbracoPersonalisationGroups.Providers;
    using Zone.UmbracoPersonalisationGroups.Providers.GeoLocation;
    using Zone.UmbracoPersonalisationGroups.Providers.Ip;

    /// <summary>
    /// Implements a personalisation group criteria based on the country derived from the vistor's IP address
    /// </summary>
    public class RegionPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IIpProvider _ipProvider;
        private readonly IGeoLocationProvider _geoLocationProvider;

        public RegionPersonalisationGroupCriteria()
        {
            _ipProvider = new HttpContextIpProvider();
            _geoLocationProvider = new MaxMindGeoLocationProvider();
        }

        public RegionPersonalisationGroupCriteria(IIpProvider ipProvider, IGeoLocationProvider geoLocationProvider)
        {
            _ipProvider = ipProvider;
            _geoLocationProvider = geoLocationProvider;
        }

        public string Name => "Region";

        public string Alias => "region";

        public string Description => "Matches visitor region derived from their IP address to a given list of regions";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            RegionSetting regionSetting;
            try
            {
                regionSetting = JsonConvert.DeserializeObject<RegionSetting>(definition);
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
                    var matchedCountry = string.Equals(regionSetting.CountryCode, country.Code, StringComparison.InvariantCultureIgnoreCase);
                    var matchedRegion = false;
                    if (matchedCountry)
                    {
                        var region = _geoLocationProvider.GetRegionFromIp(ip);
                        if (region != null)
                        {
                            matchedRegion = regionSetting.Names
                                .Intersect(region.GetAllNames(), StringComparer.OrdinalIgnoreCase)
                                .Any();
                        }
                    }

                    switch (regionSetting.Match)
                    {
                        case GeoLocationSettingMatch.IsLocatedIn:
                            return matchedRegion;
                        case GeoLocationSettingMatch.IsNotLocatedIn:
                            return !matchedRegion;
                        default:
                            return false;
                    }
                }
            }

            return false;
        }
    }
}
