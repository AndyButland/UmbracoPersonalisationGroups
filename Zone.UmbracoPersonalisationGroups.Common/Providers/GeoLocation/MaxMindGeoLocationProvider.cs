namespace Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation
{
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Hosting;
    using MaxMind.GeoIP2;
    using MaxMind.GeoIP2.Exceptions;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    public class MaxMindGeoLocationProvider : IGeoLocationProvider
    {
        private readonly string _pathToCountryDb;
        private readonly string _pathToCityDb;

        public MaxMindGeoLocationProvider()
        {
            var config = PersonalisationGroupsConfig.Value;
            _pathToCountryDb = HostingEnvironment.MapPath(config.GeoLocationCountryDatabasePath);
            _pathToCityDb = HostingEnvironment.MapPath(config.GeoLocationCityDatabasePath);
        }

        public Country GetCountryFromIp(string ip)
        {
            var cacheKey = $"PersonalisationGroups_GeoLocation_Country_{ip}";
            var cachedItem = RuntimeCacheHelper.GetCacheItem(cacheKey,
                () =>
                {
                    try
                    {
                        using (var reader = new DatabaseReader(_pathToCountryDb))
                        {
                            try
                            {
                                var response = reader.Country(ip);
                                return new Country { Code = response.Country.IsoCode, Name = response.Country.Name, };
                            }
                            catch (AddressNotFoundException)
                            {
                                return null;
                            }
                            catch (GeoIP2Exception ex)
                            {
                                if (IsInvalidIpException(ex))
                                {
                                    return null;
                                }

                                throw;
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

            return cachedItem;
        }

        public Region GetRegionFromIp(string ip)
        {
            var cacheKey = $"PersonalisationGroups_GeoLocation_Region_{ip}";
            var cachedItem = RuntimeCacheHelper.GetCacheItem(cacheKey,
                () =>
                {
                    try
                    {
                        using (var reader = new DatabaseReader(_pathToCityDb))
                        {
                            try
                            {
                                var response = reader.City(ip);
                                var region = new Region
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

                                return region;
                            }
                            catch (AddressNotFoundException)
                            {
                                return null;
                            }
                            catch (GeoIP2Exception ex)
                            {
                                if (IsInvalidIpException(ex))
                                {
                                    return null;
                                }

                                throw;
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

            return cachedItem;
        }

        private static bool IsInvalidIpException(GeoIP2Exception ex)
        {
            return ex.Message.StartsWith("The specified IP address was incorrectly formatted");
        }
    }
}
