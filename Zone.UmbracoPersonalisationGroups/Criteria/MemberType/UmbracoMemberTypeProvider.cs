namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberType
{
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberType;
    using Zone.UmbracoPersonalisationGroups.Helpers;

    public class UmbracoMemberTypeProvider : MemberTypeProviderBase
    {
        protected override string GetAuthenticatedMemberType()
        {
            var member = MemberHelper.GetCurrentMember();
            return member != null ? member.DocumentTypeAlias : string.Empty;
        }
    }
}
