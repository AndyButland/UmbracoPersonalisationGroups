namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberProfileField
{
    using System.Web;
    using Umbraco.Web;
    using Umbraco.Web.Security;

    public class UmbracoMemberProfileFieldProvider : IMemberProfileFieldProvider
    {
        public string GetMemberProfileFieldValue(string alias)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                var membershipHelper = new MembershipHelper(UmbracoContext.Current);
                var member = membershipHelper.GetCurrentMember();
                if (member != null && member.HasProperty(alias))
                {
                    return member.GetPropertyValue<string>(alias);
                }

            }

            return string.Empty;
        }
    }
}
