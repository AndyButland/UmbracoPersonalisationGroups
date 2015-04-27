namespace Zone.UmbracoPersonalisationGroups.Criteria.TimeOfDay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the time of the day
    /// </summary>
    public class TimeOfDayPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        public string Name
        {
            get { return "Time of day"; }
        }

        public string Alias
        {
            get { return "timeOfDay"; }
        }

        public string Description
        {
            get { return "Matches visitor session with defined times of the day"; }
        }

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            try
            {
                var definedTimesOfDay = JsonConvert.DeserializeObject<IList<TimeOfDaySetting>>(definition);
                var now = int.Parse(DateTime.Now.ToString("HHmm"));
                return definedTimesOfDay
                    .Any(x => x.From <= now && x.To >= now);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException(string.Format("Provided definition is not valid JSON: {0}", definition));
            }
        }
    }
}
