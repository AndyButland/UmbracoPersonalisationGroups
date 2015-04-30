namespace Zone.UmbracoPersonalisationGroups.Criteria.Country
{
    using System;
    using System.Configuration;
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
            using (var reader = new DatabaseReader(_pathToDb))
            {
                try
                {
                    var response = reader.Country(ip);
                    return response.Country.IsoCode;
                }
                catch (AddressNotFoundException)
                {
                    return string.Empty;
                }
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
