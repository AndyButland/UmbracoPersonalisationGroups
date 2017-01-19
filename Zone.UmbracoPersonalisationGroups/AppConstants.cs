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
            public const string CustomPersonalisationGroupPickerAlias = "personalisationGroups.groupPickerAlias";

            public const string CustomGeoLocationCountryDatabasePath = "personalisationGroups.geoLocationCountryDatabasePath";

            public const string CustomGeoLocationCityDatabasePath = "personalisationGroups.geoLocationCityDatabasePath";

            public const string IncludeCriteria = "personalisationGroups.includeCriteria";

            public const string ExcludeCriteria = "personalisationGroups.excludeCriteria";

            public const string NumberOfVisitsTrackingCookieExpiryInDays = "personalisationGroups.numberOfVisitsTrackingCookieExpiryInDays";

            public const string ViewedPagesTrackingCookieExpiryInDays = "personalisationGroups.viewedPagesTrackingCookieExpiryInDays";

            public const string CookieKeyForTrackingNumberOfVisits = "personalisationGroups.CookieKeyForTrackingNumberOfVisits";

            public const string CookieKeyForTrackingIfSessionAlreadyTracked = "personalisationGroups.cookieKeyForTrackingIfSessionAlreadyTracked";

            public const string CookieKeyForTrackingPagesViewed = "personalisationGroups.CookieKeyForTrackingPagesViewed";

            public const string CookieKeyForSessionMatchedGroups = "personalisationGroups.cookieKeyForSessionMatchedGroups";

            public const string CookieKeyForPersistentMatchedGroups = "personalisationGroups.cookieKeyForPersistentMatchedGroups";

            public const string PersistentMatchedGroupsCookieExpiryInDays = "personalisationGroups.persistentMatchedGroupsCookieExpiryInDays";
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
