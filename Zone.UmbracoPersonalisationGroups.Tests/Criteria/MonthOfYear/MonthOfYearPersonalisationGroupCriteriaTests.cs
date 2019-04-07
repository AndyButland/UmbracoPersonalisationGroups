namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria.MonthOfYear
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.MonthOfYear;

    [TestClass]
    public class MonthOfYearPersonalisationGroupCriteriaTests : DateTimeCriteriaTestsBase
    {
        private const string DefinitionFormat = "[ {0}, {1} ]";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MonthOfYearPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new MonthOfYearPersonalisationGroupCriteria(mockDateTimeProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MonthOfYearPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new MonthOfYearPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void MonthOfYearPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyDays_ReturnsFalse()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new MonthOfYearPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = "[]";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MonthOfYearPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentDays_ReturnsFalse()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new MonthOfYearPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = string.Format(DefinitionFormat, 2, 3); // February, March

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MonthOfYearPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingDays_ReturnsTrue()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new MonthOfYearPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = string.Format(DefinitionFormat, 6, 7); // June, July

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
