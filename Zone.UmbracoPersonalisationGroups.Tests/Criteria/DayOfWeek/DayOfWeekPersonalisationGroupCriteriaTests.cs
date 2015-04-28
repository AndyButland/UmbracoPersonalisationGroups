namespace Zone.UmbracoPersonalisationGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Criteria.DayOfWeek;

    [TestClass]
    public class DayOfWeekPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "[ {0}, {1} ]";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var criteria = new DayOfWeekPersonalisationGroupCriteria();

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var criteria = new DayOfWeekPersonalisationGroupCriteria();
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyDays_ReturnsFalse()
        {
            // Arrange
            var criteria = new DayOfWeekPersonalisationGroupCriteria();
            var definition = "[]";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentDays_ReturnsFalse()
        {
            // Arrange
            var criteria = new DayOfWeekPersonalisationGroupCriteria();
            var definition = string.Format(DefinitionFormat,
                (int)DateTime.Now.AddDays(1).DayOfWeek,
                (int)DateTime.Now.AddDays(2).DayOfWeek);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingDays_ReturnsTrue()
        {
            // Arrange
            var criteria = new DayOfWeekPersonalisationGroupCriteria();
            var definition = string.Format(DefinitionFormat,
                (int)DateTime.Now.DayOfWeek,
                (int)DateTime.Now.AddDays(1).DayOfWeek);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
