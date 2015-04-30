namespace Zone.UmbracoPersonalisationGroups.Criteria.Country
{
    using System;
    using System.Configuration;
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
                using (var reader = new DatabaseReader(_pathToDb))
                {
                    try
                    {
                        var response = reader.Country(ip);
                        var isoCode = response.Country.IsoCode;
                        if (!string.IsNullOrEmpty(isoCode))
                        {
                            HttpRuntime.Cache.Insert(cacheKey, isoCode, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
                        }

                        return isoCode;
                    }
                    catch (AddressNotFoundException)
                    {
                        return string.Empty;
                    }
                }
            }
            else
            {
                return countryCode.ToString();
            }

        }

        private string GetDatabasePath()
        {
            var path = ConfigurationManager.AppSettings[AppConstants.ConfigKeyForCustomGeoLocationCountryDatabasePath];
            if (string.IsNullOrEmpty(path))
            {
                path = AppConstants.DefaultGeoLocationCountryDatabasePath;
            }

            return path;
        }
    }
}
