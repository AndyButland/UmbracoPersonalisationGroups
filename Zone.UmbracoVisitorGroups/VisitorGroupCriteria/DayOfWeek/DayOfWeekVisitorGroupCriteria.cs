namespace Zone.UmbracoVisitorGroups
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;

    public class DayOfWeekVisitorGroupCriteria : IVisitorGroupCriteria
    {
        public string Name
        {
            get { return "Day of week"; }
        }

        public string Alias
        {
            get { return "dayOfWeek"; }
        }

        public string Description
        {
            get { return "Matches visitor session with defined days of the week"; }
        }

        public string DefinitionSyntaxDescription
        {
            get { return "Example JSON: [ 1, 2, 6, 7 ].  Sunday is considered day 1."; }
        }

        public bool MatchesVisitor(string definition)
        {
            var definedDays = JsonConvert.DeserializeObject<int[]>(definition);
            return definedDays.Contains((int)DateTime.Now.DayOfWeek);
        }
    }
}
