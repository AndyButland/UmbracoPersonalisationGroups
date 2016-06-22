namespace Zone.UmbracoPersonalisationGroups.Criteria.NumberOfVisits
{
    using System;
    using Newtonsoft.Json;
    using PagesViewed;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the whether certain pages (node Ids) have been viewed
    /// </summary>
    public class NumberOfVisitsPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        internal static string CriteriaAlias = "numberOfVisits";

        private readonly INumberOfVisitsProvider _numberOfVisitsProvider;

        public NumberOfVisitsPersonalisationGroupCriteria()
        {
            _numberOfVisitsProvider = new CookieNumberOfVisitsProvider();
        }

        public NumberOfVisitsPersonalisationGroupCriteria(INumberOfVisitsProvider numberOfVisitsProvider)
        {
            _numberOfVisitsProvider = numberOfVisitsProvider;
        }

        public string Name => "Number of visits";

        public string Alias => CriteriaAlias;

        public string Description => "Matches visitor session with the number of times they have visited the site";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            NumberOfVisitsSetting numberOfVisitsSetting;
            try
            {
                numberOfVisitsSetting = JsonConvert.DeserializeObject<NumberOfVisitsSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            var timesVisited = _numberOfVisitsProvider.GetNumberOfVisits();

            switch (numberOfVisitsSetting.Match)
            {
                case NumberOfVisitsSettingMatch.MoreThan:
                    return timesVisited > numberOfVisitsSetting.Number;
                case NumberOfVisitsSettingMatch.LessThan:
                    return timesVisited < numberOfVisitsSetting.Number;
                case NumberOfVisitsSettingMatch.Exactly:
                    return timesVisited == numberOfVisitsSetting.Number;
                default:
                    return false;
            }
        }
    }
}
