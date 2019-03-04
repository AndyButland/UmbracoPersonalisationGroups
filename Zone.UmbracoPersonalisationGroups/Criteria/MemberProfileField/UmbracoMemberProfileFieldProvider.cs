namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberProfileField
{
    using Umbraco.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberProfileField;
    using Zone.UmbracoPersonalisationGroups.Helpers;

    public class UmbracoMemberProfileFieldProvider : MemberProfileFieldProviderBase
    {
        protected override string GetAuthenticatedMemberProfileFieldValue(string alias)
        {
            var member = MemberHelper.GetCurrentMember();
            if (member != null && member.HasProperty(alias))
            {
                return member.GetPropertyValue<string>(alias);
            }

            return string.Empty;
        }
    }
}
