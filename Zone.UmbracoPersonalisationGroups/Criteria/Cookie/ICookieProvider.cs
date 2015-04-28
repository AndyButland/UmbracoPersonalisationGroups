namespace Zone.UmbracoPersonalisationGroups.Criteria.Cookie
{
    public interface ICookieProvider
    {
        bool CookieExists(string key);

        string GetCookieValue(string key);
    }
}
