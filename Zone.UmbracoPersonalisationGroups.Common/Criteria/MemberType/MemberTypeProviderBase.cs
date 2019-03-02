namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberType
{
    using System.Web;

    public abstract class MemberTypeProviderBase : IMemberTypeProvider
    {
        public string GetMemberType()
        {
            return HttpContext.Current.Request.IsAuthenticated
                ? GetAuthenticatedMemberType()
                : string.Empty;
        }

        protected abstract string GetAuthenticatedMemberType();
    }
}
