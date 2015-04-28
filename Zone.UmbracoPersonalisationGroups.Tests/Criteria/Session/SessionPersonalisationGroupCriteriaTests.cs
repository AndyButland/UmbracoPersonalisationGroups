namespace Zone.UmbracoPersonalisationGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.Session;

    [TestClass]
    public class SessionPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"key\": \"{0}\", \"match\": \"{1}\", \"value\": \"{2}\" }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForSessionExists_WithExistingSession_ReturnsTrue()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "Exists", string.Empty);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForSessionExists_WithMissingSession_ReturnsFalse()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = string.Format(DefinitionFormat, "missing-key", "Exists", string.Empty);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForSessionAbsent_WithAbsentSession_ReturnsTrue()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = string.Format(DefinitionFormat, "missing-key", "DoesNotExist", string.Empty);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForSessionAbsent_WithExistingSession_ReturnsFalse()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "DoesNotExist", string.Empty);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForSessionMatchingValue_WithMatchingSession_ReturnsTrue()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "MatchesValue", "aaa,bbb,ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForSessionMatchingValue_WithNonMatchingSession_ReturnsFalse()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "MatchesValue", "aaa,bbb,xxx");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForSessionContainingValue_WithMatchingSession_ReturnsTrue()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "ContainsValue", "bbb");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SessionPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForSessionContainingValue_WithNonMatchingSession_ReturnsFalse()
        {
            // Arrange
            var mockSessionProvider = MockSessionProvider();
            var criteria = new SessionPersonalisationGroupCriteria(mockSessionProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "ContainsValue", "xxx");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        #region Mocks

        protected static Mock<ISessionProvider> MockSessionProvider()
        {
            var mock = new Mock<ISessionProvider>();

            mock.Setup(x => x.KeyExists(It.Is<string>(y => y == "key"))).Returns(true);
            mock.Setup(x => x.KeyExists(It.Is<string>(y => y == "missing-key"))).Returns(false);
            mock.Setup(x => x.GetValue(It.Is<string>(y => y == "key"))).Returns("aaa,bbb,ccc");

            return mock;
        }

        #endregion
    }
}
