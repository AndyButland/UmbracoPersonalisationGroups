namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria.DayOfWeek
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.DayOfWeek;

    [TestClass]
    public class DayOfWeekPersonalisationGroupCriteriaTests : DateTimeCriteriaTestsBase
    {
        private const string DefinitionFormat = "[ {0}, {1} ]";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new DayOfWeekPersonalisationGroupCriteria(mockDateTimeProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new DayOfWeekPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyDays_ReturnsFalse()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new DayOfWeekPersonalisationGroupCriteria(mockDateTimeProvider.Object);
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
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new DayOfWeekPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = string.Format(DefinitionFormat, 2, 3); // Monday, Tuesday

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DayOfWeekPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingDays_ReturnsTrue()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new DayOfWeekPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = string.Format(DefinitionFormat, 6, 7); // Friday, Saturday

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
