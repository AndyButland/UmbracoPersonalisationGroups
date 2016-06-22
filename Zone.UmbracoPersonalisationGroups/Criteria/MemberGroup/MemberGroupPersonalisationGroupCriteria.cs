namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberGroup
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a session key
    /// </summary>
    public class MemberGroupPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IMemberGroupProvider _memberGroupProvider;

        public MemberGroupPersonalisationGroupCriteria()
        {
            _memberGroupProvider = new UmbracoMemberGroupProvider();
        }

        public MemberGroupPersonalisationGroupCriteria(IMemberGroupProvider memberGroupProvider)
        {
            _memberGroupProvider = memberGroupProvider;
        }

        public string Name => "Member group";

        public string Alias => "memberGroup";

        public string Description => "Matches authenticated visitor session with their member group";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            MemberGroupSetting setting;
            try
            {
                setting = JsonConvert.DeserializeObject<MemberGroupSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            var memberGroups = _memberGroupProvider.GetMemberGroups();
            return (setting.Match == MemberGroupSettingMatch.IsInGroup && memberGroups.Contains(setting.GroupName, StringComparer.InvariantCultureIgnoreCase) ||
                   (setting.Match == MemberGroupSettingMatch.IsNotInGroup && !memberGroups.Contains(setting.GroupName, StringComparer.InvariantCultureIgnoreCase)));
        }
    }
}
