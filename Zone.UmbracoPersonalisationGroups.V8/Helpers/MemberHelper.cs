namespace Zone.UmbracoPersonalisationGroups.V8.Helpers
{
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web.Composing;

    public static class MemberHelper
    {
        public static IPublishedContent GetCurrentMember()
        {
            return Current.UmbracoHelper.MembershipHelper.GetCurrentMember();
        }
    }
}
