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

            public const string IncludeCriteria = "personalisationGroups.includeCriteria";

            public const string ExcludeCriteria = "personalisationGroups.excludeCriteria";

            public const string ViewedPagesTrackingCookieExpiryInDays = "personalisationGroups.viewedPagesTrackingCookieExpiryInDays";
        }

        public const string DefaultPersonalisationGroupPickerAlias = "personalisationGroups";

        public const string PersonalisationGroupDefinitionPropertyEditorAlias = "personalisationGroupDefinition";

        public const string PersonalisationGroupDefinitionPropertyAlias = "definition";

        public const string ResourceRoot = "/App_Plugins/UmbracoPersonalisationGroups/GetResource/";

        public const string ResourceForCriteriaRoot = "/App_Plugins/UmbracoPersonalisationGroups/GetResourceForCriteria/";

        public const string DefaultGeoLocationCountryDatabasePath = "/App_Data/GeoLite2-Country.mmdb";

        public const int DefaultViewedPagesTrackingCookieExpiryInDays = 90;
    }
}
