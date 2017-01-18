namespace Zone.UmbracoPersonalisationGroups.Criteria.DayOfWeek
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Umbraco.Core;
    using Zone.UmbracoPersonalisationGroups.Providers;
    using Zone.UmbracoPersonalisationGroups.Providers.DateTime;

    /// <summary>
    /// Implements a personalisation group criteria based on the day of the week
    /// </summary>
    public class DayOfWeekPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DayOfWeekPersonalisationGroupCriteria()
        {
            _dateTimeProvider = new DateTimeProvider();
        }

        public DayOfWeekPersonalisationGroupCriteria(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public string Name => "Day of week";

        public string Alias => "dayOfWeek";

        public string Description => "Matches visitor session with defined days of the week";

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            try
            {
                var definedDays = JsonConvert.DeserializeObject<int[]>(definition);
                return definedDays.Contains((int)_dateTimeProvider.GetCurrentDateTime().DayOfWeek + 1);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }
        }
    }
}
