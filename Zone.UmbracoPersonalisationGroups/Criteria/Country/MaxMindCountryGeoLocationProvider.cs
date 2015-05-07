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

    public class MaxMindCountryGeoLocationProvider : ICountryGeoLocationProvider
    {
        private readonly string _pathToDb;

        public MaxMindCountryGeoLocationProvider()
        {
            _pathToDb = HostingEnvironment.MapPath(GetDatabasePath());
        }

        public string GetCountryFromIp(string ip)
        {
            var cacheKey = string.Format("PersonalisationGroups_Criteria_Country_GeoLocation_{0}", ip);
            var countryCode = HttpRuntime.Cache[cacheKey];
            if (countryCode == null)
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
                        string.Format(
                            "MaxMind Geolocation database required for locating visitor country from IP address not found, expected at: {0}. The path is derived from either the default ({1}) or can be configured using a relative path in an appSetting with key: \"{2}\"",
                                _pathToDb, 
                                AppConstants.DefaultGeoLocationCountryDatabasePath,
                                AppConstants.ConfigKeys.CustomGeoLocationCountryDatabasePath), 
                            _pathToDb);
                }
            }
            else
            {
                return countryCode.ToString();
            }

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
