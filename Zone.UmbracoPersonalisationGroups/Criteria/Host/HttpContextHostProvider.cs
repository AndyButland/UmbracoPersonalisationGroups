namespace Zone.UmbracoPersonalisationGroups.Criteria.Host
{
    using System.Web;

    public class HttpContextHostProvider : IHostProvider
    {
        public string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }
    }
}
