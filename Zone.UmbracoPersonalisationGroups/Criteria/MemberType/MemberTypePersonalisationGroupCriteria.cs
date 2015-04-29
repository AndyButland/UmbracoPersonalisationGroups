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

        public string Name
        {
            get { return "Member type"; }
        }

        public string Alias
        {
            get { return "memberType"; }
        }

        public string Description
        {
            get { return "Matches authenticated visitor session with their member type"; }
        }

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
                throw new ArgumentException(string.Format("Provided definition is not valid JSON: {0}", definition));
            }

            var memberType = _memberTypeProvider.GetMemberType();
            return (setting.Match == MemberTypeSettingMatch.IsOfType && string.Equals(setting.TypeName, memberType, StringComparison.InvariantCultureIgnoreCase)) ||
                   (setting.Match == MemberTypeSettingMatch.IsNotOfType && !string.Equals(setting.TypeName, memberType, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
