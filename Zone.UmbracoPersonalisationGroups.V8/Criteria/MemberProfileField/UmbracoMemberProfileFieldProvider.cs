namespace Zone.UmbracoPersonalisationGroups.V8.Criteria.MemberProfileField
{
    using Umbraco.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberProfileField;
    using Zone.UmbracoPersonalisationGroups.V8.Helpers;

    public class UmbracoMemberProfileFieldProvider : MemberProfileFieldProviderBase
    {
        protected override string GetAuthenticatedMemberProfileFieldValue(string alias)
        {
            var member = MemberHelper.GetCurrentMember();
            if (member != null && member.HasProperty(alias))
            {
                return member.Value<string>(alias);
            }

            return string.Empty;
        }
    }
}
