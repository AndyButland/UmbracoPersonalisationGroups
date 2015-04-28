namespace Zone.UmbracoPersonalisationGroups.Criteria.Cookie
{
    using System.Web;

    public class HttpContextCookieProvider : ICookieProvider
    {
        public bool CookieExists(string key)
        {
            return HttpContext.Current.Request.Cookies[key] != null;
        }

        public string GetCookieValue(string key)
        {
            return HttpContext.Current.Request.Cookies[key].Value;    
        }
    }
}
