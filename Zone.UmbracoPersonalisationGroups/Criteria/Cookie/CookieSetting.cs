namespace Zone.UmbracoPersonalisationGroups.Criteria.Cookie
{
    public enum CookieSettingMatch
    {
        Exists,
        DoesNotExist,
        MatchesValue,
        ContainsValue,
    }

    public class CookieSetting
    {
        public string Key { get; set; }

        public CookieSettingMatch Match { get; set; }

        public string Value { get; set; }
    }
}
