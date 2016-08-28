namespace Zone.UmbracoPersonalisationGroups.Criteria.Referral
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the presence, absence or value of a session key
    /// </summary>
    public class ReferralPersonalisationGroupCriteria : PersonalisationGroupCriteriaBase, IPersonalisationGroupCriteria
    {
        private readonly IReferrerProvider _referrerProvider;

        public ReferralPersonalisationGroupCriteria()
        {
            _referrerProvider = new HttpContextReferrerProvider();
        }

        public ReferralPersonalisationGroupCriteria(IReferrerProvider referrerProvider)
        {
            _referrerProvider = referrerProvider;
        }

        public string Name => "Referral";

        public string Alias => "referral";

        public string Description => "Matches visitor with a referral URL";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            ReferralSetting referralSetting;
            try
            {
                referralSetting = JsonConvert.DeserializeObject<ReferralSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            var referrer = _referrerProvider.GetReferrer();
            var invariantCulture = CultureInfo.InvariantCulture;
            switch (referralSetting.Match)
            {
                case ReferralSettingMatch.MatchesValue:
                    return MatchesValue(referrer, referralSetting.Value);
                case ReferralSettingMatch.DoesNotMatchValue:
                    return !MatchesValue(referrer, referralSetting.Value);
                case ReferralSettingMatch.ContainsValue:
                    return ContainsValue(referrer, referralSetting.Value);
                case ReferralSettingMatch.DoesNotContainValue:
                    return !ContainsValue(referrer, referralSetting.Value);
                default:
                    return false;
            }
        }
    }
}
