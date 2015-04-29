namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberType
{
    public enum MemberTypeSettingMatch
    {
        IsOfType,
        IsNotOfType,
    }

    public class MemberTypeSetting
    {
        public MemberTypeSettingMatch Match { get; set; }
        
        public string TypeName { get; set; }
    }
}
