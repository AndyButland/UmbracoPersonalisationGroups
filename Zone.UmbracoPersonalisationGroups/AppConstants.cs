namespace Zone.UmbracoPersonalisationGroups
{
    public enum Comparison
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
    }

    public static class AppConstants
    {
        public static class ConfigKeys
        {
            private const string Prefix = "personalisationGroups.";

            public const string DisablePackage = Prefix + "disabled";

            public const string CustomPersonalisationGroupPickerAlias = Prefix + "groupPickerAlias";

            public const string CustomGeoLocationCountryDatabasePath = Prefix + "geoLocationCountryDatabasePath";

            public const string CustomGeoLocationCityDatabasePath = Prefix + "geoLocationCityDatabasePath";

            public const string IncludeCriteria = Prefix + "includeCriteria";

            public const string ExcludeCriteria = Prefix + "excludeCriteria";

            public const string NumberOfVisitsTrackingCookieExpiryInDays = Prefix + "numberOfVisitsTrackingCookieExpiryInDays";

            public const string ViewedPagesTrackingCookieExpiryInDays = Prefix + "viewedPagesTrackingCookieExpiryInDays";

            public const string CookieKeyForTrackingNumberOfVisits = Prefix + "CookieKeyForTrackingNumberOfVisits";

            public const string CookieKeyForTrackingIfSessionAlreadyTracked = Prefix + "cookieKeyForTrackingIfSessionAlreadyTracked";

            public const string CookieKeyForTrackingPagesViewed = Prefix + "CookieKeyForTrackingPagesViewed";

            public const string CookieKeyForSessionMatchedGroups = Prefix + "cookieKeyForSessionMatchedGroups";

            public const string CookieKeyForPersistentMatchedGroups = Prefix + "cookieKeyForPersistentMatchedGroups";

            public const string PersistentMatchedGroupsCookieExpiryInDays = Prefix + "persistentMatchedGroupsCookieExpiryInDays";

            public const string TestFixedIp = Prefix + "testFixedIp";
        }

        public static class DocumentTypeAliases
        {
            public const string PersonalisationGroupsFolder = "PersonalisationGroupsFolder";

            public const string PersonalisationGroup = "PersonalisationGroup";
        }

        public const string DefaultPersonalisationGroupPickerAlias = "personalisationGroups";

        public const string PersonalisationGroupDefinitionPropertyEditorAlias = "personalisationGroupDefinition";

        public const string PersonalisationGroupDefinitionPropertyAlias = "definition";

        public const string ResourceRoot = "/App_Plugins/UmbracoPersonalisationGroups/GetResource/";

        public const string ResourceRootNameSpace = "Zone.UmbracoPersonalisationGroups.PropertyEditors.";

        public const string ResourceForCriteriaRoot = "/App_Plugins/UmbracoPersonalisationGroups/GetResourceForCriteria/";

        public const string ResourceForCriteriaRootNameSpace = "Zone.UmbracoPersonalisationGroups.Criteria.";

        public const string ResourceExtension = ".umb"; 

        public const string DefaultGeoLocationCountryDatabasePath = "/App_Data/GeoLite2-Country.mmdb";

        public const string DefaultGeoLocationCityDatabasePath = "/App_Data/GeoLite2-City.mmdb";

        public const int DefaultViewedPagesTrackingCookieExpiryInDays = 90;

        public const int DefaultMatchedGroupsTrackingCookieExpiryInDays = 90;

        public static class CacheKeys
        {
            public const string PersonalisationGroupsVisitorHash = "PersonalisationGroups.VisitorHash";
        }

        public static class SessionKeys
        {
            public const string PersonalisationGroupsEnsureSession = "PersonalisationGroups.EnsureSession";
        }
    }
}
