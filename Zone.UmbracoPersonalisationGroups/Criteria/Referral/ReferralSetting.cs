namespace Zone.UmbracoPersonalisationGroups.Criteria.Referral
{
    public enum ReferralSettingMatch
    {
        MatchesValue,
        DoesNotMatchValue,
        ContainsValue,
        DoesNotContainValue,
    }

    public class ReferralSetting
    {
        public ReferralSettingMatch Match { get; set; }

        public string Value { get; set; }
    }
}
