namespace Zone.UmbracoPersonalisationGroups.Common.Providers.RequestHeaders
{
    using System.Collections.Specialized;

    public interface IRequestHeadersProvider
    {
        NameValueCollection GetHeaders();
    }
}
