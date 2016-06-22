namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberType
{
    using System;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a session key
    /// </summary>
    public class MemberTypePersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IMemberTypeProvider _memberTypeProvider;

        public MemberTypePersonalisationGroupCriteria()
        {
            _memberTypeProvider = new UmbracoMemberTypeProvider();
        }

        public MemberTypePersonalisationGroupCriteria(IMemberTypeProvider memberTypeProvider)
        {
            _memberTypeProvider = memberTypeProvider;
        }

        public string Name => "Member type";

        public string Alias => "memberType";

        public string Description => "Matches authenticated visitor session with their member type";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            MemberTypeSetting setting;
            try
            {
                setting = JsonConvert.DeserializeObject<MemberTypeSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            var memberType = _memberTypeProvider.GetMemberType();
            return (setting.Match == MemberTypeSettingMatch.IsOfType && string.Equals(setting.TypeName, memberType, StringComparison.InvariantCultureIgnoreCase)) ||
                   (setting.Match == MemberTypeSettingMatch.IsNotOfType && !string.Equals(setting.TypeName, memberType, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
