namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberType
{
    using System.Web;
    using Umbraco.Web;
    using Umbraco.Web.Security;

    public class UmbracoMemberTypeProvider : IMemberTypeProvider
    {
        public string GetMemberType()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                var membershipHelper = new MembershipHelper(UmbracoContext.Current);
                var member = membershipHelper.GetCurrentMember();
                if (member != null)
                {
                    return member.DocumentTypeAlias;
                }

            }

            return string.Empty;
        }
    }
}
