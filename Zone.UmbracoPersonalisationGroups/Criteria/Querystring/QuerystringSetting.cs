namespace Zone.UmbracoPersonalisationGroups.Criteria.Querystring
{
    public enum QuerystringSettingMatch
    {
        MatchesValue,
        DoesNotMatchValue,
        ContainsValue,
        DoesNotContainValue,
        GreaterThanValue,
        GreaterThanOrEqualToValue,
        LessThanValue,
        LessThanOrEqualToValue,
        MatchesRegex,
        DoesNotMatchRegex,
    }

    public class QuerystringSetting
    {
        public string Key { get; set; }

        public QuerystringSettingMatch Match { get; set; }

        public string Value { get; set; }
    }
}