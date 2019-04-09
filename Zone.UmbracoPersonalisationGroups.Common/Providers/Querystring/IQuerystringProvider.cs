namespace Zone.UmbracoPersonalisationGroups.Common.Providers.Querystring
{
    using System.Collections.Specialized;

    public interface IQuerystringProvider
    {
        NameValueCollection GetQuerystring();
    }
}
