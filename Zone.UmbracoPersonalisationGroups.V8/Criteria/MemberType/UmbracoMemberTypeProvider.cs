namespace Zone.UmbracoPersonalisationGroups.V8.Criteria.MemberType
{
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberType;
    using Zone.UmbracoPersonalisationGroups.V8.Helpers;

    public class UmbracoMemberTypeProvider : MemberTypeProviderBase
    {
        protected override string GetAuthenticatedMemberType()
        {
            var member = MemberHelper.GetCurrentMember();
            if (member != null)
            {
                return member.ContentType.Alias;
            }

            return string.Empty;
        }
    }
}
