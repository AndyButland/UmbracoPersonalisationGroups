namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberProfileField
{
    public enum MemberProfileFieldSettingMatch
    {
        MatchesValue,
        DoesNotMatchValue,
    }

    public class MemberProfileFieldSetting
    {
        public string Alias { get; set; }

        public MemberProfileFieldSettingMatch Match { get; set; }

        public string Value { get; set; }
    }
}
