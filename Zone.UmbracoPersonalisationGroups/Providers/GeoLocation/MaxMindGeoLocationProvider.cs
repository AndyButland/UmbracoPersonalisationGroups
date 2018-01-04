namespace Zone.UmbracoPersonalisationGroups.Providers.GeoLocation
{
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Hosting;
    using MaxMind.GeoIP2;
    using MaxMind.GeoIP2.Exceptions;
    using Umbraco.Core;
    using Umbraco.Core.Configuration;
    using Zone.UmbracoPersonalisationGroups.Configuration;

    public class MaxMindGeoLocationProvider : IGeoLocationProvider
    {
        private readonly string _pathToCountryDb;
        private readonly string _pathToCityDb;

        public MaxMindGeoLocationProvider()
        {
            var config = UmbracoConfig.For.PersonalisationGroups();
            _pathToCountryDb = HostingEnvironment.MapPath(config.GeoLocationCountryDatabasePath);
            _pathToCityDb = HostingEnvironment.MapPath(config.GeoLocationCityDatabasePath);
        }

        public Country GetCountryFromIp(string ip)
        {
            var cacheKey = $"PersonalisationGroups_GeoLocation_Country_{ip}";
            var cachedItem = ApplicationContext.Current.ApplicationCache.RuntimeCache
                .GetCacheItem(cacheKey,
                    () =>
                    {
                        try
                        {
                            using (var reader = new DatabaseReader(_pathToCountryDb))
                            {
                                try
                                {
                                    var response = reader.Country(ip);
                                    var country = new Country
                                    {
                                        Code = response.Country.IsoCode,
                                        Name = response.Country.Name,
                                    };

                                    HttpRuntime.Cache.Insert(cacheKey, country, null, 
                                        Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);

                                    return country;
                                }
                                catch (AddressNotFoundException)
                                {
                                    return string.Empty;
                                }
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            throw new FileNotFoundException(
                                $"MaxMind Geolocation database required for locating visitor country from IP address not found, expected at: {_pathToCountryDb}. The path is derived from either the default ({AppConstants.DefaultGeoLocationCountryDatabasePath}) or can be configured using a relative path in an appSetting with key: \"{AppConstants.ConfigKeys.CustomGeoLocationCountryDatabasePath}\"",
                                    _pathToCountryDb);
                        }
                    });

            return cachedItem as Country;
        }

        public Region GetRegionFromIp(string ip)
        {
            var cacheKey = $"PersonalisationGroups_GeoLocation_Region_{ip}";
            var cachedItem = ApplicationContext.Current.ApplicationCache.RuntimeCache
                .GetCacheItem(cacheKey,
                    () =>
                    {
                        try
                        {
                            using (var reader = new DatabaseReader(_pathToCityDb))
                            {
                                try
                                {
                                    var response = reader.City(ip);
                                    var country = new Region
                                    {
                                        City = response.City.Name,
                                        Subdivisions = response.Subdivisions
                                            .Select(x => x.Name)
                                            .ToArray(),
                                        Country = new Country
                                        {
                                            Code = response.Country.IsoCode,
                                            Name = response.Country.Name,
                                        }
                                    };

                                    HttpRuntime.Cache.Insert(cacheKey, country, null,
                                        Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);

                                    return country;
                                }
                                catch (AddressNotFoundException)
                                {
                                    return string.Empty;
                                }
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            throw new FileNotFoundException(
                                $"MaxMind Geolocation database required for locating visitor region from IP address not found, expected at: {_pathToCountryDb}. The path is derived from either the default ({AppConstants.DefaultGeoLocationCountryDatabasePath}) or can be configured using a relative path in an appSetting with key: \"{AppConstants.ConfigKeys.CustomGeoLocationCountryDatabasePath}\"",
                                    _pathToCountryDb);
                        }
                    });

            return cachedItem as Region;
        }
    }
}
