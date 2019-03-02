namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.Querystring
{
    using System.Collections.Specialized;

    public interface IQuerystringProvider
    {
        NameValueCollection GetQuerystring();
    }
}
