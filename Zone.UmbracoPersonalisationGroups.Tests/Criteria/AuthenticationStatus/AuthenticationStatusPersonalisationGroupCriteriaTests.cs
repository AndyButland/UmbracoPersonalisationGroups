namespace Zone.UmbracoPersonalisationGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.AuthenticationStatus;

    [TestClass]
    public class AuthenticationStatusPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"isAuthenticated\": {0} }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthenticationStatusPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockAuthenticationStatusProvider = MockAuthenticationStatusProvider();
            var criteria = new AuthenticationStatusPersonalisationGroupCriteria(mockAuthenticationStatusProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AuthenticationStatusPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockAuthenticationStatusProvider = MockAuthenticationStatusProvider();
            var criteria = new AuthenticationStatusPersonalisationGroupCriteria(mockAuthenticationStatusProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void AuthenticationStatusPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionIsAuthenticated_WithAuthenticatedMember_ReturnsTrue()
        {
            // Arrange
            var mockAuthenticationStatusProvider = MockAuthenticationStatusProvider(true);
            var criteria = new AuthenticationStatusPersonalisationGroupCriteria(mockAuthenticationStatusProvider.Object);
            var definition = string.Format(DefinitionFormat, "true");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AuthenticationStatusPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionIsAuthenticated_WithUnauthenticatedMember_ReturnsTrue()
        {
            // Arrange
            var mockAuthenticationStatusProvider = MockAuthenticationStatusProvider();
            var criteria = new AuthenticationStatusPersonalisationGroupCriteria(mockAuthenticationStatusProvider.Object);
            var definition = string.Format(DefinitionFormat, "true");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AuthenticationStatusPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionIsUnauthenticated_WithAuthenticatedMember_ReturnsFalse()
        {
            // Arrange
            var mockAuthenticationStatusProvider = MockAuthenticationStatusProvider(true);
            var criteria = new AuthenticationStatusPersonalisationGroupCriteria(mockAuthenticationStatusProvider.Object);
            var definition = string.Format(DefinitionFormat, "false");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AuthenticationStatusPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionIsUnauthenticated_WithUnauthenticatedMember_ReturnsTrue()
        {
            // Arrange
            var mockAuthenticationStatusProvider = MockAuthenticationStatusProvider();
            var criteria = new AuthenticationStatusPersonalisationGroupCriteria(mockAuthenticationStatusProvider.Object);
            var definition = string.Format(DefinitionFormat, "false");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        #region Mocks

        protected static Mock<IAuthenticationStatusProvider> MockAuthenticationStatusProvider(bool authenticationStatus = false)
        {
            var mock = new Mock<IAuthenticationStatusProvider>();

            mock.Setup(x => x.IsAuthenticated()).Returns(authenticationStatus);

            return mock;
        }

        #endregion
    }
}