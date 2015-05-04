namespace Zone.UmbracoPersonalisationGroups.Criteria.Session
{
    public enum SessionSettingMatch
    {
        Exists,
        DoesNotExist,
        MatchesValue,
        ContainsValue,
        GreaterThanValue,
        GreaterThanOrEqualToValue,
        LessThanValue,
        LessThanOrEqualToValue,
    }

    public class SessionSetting
    {
        public string Key { get; set; }

        public SessionSettingMatch Match { get; set; }

        public string Value { get; set; }
    }
}
