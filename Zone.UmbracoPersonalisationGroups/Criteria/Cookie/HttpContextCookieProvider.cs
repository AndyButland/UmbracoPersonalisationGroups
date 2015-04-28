namespace Zone.UmbracoPersonalisationGroups.Criteria.Cookie
{
    using System.Web;

    public class HttpContextCookieProvider : ICookieProvider
    {
        private readonly HttpContext _httpContext;

        public HttpContextCookieProvider()
        {
            _httpContext = HttpContext.Current;
        }

        public bool CookieExists(string key)
        {
            return _httpContext.Request.Cookies[key] != null;
        }

        public string GetCookieValue(string key)
        {
            return _httpContext.Request.Cookies[key].Value;    
        }
    }
}
