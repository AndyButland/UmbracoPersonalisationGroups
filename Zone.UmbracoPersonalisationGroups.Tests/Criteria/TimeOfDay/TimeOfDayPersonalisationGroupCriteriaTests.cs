namespace Zone.UmbracoPersonalisationGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Criteria.TimeOfDay;

    [TestClass]
    public class TimeOfDayPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "[ {{ \"from\": {0}, \"to\": {1} }}, {{ \"from\": {2}, \"to\": {3} }} ]";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var criteria = new TimeOfDayPersonalisationGroupCriteria();

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var criteria = new TimeOfDayPersonalisationGroupCriteria();
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyDays_ReturnsFalse()
        {
            // Arrange
            var criteria = new TimeOfDayPersonalisationGroupCriteria();
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
            var criteria = new TimeOfDayPersonalisationGroupCriteria();
            var definition = string.Format(DefinitionFormat,
                DateTime.Now.AddHours(2).ToString("HHmm"),
                DateTime.Now.AddHours(2).AddMinutes(30).ToString("HHmm"),
                DateTime.Now.AddHours(4).ToString("HHmm"),
                DateTime.Now.AddHours(4).AddMinutes(30).ToString("HHmm"));

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TimeOfDayPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingTimePeriods_ReturnsTrue()
        {
            // Arrange
            var criteria = new TimeOfDayPersonalisationGroupCriteria();
            var definition = string.Format(DefinitionFormat,
                DateTime.Now.AddMinutes(-1).ToString("HHmm"),
                DateTime.Now.AddMinutes(30).ToString("HHmm"),
                DateTime.Now.AddHours(4).ToString("HHmm"),
                DateTime.Now.AddHours(4).AddMinutes(30).ToString("HHmm"));

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
