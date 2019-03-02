namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.Querystring
{
    using System;
    using Newtonsoft.Json;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    public class QuerystringPersonalisationGroupCriteria : PersonalisationGroupCriteriaBase, IPersonalisationGroupCriteria
    {
        private readonly IQuerystringProvider _querystringProvider;

        public QuerystringPersonalisationGroupCriteria()
        {
            _querystringProvider = new HttpContextQuerystringProvider();
        }

        public QuerystringPersonalisationGroupCriteria(IQuerystringProvider querystringProvider)
        {
            _querystringProvider = querystringProvider;
        }

        public string Alias => "querystring";

        public string Name => "Querystring";

        public string Description => "Matches visitor based on specific values in the Querystring";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, nameof(definition));

            QuerystringSetting querystringSetting;
            try
            {
                querystringSetting = JsonConvert.DeserializeObject<QuerystringSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            var querystring = _querystringProvider.GetQuerystring();

            var querystringValue = querystring[querystringSetting.Key];

            switch (querystringSetting.Match)
            {
                case QuerystringSettingMatch.MatchesValue:
                    return MatchesValue(querystringValue, querystringSetting.Value);
                case QuerystringSettingMatch.DoesNotMatchValue:
                    return !MatchesValue(querystringValue, querystringSetting.Value);
                case QuerystringSettingMatch.ContainsValue:
                    return ContainsValue(querystringValue, querystringSetting.Value);
                case QuerystringSettingMatch.DoesNotContainValue:
                    return !ContainsValue(querystringValue, querystringSetting.Value);
                case QuerystringSettingMatch.GreaterThanValue:
                case QuerystringSettingMatch.GreaterThanOrEqualToValue:
                case QuerystringSettingMatch.LessThanValue:
                case QuerystringSettingMatch.LessThanOrEqualToValue:
                    return CompareValues(querystringValue, querystringSetting.Value, GetComparison(querystringSetting.Match));
                case QuerystringSettingMatch.MatchesRegex:
                    return MatchesRegex(querystringValue, querystringSetting.Value);
                case QuerystringSettingMatch.DoesNotMatchRegex:
                    return !MatchesRegex(querystringValue, querystringSetting.Value);
                default:
                    return false;
            }
        }

        private static Comparison GetComparison(QuerystringSettingMatch settingMatch)
        {
            switch (settingMatch)
            {
                case QuerystringSettingMatch.GreaterThanValue:
                    return Comparison.GreaterThan;
                case QuerystringSettingMatch.GreaterThanOrEqualToValue:
                    return Comparison.GreaterThanOrEqual;
                case QuerystringSettingMatch.LessThanValue:
                    return Comparison.LessThan;
                case QuerystringSettingMatch.LessThanOrEqualToValue:
                    return Comparison.LessThanOrEqual;
                default:
                    throw new ArgumentException("Setting supplied does not match a comparison type", nameof(settingMatch));
            }
        }
    }
}