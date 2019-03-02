namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.DayOfWeek
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.DateTime;

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
            Mandate.ParameterNotNullOrEmpty(definition, nameof(definition));

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
