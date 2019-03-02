namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria.Host
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.Host;

    [TestClass]
    public class HostPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"value\": \"{0}\", \"match\": \"{1}\" }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HostPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HostPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void HostPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForHostMatches_WithMatchingValue_ReturnsTrue()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = string.Format(DefinitionFormat, "http://www.example.com/", "MatchesValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HostPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForHostMatches_WithNonMatchingValue_ReturnsFalse()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = string.Format(DefinitionFormat, "http://www.another-example.com/", "MatchesValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HostPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForHostDoesNotMatch_WithNonMatchingValue_ReturnsTrue()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = string.Format(DefinitionFormat, "http://www.another-example.com/", "DoesNotMatchValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HostPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForHostDoesNotMatch_WithMatchingValue_ReturnsFalse()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = string.Format(DefinitionFormat, "http://www.example.com/", "DoesNotMatchValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void HostPersonalisationGroupCriteria_ContainsVisitor_WithDefinitionForHostContains_WithMatchingValue_ReturnsTrue()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = string.Format(DefinitionFormat, "example", "ContainsValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HostPersonalisationGroupCriteria_ContainsVisitor_WithDefinitionForHostContains_WithNonMatchingValue_ReturnsFalse()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = string.Format(DefinitionFormat, "another-example", "ContainsValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HostPersonalisationGroupCriteria_ContainsVisitor_WithDefinitionForHostDoesNotMatch_WithNonMatchingValue_ReturnsTrue()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = string.Format(DefinitionFormat, "another-example", "DoesNotContainValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HostPersonalisationGroupCriteria_ContainsVisitor_WithDefinitionForHostDoesNotMatch_WithMatchingValue_ReturnsFalse()
        {
            // Arrange
            var mockHostProvider = MockHostProvider();
            var criteria = new HostPersonalisationGroupCriteria(mockHostProvider.Object);
            var definition = string.Format(DefinitionFormat, "example", "DoesNotContainValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        #region Mocks

        private static Mock<IHostProvider> MockHostProvider()
        {
            var mock = new Mock<IHostProvider>();

            mock.Setup(x => x.GetHost()).Returns("http://www.example.com/");

            return mock;
        }

        #endregion
    }
}
