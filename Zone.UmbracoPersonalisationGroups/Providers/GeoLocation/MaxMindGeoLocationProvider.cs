namespace Zone.UmbracoPersonalisationGroups.Providers.GeoLocation
{
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Hosting;
    using MaxMind.GeoIP2;
    using MaxMind.GeoIP2.Exceptions;
    using Umbraco.Core;

    public class MaxMindGeoLocationProvider : IGeoLocationProvider
    {
        private readonly string _pathToDb;

        public MaxMindGeoLocationProvider()
        {
            _pathToDb = HostingEnvironment.MapPath(GetDatabasePath());
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
                            using (var reader = new DatabaseReader(_pathToDb))
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
                                $"MaxMind Geolocation database required for locating visitor country from IP address not found, expected at: {_pathToDb}. The path is derived from either the default ({AppConstants.DefaultGeoLocationCountryDatabasePath}) or can be configured using a relative path in an appSetting with key: \"{AppConstants.ConfigKeys.CustomGeoLocationCountryDatabasePath}\"",
                                    _pathToDb);
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
                            using (var reader = new DatabaseReader(_pathToDb))
                            {
                                try
                                {
                                    var response = reader.City(ip);
                                    var country = new Region
                                    {
                                        Code = response.MostSpecificSubdivision.IsoCode,
                                        Name = response.MostSpecificSubdivision.Name,
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
                                $"MaxMind Geolocation database required for locating visitor region from IP address not found, expected at: {_pathToDb}. The path is derived from either the default ({AppConstants.DefaultGeoLocationCountryDatabasePath}) or can be configured using a relative path in an appSetting with key: \"{AppConstants.ConfigKeys.CustomGeoLocationCountryDatabasePath}\"",
                                    _pathToDb);
                        }
                    });

            return cachedItem as Region;
        }

        private string GetDatabasePath()
        {
            var path = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.CustomGeoLocationCountryDatabasePath];
            if (string.IsNullOrEmpty(path))
            {
                path = AppConstants.DefaultGeoLocationCountryDatabasePath;
            }

            return path;
        }
    }
}
