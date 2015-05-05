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
        public const string DefaultPersonalisationGroupPickerAlias = "personalisationGroups";

        public const string ConfigKeyForCustomPersonalisationGroupPickerAlias = "personalisationGroups.groupPickerAlias";

        public const string PersonalisationGroupDefinitionPropertyEditorAlias = "personalisationGroupDefinition";

        public const string PersonalisationGroupDefinitionPropertyAlias = "definition";

        public const string ResourceRoot = "/App_Plugins/UmbracoPersonalisationGroups/GetResource/";

        public const string ResourceForCriteriaRoot = "/App_Plugins/UmbracoPersonalisationGroups/GetResourceForCriteria/";

        public const string DefaultGeoLocationCountryDatabasePath = "/App_Data/GeoLite2-Country.mmdb";

        public const string ConfigKeyForCustomGeoLocationCountryDatabasePath = "personalisationGroups.geoLocationCountryDatabasePath";
    }
}
