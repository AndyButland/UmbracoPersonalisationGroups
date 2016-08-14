namespace Zone.UmbracoPersonsalisationGroups.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups;

    [TestClass]
    public class PersonalisationGroupMatcherTests
    {
        [TestMethod]
        public void PersonalisationGroupMatcher_CountMatchingDefinitionDetails_WithDefinitonForMatchAll_AndMatchesAll_ReturnsCount()
        {
            // Arrange
            var definition = new PersonalisationGroupDefinition
            {
                Match = PersonalisationGroupDefinitionMatch.All,
                Details = new List<PersonalisationGroupDefinitionDetail>
                {
                    MatchingDayOfWeekDefinition(),
                    MatchingTimeOfDayDefinition(),
                }
            };

            // Act
            var result = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void PersonalisationGroupMatcher_CountMatchingDefinitionDetails_WithDefinitonForMatchAll_AndMatchesAllButLast_ReturnsCount()
        {
            // Arrange
            var definition = new PersonalisationGroupDefinition
            {
                Match = PersonalisationGroupDefinitionMatch.All,
                Details = new List<PersonalisationGroupDefinitionDetail>
                {
                    MatchingDayOfWeekDefinition(),
                    MatchingTimeOfDayDefinition(),
                    NonMatchingDayOfWeekDefinition(),
                }
            };

            // Act
            var result = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void PersonalisationGroupMatcher_CountMatchingDefinitionDetails_WithDefinitonForMatchAll_AndNotMatchingFirst_ReturnsShortCutCount()
        {
            // Arrange
            var definition = new PersonalisationGroupDefinition
            {
                Match = PersonalisationGroupDefinitionMatch.All,
                Details = new List<PersonalisationGroupDefinitionDetail>
                {
                    NonMatchingDayOfWeekDefinition(),
                    MatchingDayOfWeekDefinition(),
                    MatchingTimeOfDayDefinition(),
                }
            };

            // Act
            var result = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void PersonalisationGroupMatcher_CountMatchingDefinitionDetails_WithDefinitonForMatchAny_AndMatchingFirst_ReturnsShortCutCount()
        {
            // Arrange
            var definition = new PersonalisationGroupDefinition
            {
                Match = PersonalisationGroupDefinitionMatch.All,
                Details = new List<PersonalisationGroupDefinitionDetail>
                {
                    MatchingDayOfWeekDefinition(),
                    NonMatchingDayOfWeekDefinition(),
                    MatchingTimeOfDayDefinition(),
                }
            };

            // Act
            var result = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void PersonalisationGroupMatcher_IsMatch_WithMissingCritieria_ThrowsException()
        {
            // Arrange
            var definitionDetail = new PersonalisationGroupDefinitionDetail
            {
                Alias = "invalidAlias",
                Definition = string.Empty,
            };

            // Act
            PersonalisationGroupMatcher.IsMatch(definitionDetail);
        }

        [TestMethod]
        public void PersonalisationGroupMatcher_IsMatch_WithMatchingCriteria_ReturnsTrue()
        {
            // Arrange
            var definitionDetail = MatchingDayOfWeekDefinition();

            // Act
            var result = PersonalisationGroupMatcher.IsMatch(definitionDetail);

            // Arrange
            Assert.IsTrue(result);
        }

        private static PersonalisationGroupDefinitionDetail MatchingDayOfWeekDefinition()
        {
            return new PersonalisationGroupDefinitionDetail
            {
                Alias = "dayOfWeek",
                Definition = $"[ {(int) (DateTime.Now.DayOfWeek) + 1} ]",
            };
        }

        private static PersonalisationGroupDefinitionDetail NonMatchingDayOfWeekDefinition()
        {
            return new PersonalisationGroupDefinitionDetail
            {
                Alias = "dayOfWeek",
                Definition = $"[ {(int)(DateTime.Now.DayOfWeek) + 2} ]",
            };
        }

        private static PersonalisationGroupDefinitionDetail MatchingTimeOfDayDefinition()
        {
            return new PersonalisationGroupDefinitionDetail
            {
                Alias = "timeOfDay",
                Definition = "[ { \"from\": \"0000\", \"to\": \"2359\" } ]"
            };
        }
    }
}
