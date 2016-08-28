namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberProfileField
{
    using System;
    using Newtonsoft.Json;
    using Umbraco.Core;
    using Zone.UmbracoPersonalisationGroups.Helpers;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a session key
    /// </summary>
    public class MemberProfileFieldPersonalisationGroupCriteria : PersonalisationGroupCriteriaBase, IPersonalisationGroupCriteria
    {
        private readonly IMemberProfileFieldProvider _memberProfileFieldProvider;

        public MemberProfileFieldPersonalisationGroupCriteria()
        {
            _memberProfileFieldProvider = new UmbracoMemberProfileFieldProvider();
        }

        public MemberProfileFieldPersonalisationGroupCriteria(IMemberProfileFieldProvider memberProfileFieldProvider)
        {
            _memberProfileFieldProvider = memberProfileFieldProvider;
        }

        public string Name => "Member profile field";

        public string Alias => "memberProfileField";

        public string Description => "Matches authenticated visitor session with a field on their member profile";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            MemberProfileFieldSetting setting;
            try
            {
                setting = JsonConvert.DeserializeObject<MemberProfileFieldSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            var value = _memberProfileFieldProvider.GetMemberProfileFieldValue(setting.Alias);

            switch (setting.Match)
            {
                case MemberProfileFieldSettingMatch.MatchesValue:
                    return MatchesValue(value, setting.Value);
                case MemberProfileFieldSettingMatch.DoesNotMatchValue:
                    return !MatchesValue(value, setting.Value);
                case MemberProfileFieldSettingMatch.GreaterThanValue:
                case MemberProfileFieldSettingMatch.GreaterThanOrEqualToValue:
                case MemberProfileFieldSettingMatch.LessThanValue:
                case MemberProfileFieldSettingMatch.LessThanOrEqualToValue:
                    return CompareValues(value, setting.Value, GetComparison(setting.Match));
                default:
                    return false;
            }
        }

        private static Comparison GetComparison(MemberProfileFieldSettingMatch settingMatch)
        {
            switch (settingMatch)
            {
                case MemberProfileFieldSettingMatch.GreaterThanValue:
                    return Comparison.GreaterThan;
                case MemberProfileFieldSettingMatch.GreaterThanOrEqualToValue:
                    return Comparison.GreaterThanOrEqual;
                case MemberProfileFieldSettingMatch.LessThanValue:
                    return Comparison.LessThan;
                case MemberProfileFieldSettingMatch.LessThanOrEqualToValue:
                    return Comparison.LessThanOrEqual;
                default:
                    throw new ArgumentException("Setting supplied does not match a comparison type", "settingMatch");
            }
        }
    }
}
