namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberType
{
    using Umbraco.Web;
    using Umbraco.Web.Security;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberType;

    public class UmbracoMemberTypeProvider : MemberTypeProviderBase
    {
        protected override string GetAuthenticatedMemberType()
        {
            var membershipHelper = new MembershipHelper(UmbracoContext.Current);
            var member = membershipHelper.GetCurrentMember();
            if (member != null)
            {
                return member.DocumentTypeAlias;
            }

            return string.Empty;
        }
    }
}
