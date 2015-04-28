namespace Zone.UmbracoPersonalisationGroups.Criteria.AuthenticationStatus
{
    using System.Web;

    public class HttpContextAuthenticationStatusProvider : IAuthenticationStatusProvider
    {
        public bool IsAuthenticated()
        {
            return HttpContext.Current.Request.IsAuthenticated;
        }
    }
}
