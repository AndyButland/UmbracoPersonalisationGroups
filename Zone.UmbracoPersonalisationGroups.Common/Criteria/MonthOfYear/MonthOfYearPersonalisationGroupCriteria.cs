namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.MonthOfYear
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.DateTime;

    /// <summary>
    /// Implements a personalisation group criteria based on the month of the year
    /// </summary>
    public class MonthOfYearPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public MonthOfYearPersonalisationGroupCriteria()
        {
            _dateTimeProvider = new DateTimeProvider();
        }

        public MonthOfYearPersonalisationGroupCriteria(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public string Name => "Month of year";

        public string Alias => "monthOfYear";

        public string Description => "Matches visitor session with defined months of the year";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, nameof(definition));

            try
            {
                var definedMonths = JsonConvert.DeserializeObject<int[]>(definition);
                return definedMonths.Contains(_dateTimeProvider.GetCurrentDateTime().Month);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }
        }
    }
}
