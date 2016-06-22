namespace Zone.UmbracoPersonalisationGroups.Criteria.Country
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Hosting;
    using MaxMind.GeoIP2;
    using MaxMind.GeoIP2.Exceptions;
    using Umbraco.Core;

    public class MaxMindCountryGeoLocationProvider : ICountryGeoLocationProvider
    {
        private readonly string _pathToDb;

        public MaxMindCountryGeoLocationProvider()
        {
            _pathToDb = HostingEnvironment.MapPath(GetDatabasePath());
        }

        public string GetCountryFromIp(string ip)
        {
            var cacheKey = $"PersonalisationGroups_Criteria_Country_GeoLocation_{ip}";
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
                                    var isoCode = response.Country.IsoCode;
                                    if (!string.IsNullOrEmpty(isoCode))
                                    {
                                        HttpRuntime.Cache.Insert(cacheKey, isoCode, null, Cache.NoAbsoluteExpiration,
                                            Cache.NoSlidingExpiration);
                                    }

                                    return isoCode;
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

            return cachedItem?.ToString() ?? string.Empty;
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
