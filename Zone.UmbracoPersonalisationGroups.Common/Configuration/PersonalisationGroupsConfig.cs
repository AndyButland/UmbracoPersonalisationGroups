namespace Zone.UmbracoPersonalisationGroups.Common.Configuration
{
    using System;
    using System.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation;

    /// <summary>
    /// Configuration for personalisation groups
    /// </summary>
    public class PersonalisationGroupsConfig
    {
        private static PersonalisationGroupsConfig _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalisationGroupsConfig"/> class
        /// with details from configuration.
        /// </summary>
        private PersonalisationGroupsConfig()
        {
            DisablePackage = GetConfigBoolValue(AppConstants.ConfigKeys.DisablePackage, false);
            GroupPickerAlias = GetConfigStringValue(AppConstants.ConfigKeys.CustomGroupPickerAlias, AppConstants.DefaultGroupPickerAlias);
            GeoLocationCountryDatabasePath = GetConfigStringValue(AppConstants.ConfigKeys.CustomGeoLocationCountryDatabasePath, AppConstants.DefaultGeoLocationCountryDatabasePath);
            GeoLocationCityDatabasePath = GetConfigStringValue(AppConstants.ConfigKeys.CustomGeoLocationCityDatabasePath, AppConstants.DefaultGeoLocationCityDatabasePath);
            IncludeCriteria = GetConfigStringValue(AppConstants.ConfigKeys.IncludeCriteria, string.Empty);
            ExcludeCriteria = GetConfigStringValue(AppConstants.ConfigKeys.ExcludeCriteria, string.Empty);
            NumberOfVisitsTrackingCookieExpiryInDays = GetConfigIntValue(AppConstants.ConfigKeys.NumberOfVisitsTrackingCookieExpiryInDays, AppConstants.DefaultNumberOfVisitsTrackingCookieExpiryInDays);
            ViewedPagesTrackingCookieExpiryInDays = GetConfigIntValue(AppConstants.ConfigKeys.ViewedPagesTrackingCookieExpiryInDays, AppConstants.DefaultViewedPagesTrackingCookieExpiryInDays);
            CookieKeyForTrackingNumberOfVisits = GetConfigStringValue(AppConstants.ConfigKeys.CookieKeyForTrackingNumberOfVisits, AppConstants.DefaultCookieKeyForTrackingNumberOfVisits);
            CookieKeyForTrackingIfSessionAlreadyTracked = GetConfigStringValue(AppConstants.ConfigKeys.CookieKeyForTrackingIfSessionAlreadyTracked, AppConstants.DefaultCookieKeyForTrackingIfSessionAlreadyTracked);
            CookieKeyForTrackingPagesViewed = GetConfigStringValue(AppConstants.ConfigKeys.CookieKeyForTrackingPagesViewed, AppConstants.DefaultCookieKeyForTrackingPagesViewed);
            CookieKeyForSessionMatchedGroups = GetConfigStringValue(AppConstants.ConfigKeys.CookieKeyForSessionMatchedGroups, AppConstants.DefaultCookieKeyForSessionMatchedGroups);
            CookieKeyForPersistentMatchedGroups = GetConfigStringValue(AppConstants.ConfigKeys.CookieKeyForPersistentMatchedGroups, AppConstants.DefaultCookieKeyForPersistentMatchedGroups);
            PersistentMatchedGroupsCookieExpiryInDays = GetConfigIntValue(AppConstants.ConfigKeys.PersistentMatchedGroupsCookieExpiryInDays, AppConstants.DefaultPersistentMatchedGroupsCookieExpiryInDays);
            TestFixedIp = GetConfigStringValue(AppConstants.ConfigKeys.TestFixedIp, string.Empty);
            CountryCodeProvider = (CountryCodeProvider)Enum.Parse(typeof(CountryCodeProvider), GetConfigStringValue(AppConstants.ConfigKeys.CountryCodeProvider, CountryCodeProvider.MaxMindDatabase.ToString()));
            CdnCountryCodeHttpHeaderName = GetConfigStringValue(AppConstants.ConfigKeys.CdnCountryCodeHttpHeaderName, AppConstants.DefaultCdnCountryCodeHttpHeaderName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalisationGroupsConfig"/> class
        /// with details passed as parameters.
        /// </summary>
        public PersonalisationGroupsConfig(
            bool disablePackage = false,
            string groupPickerAlias = AppConstants.DefaultGroupPickerAlias,
            string geoLocationCountryDatabasePath = AppConstants.DefaultGeoLocationCountryDatabasePath,
            string geoLocationCityDatabasePath = AppConstants.DefaultGeoLocationCityDatabasePath,
            string includeCriteria = "",
            string excludeCriteria = "",
            int numberOfVisitsTrackingCookieExpiryInDays = AppConstants.DefaultNumberOfVisitsTrackingCookieExpiryInDays,
            int viewedPagesTrackingCookieExpiryInDays = AppConstants.DefaultViewedPagesTrackingCookieExpiryInDays,
            string cookieKeyForTrackingNumberOfVisits = AppConstants.DefaultCookieKeyForTrackingNumberOfVisits,
            string cookieKeyForTrackingIfSessionAlreadyTracked = AppConstants.DefaultCookieKeyForTrackingIfSessionAlreadyTracked,
            string cookieKeyForTrackingPagesViewed = AppConstants.DefaultCookieKeyForTrackingPagesViewed,
            string cookieKeyForSessionMatchedGroups = AppConstants.DefaultCookieKeyForSessionMatchedGroups,
            string cookieKeyForPersistentMatchedGroups = AppConstants.DefaultCookieKeyForPersistentMatchedGroups, 
            int persistentMatchedGroupsCookieExpiryInDays = AppConstants.DefaultPersistentMatchedGroupsCookieExpiryInDays, 
            string testFixedIp = "",
            CountryCodeProvider countryCodeProvider = CountryCodeProvider.MaxMindDatabase,
            string cdnCountryCodeHttpHeaderName = AppConstants.DefaultCdnCountryCodeHttpHeaderName)
        {
            DisablePackage = disablePackage;
            GroupPickerAlias = groupPickerAlias;
            GeoLocationCountryDatabasePath = geoLocationCountryDatabasePath;
            GeoLocationCityDatabasePath = geoLocationCityDatabasePath;
            IncludeCriteria = includeCriteria;
            ExcludeCriteria = excludeCriteria;
            NumberOfVisitsTrackingCookieExpiryInDays = numberOfVisitsTrackingCookieExpiryInDays;
            ViewedPagesTrackingCookieExpiryInDays = viewedPagesTrackingCookieExpiryInDays;
            CookieKeyForTrackingNumberOfVisits = cookieKeyForTrackingNumberOfVisits;
            CookieKeyForTrackingIfSessionAlreadyTracked = cookieKeyForTrackingIfSessionAlreadyTracked;
            CookieKeyForTrackingPagesViewed = cookieKeyForTrackingPagesViewed;
            CookieKeyForSessionMatchedGroups = cookieKeyForSessionMatchedGroups;
            CookieKeyForPersistentMatchedGroups = cookieKeyForPersistentMatchedGroups;
            PersistentMatchedGroupsCookieExpiryInDays = persistentMatchedGroupsCookieExpiryInDays;
            TestFixedIp = testFixedIp;
            CountryCodeProvider = countryCodeProvider;
            CdnCountryCodeHttpHeaderName = cdnCountryCodeHttpHeaderName;
        }

        internal static PersonalisationGroupsConfig Value => _value ?? new PersonalisationGroupsConfig();

        public static void Setup(PersonalisationGroupsConfig config)
        {
            _value = config;
        }

        internal static void Reset()
        {
            _value = null;
        }

        public bool DisablePackage { get; }

        public string GroupPickerAlias { get; }

        public string GeoLocationCountryDatabasePath { get; }

        public string GeoLocationCityDatabasePath { get; }

        public string IncludeCriteria { get; }

        public string ExcludeCriteria { get; }

        public int NumberOfVisitsTrackingCookieExpiryInDays { get; }

        public int ViewedPagesTrackingCookieExpiryInDays { get; }

        public string CookieKeyForTrackingNumberOfVisits { get; }

        public string CookieKeyForTrackingIfSessionAlreadyTracked { get; }

        public string CookieKeyForTrackingPagesViewed { get; }

        public string CookieKeyForSessionMatchedGroups { get; }

        public string CookieKeyForPersistentMatchedGroups { get; }

        public int PersistentMatchedGroupsCookieExpiryInDays { get; }

        public string TestFixedIp { get; }

        public CountryCodeProvider CountryCodeProvider { get; }

        public string CdnCountryCodeHttpHeaderName { get; }

        private static bool GetConfigBoolValue(string key, bool defaultValue)
        {
            return bool.TryParse(GetConfigStringValue(key), out bool value) ? value : defaultValue;
        }

        private static int GetConfigIntValue(string key, int defaultValue)
        {
            return int.TryParse(GetConfigStringValue(key), out int value) ? value : defaultValue;
        }

        private static string GetConfigStringValue(string key, string defaultValue = "")
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}
