namespace Zone.UmbracoVisitorGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoVisitorGroups.VisitorGroupCriteria.TimeOfDay;

    [TestClass]
    public class TimeOfDayVisitorGroupCriteriaTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeOfDayVisitorGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var criteria = new TimeOfDayVisitorGroupCriteria();

            // Act
            criteria.MatchesVisitor((string) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TimeOfDayVisitorGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var criteria = new TimeOfDayVisitorGroupCriteria();
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void TimeOfDayVisitorGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyDays_ReturnsFalse()
        {
            // Arrange
            var criteria = new TimeOfDayVisitorGroupCriteria();
            var definition = "[]";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TimeOfDayVisitorGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentTimePeriods_ReturnsFalse()
        {
            // Arrange
            var criteria = new TimeOfDayVisitorGroupCriteria();
            var definition = string.Format("[ {{ \"from\": {0}, \"to\": {1} }}, {{ \"from\": {2}, \"to\": {3} }} ]",
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
        public void TimeOfDayVisitorGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingTimePeriods_ReturnsTrue()
        {
            // Arrange
            var criteria = new TimeOfDayVisitorGroupCriteria();
            var definition = string.Format("[ {{ \"from\": {0}, \"to\": {1} }}, {{ \"from\": {2}, \"to\": {3} }} ]",
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
