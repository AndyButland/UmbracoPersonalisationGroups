namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System;
    using System.Linq;
    using Cookie;
    using Helpers;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the whether certain pages (node Ids) have been viewed
    /// </summary>
    public class PagesViewedPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IPagesViewedProvider _pagesViewedProvider;

        public PagesViewedPersonalisationGroupCriteria()
        {
            _pagesViewedProvider = new CookiePagesViewedProvider();
        }

        public PagesViewedPersonalisationGroupCriteria(IPagesViewedProvider pagesViewedProvider)
        {
            _pagesViewedProvider = pagesViewedProvider;
        }

        public string Name
        {
            get { return "Pages viewed"; }
        }

        public string Alias
        {
            get { return "pagesViewed"; }
        }

        public string Description
        {
            get { return "Matches visitor session with whether certain pages have been viewed"; }
        }

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            PagesViewedSetting pagesViewedSetting;
            try
            {
                pagesViewedSetting = JsonConvert.DeserializeObject<PagesViewedSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException(string.Format("Provided definition is not valid JSON: {0}", definition));
            }

            var nodeIdsViewed = _pagesViewedProvider.GetNodeIdsViewed();

            switch (pagesViewedSetting.Match)
            {
                case PagesViewedSettingMatch.ViewedAny:
                    return pagesViewedSetting.NodeIds
                        .ContainsAny(nodeIdsViewed);
                case PagesViewedSettingMatch.ViewedAll:
                    return pagesViewedSetting.NodeIds
                        .ContainsAll(nodeIdsViewed);
                case PagesViewedSettingMatch.NotViewedAny:
                    return !pagesViewedSetting.NodeIds
                        .ContainsAny(nodeIdsViewed);
                case PagesViewedSettingMatch.NotViewedAll:
                    return !pagesViewedSetting.NodeIds
                        .ContainsAll(nodeIdsViewed);
                default:
                    return false;
            }
        }
    }
}
