namespace Zone.UmbracoPersonalisationGroups.Criteria.DayOfWeek
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on the day of the week
    /// </summary>
    public class DayOfWeekPersonalisationGroupCriteria : IPersonalisationGroupCriteria
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

        public bool HasDefinitionEditorView
        {
            get { return true; }
        }

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

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
