namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberProfileField
{
    using System;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a session key
    /// </summary>
    public class MemberProfileFieldPersonalisationGroupCriteria : IPersonalisationGroupCriteria
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

        public string Name
        {
            get { return "Member profile field"; }
        }

        public string Alias
        {
            get { return "memberProfileField"; }
        }

        public string Description
        {
            get { return "Matches authenticated visitor session with a field on their member profile"; }
        }

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
                throw new ArgumentException(string.Format("Provided definition is not valid JSON: {0}", definition));
            }

            var value = _memberProfileFieldProvider.GetMemberProfileFieldValue(setting.Alias);
            return (setting.Match == MemberProfileFieldSettingMatch.MatchesValue && string.Equals(setting.Value, value, StringComparison.InvariantCultureIgnoreCase)) ||
                   (setting.Match == MemberProfileFieldSettingMatch.DoesNotMatchValue && !string.Equals(setting.Value, value, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
