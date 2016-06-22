namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.TimeOfDay
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria;
    using Zone.UmbracoPersonalisationGroups.Criteria.TimeOfDay;

    [TestClass]
    public class TimeOfDayPersonalisationGroupCriteriaTests : DateTimeCriteriaTestsBase
    {
        private const string DefinitionFormat = "[ {{ \"from\": \"{0}\", \"to\": \"{1}\" }}, {{ \"from\": \"{2}\", \"to\": \"{3}\" }} ]";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new TimeOfDayPersonalisationGroupCriteria(mockDateTimeProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new TimeOfDayPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyDays_ReturnsFalse()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new TimeOfDayPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = "[]";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentTimePeriods_ReturnsFalse()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new TimeOfDayPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = string.Format(DefinitionFormat, "0900", "0930", "1100", "1130");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingTimePeriods_ReturnsTrue()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider();
            var criteria = new TimeOfDayPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = string.Format(DefinitionFormat, "0900", "1015", "1100", "1130");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingTimePeriodsInLastMinute_ReturnsTrue()
        {
            // Arrange
            var mockDateTimeProvider = MockDateTimeProvider(new DateTime(2016, 1, 1, 23, 59, 59));
            var criteria = new TimeOfDayPersonalisationGroupCriteria(mockDateTimeProvider.Object);
            var definition = string.Format(DefinitionFormat, "0900", "0930", "2300", "2359");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
