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
            if (AreCookiesDeclined())
            {
                return;
            }

            if (!PersonalisationGroupsConfig.Value.DisableHttpContextItemsUseInCookieOperations)
            {
                HttpContext.Current.Items[$"personalisationGroups.cookie.{cookie.Name}"] = cookie.Value;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private bool AreCookiesDeclined()
        {
            // Cookies can be declined by a solution developer either by setting a cookie or session variable.
            // If either of these exist, we shouldn't write any cookies.
            return HttpContext.Current.Request.Cookies[PersonalisationGroupsConfig.Value.CookieKeyForTrackingCookiesDeclined] != null ||
                   HttpContext.Current.Session?[PersonalisationGroupsConfig.Value.SessionKeyForTrackingCookiesDeclined] != null;

        }
    }
}
