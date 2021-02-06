namespace Zone.UmbracoPersonalisationGroups.Common.Providers.Cookie
{
    public interface ICookieProvider
    {
        bool CookieExists(string key);

        string GetCookieValue(string key);
    }
}
