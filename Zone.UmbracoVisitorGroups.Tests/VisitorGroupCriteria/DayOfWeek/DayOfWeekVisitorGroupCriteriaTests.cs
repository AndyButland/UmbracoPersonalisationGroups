namespace Zone.UmbracoVisitorGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoVisitorGroups.VisitorGroupCriteria;

    [TestClass]
    public class DayOfWeekVisitorGroupCriteriaTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DayOfWeekVisitorGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var criteria = new DayOfWeekVisitorGroupCriteria();

            // Act
            criteria.MatchesVisitor((string) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DayOfWeekVisitorGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var criteria = new DayOfWeekVisitorGroupCriteria();
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void DayOfWeekVisitorGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyDays_ReturnsFalse()
        {
            // Arrange
            var criteria = new DayOfWeekVisitorGroupCriteria();
            var definition = "[]";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DayOfWeekVisitorGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentDays_ReturnsFalse()
        {
            // Arrange
            var criteria = new DayOfWeekVisitorGroupCriteria();
            var definition = string.Format("[ {0}, {1} ]", 
                (int)(DateTime.Now.AddDays(1).DayOfWeek), 
                (int)(DateTime.Now.AddDays(2).DayOfWeek));

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DayOfWeekVisitorGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingDays_ReturnsTrue()
        {
            // Arrange
            var criteria = new DayOfWeekVisitorGroupCriteria();
            var definition = string.Format("[ {0}, {1} ]", 
                (int)(DateTime.Now.DayOfWeek), 
                (int)(DateTime.Now.AddDays(1).DayOfWeek));

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
