namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.Cookie
{
    using System;
    using Newtonsoft.Json;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a cookie
    /// </summary>
    public class CookiePersonalisationGroupCriteria : PersonalisationGroupCriteriaBase, IPersonalisationGroupCriteria
    {
        private readonly ICookieProvider _cookieProvider;

        public CookiePersonalisationGroupCriteria()
        {
            _cookieProvider = new HttpContextCookieProvider();
        }

        public CookiePersonalisationGroupCriteria(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider;
        }

        public string Name => "Cookie";

        public string Alias => "cookie";

        public string Description => "Matches visitor session with the presence, absence or value of a cookie";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, nameof(definition));

            CookieSetting cookieSetting;
            try
            {
                cookieSetting = JsonConvert.DeserializeObject<CookieSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            if (string.IsNullOrEmpty(cookieSetting.Key))
            {
                throw new ArgumentNullException("key", "Cookie key not set");
            }

            var cookieExists = _cookieProvider.CookieExists(cookieSetting.Key);
            var cookieValue = string.Empty;
            if (cookieExists)
            {
                cookieValue = _cookieProvider.GetCookieValue(cookieSetting.Key);
            }

            switch (cookieSetting.Match)
            {
                case CookieSettingMatch.Exists:
                    return cookieExists;
                case CookieSettingMatch.DoesNotExist:
                    return !cookieExists;
                case CookieSettingMatch.MatchesValue:
                    return cookieExists && MatchesValue(cookieValue, cookieSetting.Value);
                case CookieSettingMatch.ContainsValue:
                    return cookieExists && ContainsValue(cookieValue, cookieSetting.Value);
                case CookieSettingMatch.GreaterThanValue:
                case CookieSettingMatch.GreaterThanOrEqualToValue:
                case CookieSettingMatch.LessThanValue:
                case CookieSettingMatch.LessThanOrEqualToValue:
                    return cookieExists &&
                        CompareValues(cookieValue, cookieSetting.Value, GetComparison(cookieSetting.Match));
                case CookieSettingMatch.MatchesRegex:
                    return cookieExists && MatchesRegex(cookieValue, cookieSetting.Value);
                case CookieSettingMatch.DoesNotMatchRegex:
                    return cookieExists && !MatchesRegex(cookieValue, cookieSetting.Value);
                default:
                    return false;
            }
        }

        private static Comparison GetComparison(CookieSettingMatch settingMatch)
        {
            switch (settingMatch)
            {
                case CookieSettingMatch.GreaterThanValue:
                    return Comparison.GreaterThan;
                case CookieSettingMatch.GreaterThanOrEqualToValue:
                    return Comparison.GreaterThanOrEqual;
                case CookieSettingMatch.LessThanValue:
                    return Comparison.LessThan;
                case CookieSettingMatch.LessThanOrEqualToValue:
                    return Comparison.LessThanOrEqual;
                default:
                    throw new ArgumentException("Setting supplied does not match a comparison type", nameof(settingMatch));
            }
        }
    }
}
