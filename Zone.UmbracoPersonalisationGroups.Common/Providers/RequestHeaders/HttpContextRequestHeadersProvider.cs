namespace Zone.UmbracoPersonalisationGroups.Common.Providers.RequestHeaders
{
    using System.Collections.Specialized;
    using System.Web;

    public class HttpContextRequestHeadersProvider : IRequestHeadersProvider
    {
        public NameValueCollection GetHeaders()
        {
            return HttpContext.Current.Request.Headers;
        }
    }
}
