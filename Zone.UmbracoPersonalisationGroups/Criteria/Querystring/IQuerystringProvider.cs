namespace Zone.UmbracoPersonalisationGroups.Criteria.Querystring
{
    using System.Collections.Specialized;

    public interface IQuerystringProvider
    {
        NameValueCollection GetQuerystring();
    }
}
