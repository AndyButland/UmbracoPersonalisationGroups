namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberProfileField
{
    public enum MemberProfileFieldSettingMatch
    {
        MatchesValue,
        DoesNotMatchValue,
        GreaterThanValue,
        GreaterThanOrEqualToValue,
        LessThanValue,
        LessThanOrEqualToValue,
    }

    public class MemberProfileFieldSetting
    {
        public string Alias { get; set; }

        public MemberProfileFieldSettingMatch Match { get; set; }

        public string Value { get; set; }
    }
}
