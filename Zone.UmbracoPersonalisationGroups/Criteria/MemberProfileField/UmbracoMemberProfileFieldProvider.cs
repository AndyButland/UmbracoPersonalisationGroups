namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberProfileField
{
    using Umbraco.Web;
    using Umbraco.Web.Security;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberProfileField;

    public class UmbracoMemberProfileFieldProvider : MemberProfileFieldProviderBase
    {
        protected override string GetAuthenticatedMemberProfileFieldValue(string alias)
        {
            var membershipHelper = new MembershipHelper(UmbracoContext.Current);
            var member = membershipHelper.GetCurrentMember();
            if (member != null && member.HasProperty(alias))
            {
                return member.GetPropertyValue<string>(alias);
            }

            return string.Empty;
        }
    }
}
