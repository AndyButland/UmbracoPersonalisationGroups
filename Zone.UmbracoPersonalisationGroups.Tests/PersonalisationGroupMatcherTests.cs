namespace Zone.UmbracoPersonalisationGroups.Common.Tests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.GroupDefinition;
    using Zone.UmbracoPersonalisationGroups.Common.Tests.TestHelpers;

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
                    Definitions.MatchingDayOfWeekDefinition(),
                    Definitions.MatchingTimeOfDayDefinition(),
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
                    Definitions.MatchingDayOfWeekDefinition(),
                    Definitions.MatchingTimeOfDayDefinition(),
                    Definitions.NonMatchingDayOfWeekDefinition(),
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
                    Definitions.NonMatchingDayOfWeekDefinition(),
                    Definitions.MatchingDayOfWeekDefinition(),
                    Definitions.MatchingTimeOfDayDefinition(),
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
                    Definitions.MatchingDayOfWeekDefinition(),
                    Definitions.NonMatchingDayOfWeekDefinition(),
                    Definitions.MatchingTimeOfDayDefinition(),
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
            var definitionDetail = Definitions.MatchingDayOfWeekDefinition();

            // Act
            var result = PersonalisationGroupMatcher.IsMatch(definitionDetail);

            // Arrange
            Assert.IsTrue(result);
        }        
    }
}
