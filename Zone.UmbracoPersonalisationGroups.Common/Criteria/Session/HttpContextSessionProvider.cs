namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.Session
{
    using System.Web;

    public class HttpContextSessionProvider : ISessionProvider
    {
        public bool KeyExists(string key)
        {
            return HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null;
        }

        public string GetValue(string key)
        {
            return HttpContext.Current.Session[key].ToString();    
        }
    }
}
