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
            if (string.IsNullOrEmpty(definition))
            {
                throw new ArgumentNullException("definition", "definition cannot be null or empty");
            }

            try
            {
                var definedDays = JsonConvert.DeserializeObject<int[]>(definition);
                return definedDays.Contains((int)DateTime.Now.DayOfWeek);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException(string.Format("Provided definition is not valid JSON: {0}", definition));
            }
        }
    }
}
