namespace Zone.UmbracoVisitorGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class TimeOfDayVisitorGroupCriteria : IVisitorGroupCriteria
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

        public string DefinitionSyntaxDescription
        {
            get { return "Example JSON: [ { \"from\": 900, \"to\": 1000 }, { \"from\": 1700, \"to\": 1800 } ].  Sunday is considered day 1."; }
        }

        public bool MatchesVisitor(string definition)
        {
            var definedTimesOfDay = JsonConvert.DeserializeObject<IList<TimeOfDaySetting>>(definition);
            var now = int.Parse(DateTime.Now.ToString("HHmm"));
            return definedTimesOfDay
                .Any(x => x.From >= now && x.To < now);
        }
    }
}
