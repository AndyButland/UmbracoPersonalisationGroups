namespace Zone.UmbracoPersonsalisationGroups.Tests.TestHelpers
{
    using System;
    using Zone.UmbracoPersonalisationGroups;

    public class Definitions
    {
        public static PersonalisationGroupDefinitionDetail MatchingDayOfWeekDefinition()
        {
            return new PersonalisationGroupDefinitionDetail
            {
                Alias = "dayOfWeek",
                Definition = $"[ {(int)(DateTime.Now.DayOfWeek) + 1} ]",
            };
        }

        public static PersonalisationGroupDefinitionDetail NonMatchingDayOfWeekDefinition()
        {
            return new PersonalisationGroupDefinitionDetail
            {
                Alias = "dayOfWeek",
                Definition = $"[ {(int)(DateTime.Now.DayOfWeek) + 2} ]",
            };
        }

        public static PersonalisationGroupDefinitionDetail MatchingTimeOfDayDefinition()
        {
            return new PersonalisationGroupDefinitionDetail
            {
                Alias = "timeOfDay",
                Definition = "[ { \"from\": \"0000\", \"to\": \"2359\" } ]"
            };
        }
    }
}
