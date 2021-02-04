namespace Zone.UmbracoPersonalisationGroups.Common.Providers.Cookie
{
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

    public class HttpContextCookieProvider : ICookieProvider
    {
        public bool CookieExists(string key)
        {
            return !PersonalisationGroupsConfig.Value.DisableHttpContextItemsUseInCookieOperations &&
                    HttpContext.Current.Items.Contains($"personalisationGroups.cookie.{key}")
                || HttpContext.Current.Request.Cookies[key] != null;
        }

        public HttpCookie GetCookie(string key)
        {
            return HttpContext.Current.Request.Cookies[key];
        }

        public string GetCookieValue(string key)
        {
            if (!PersonalisationGroupsConfig.Value.DisableHttpContextItemsUseInCookieOperations &&
                HttpContext.Current.Items.Contains($"personalisationGroups.cookie.{key}"))
            {
                return HttpContext.Current.Items[$"personalisationGroups.cookie.{key}"].ToString();
            }

            return HttpContext.Current.Request.Cookies[key]?.Value;
        }

        public void SetCookie(HttpCookie cookie)
        {
            if (!PersonalisationGroupsConfig.Value.DisableHttpContextItemsUseInCookieOperations)
            {
                HttpContext.Current.Items[$"personalisationGroups.cookie.{cookie.Name}"] = cookie.Value;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
