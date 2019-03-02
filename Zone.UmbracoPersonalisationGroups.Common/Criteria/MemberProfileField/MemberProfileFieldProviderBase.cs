namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberProfileField
{
    using System.Web;

    public abstract class MemberProfileFieldProviderBase : IMemberProfileFieldProvider
    {
        public string GetMemberProfileFieldValue(string alias)
        {
            return HttpContext.Current.Request.IsAuthenticated 
                ? GetAuthenticatedMemberProfileFieldValue(alias) 
                : string.Empty;
        }

        protected abstract string GetAuthenticatedMemberProfileFieldValue(string alias);
    }
}
