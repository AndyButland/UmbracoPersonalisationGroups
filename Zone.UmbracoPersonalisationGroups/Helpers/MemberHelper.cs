namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using Umbraco.Web.Security;

    public static class MemberHelper
    {
        public static IPublishedContent GetCurrentMember()
        {
            var membershipHelper = new MembershipHelper(UmbracoContext.Current);
            return membershipHelper.GetCurrentMember();
        }
    }
}
