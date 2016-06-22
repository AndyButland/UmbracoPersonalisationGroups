namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.NumberOfVisits
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.NumberOfVisits;

    [TestClass]
    public class NumberOfVisitsPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"match\": \"{0}\", \"number\": {1} }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NumberOfVisitsPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockNumberOfVisitsProvider = MockNumberOfVisitsProvider();
            var criteria = new NumberOfVisitsPersonalisationGroupCriteria(mockNumberOfVisitsProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NumberOfVisitsPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockNumberOfVisitsProvider = MockNumberOfVisitsProvider();
            var criteria = new NumberOfVisitsPersonalisationGroupCriteria(mockNumberOfVisitsProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void NumberOfVisitsPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForMoreThanNumber_WithMoreThanTheNumberOfVisits_ReturnsTrue()
        {
            // Arrange
            var mockNumberOfVisitsProvider = MockNumberOfVisitsProvider(3);
            var criteria = new NumberOfVisitsPersonalisationGroupCriteria(mockNumberOfVisitsProvider.Object);
            var definition = string.Format(DefinitionFormat, "MoreThan", 2);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NumberOfVisitsPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForMoreThanNumber_WithLessThanTheNumberOfVisits_ReturnsFalse()
        {
            // Arrange
            var mockNumberOfVisitsProvider = MockNumberOfVisitsProvider(1);
            var criteria = new NumberOfVisitsPersonalisationGroupCriteria(mockNumberOfVisitsProvider.Object);
            var definition = string.Format(DefinitionFormat, "MoreThan", 2);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NumberOfVisitsPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanNumber_WithLessThanTheNumberOfVisits_ReturnsTrue()
        {
            // Arrange
            var mockNumberOfVisitsProvider = MockNumberOfVisitsProvider(1);
            var criteria = new NumberOfVisitsPersonalisationGroupCriteria(mockNumberOfVisitsProvider.Object);
            var definition = string.Format(DefinitionFormat, "LessThan", 2);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NumberOfVisitsPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanNumber_WithMoreThanTheNumberOfVisits_ReturnsFalse()
        {
            // Arrange
            var mockNumberOfVisitsProvider = MockNumberOfVisitsProvider(3);
            var criteria = new NumberOfVisitsPersonalisationGroupCriteria(mockNumberOfVisitsProvider.Object);
            var definition = string.Format(DefinitionFormat, "LessThan", 2);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NumberOfVisitsPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForExactlyNumber_WithExactlyTheNumberOfVisits_ReturnsTrue()
        {
            // Arrange
            var mockNumberOfVisitsProvider = MockNumberOfVisitsProvider(2);
            var criteria = new NumberOfVisitsPersonalisationGroupCriteria(mockNumberOfVisitsProvider.Object);
            var definition = string.Format(DefinitionFormat, "Exactly", 2);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NumberOfVisitsPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForExactlyNumber_WithNotExactlyTheNumberOfVisits_ReturnsFalse()
        {
            // Arrange
            var mockNumberOfVisitsProvider = MockNumberOfVisitsProvider(3);
            var criteria = new NumberOfVisitsPersonalisationGroupCriteria(mockNumberOfVisitsProvider.Object);
            var definition = string.Format(DefinitionFormat, "Exactly", 2);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        #region Mocks

        private static Mock<INumberOfVisitsProvider> MockNumberOfVisitsProvider(int result = 0)
        {
            var mock = new Mock<INumberOfVisitsProvider>();

            mock.Setup(x => x.GetNumberOfVisits()).Returns(result);

            return mock;
        }

        #endregion
    }
}
