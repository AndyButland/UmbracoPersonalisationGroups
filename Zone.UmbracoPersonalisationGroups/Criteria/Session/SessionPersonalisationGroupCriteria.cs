namespace Zone.UmbracoPersonalisationGroups.Criteria.Session
{
    using System;
    using Helpers;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a session key
    /// </summary>
    public class SessionPersonalisationGroupCriteria : PersonalisationGroupCriteriaBase, IPersonalisationGroupCriteria
    {
        private readonly ISessionProvider _sessionProvider;

        public SessionPersonalisationGroupCriteria()
        {
            _sessionProvider = new HttpContextSessionProvider();
        }

        public SessionPersonalisationGroupCriteria(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public string Name => "Session";

        public string Alias => "session";

        public string Description => "Matches visitor session with the presence, absence or value of a session key";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            SessionSetting sessionSetting;
            try
            {
                sessionSetting = JsonConvert.DeserializeObject<SessionSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            if (string.IsNullOrEmpty(sessionSetting.Key))
            {
                throw new ArgumentNullException("key", "Session key not set");
            }

            var keyExists = _sessionProvider.KeyExists(sessionSetting.Key);
            var value = string.Empty;
            if (keyExists)
            {
                value = _sessionProvider.GetValue(sessionSetting.Key);
            }

            switch (sessionSetting.Match)
            {
                case SessionSettingMatch.Exists:
                    return keyExists;
                case SessionSettingMatch.DoesNotExist:
                    return !keyExists;
                case SessionSettingMatch.MatchesValue:
                    return keyExists && MatchesValue(value, sessionSetting.Value);
                case SessionSettingMatch.ContainsValue:
                    return keyExists && ContainsValue(value, sessionSetting.Value);
                case SessionSettingMatch.GreaterThanValue:
                case SessionSettingMatch.GreaterThanOrEqualToValue:
                case SessionSettingMatch.LessThanValue:
                case SessionSettingMatch.LessThanOrEqualToValue:
                    return keyExists && 
                        CompareValues(value, sessionSetting.Value, GetComparison(sessionSetting.Match));
                case SessionSettingMatch.MatchesRegex:
                    return keyExists && MatchesRegex(value, sessionSetting.Value);
                case SessionSettingMatch.DoesNotMatchRegex:
                    return keyExists && !MatchesRegex(value, sessionSetting.Value);

                default:
                    return false;
            }
        }

        private static Comparison GetComparison(SessionSettingMatch settingMatch)
        {
            switch (settingMatch)
            {
                case SessionSettingMatch.GreaterThanValue:
                    return Comparison.GreaterThan;
                case SessionSettingMatch.GreaterThanOrEqualToValue:
                    return Comparison.GreaterThanOrEqual;
                case SessionSettingMatch.LessThanValue:
                    return Comparison.LessThan;
                case SessionSettingMatch.LessThanOrEqualToValue:
                    return Comparison.LessThanOrEqual;
                default:
                    throw new ArgumentException("Setting supplied does not match a comparison type", nameof(settingMatch));
            }
        }
    }
}
