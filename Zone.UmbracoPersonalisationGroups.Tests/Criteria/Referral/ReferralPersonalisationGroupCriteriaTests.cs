namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.Referral
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.Referral;

    [TestClass]
    public class ReferralPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"value\": \"{0}\", \"match\": \"{1}\" }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReferralPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReferralPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void ReferralPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForReferrerMatches_WithMatchingValue_ReturnsTrue()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = string.Format(DefinitionFormat, "http://www.example.com/", "MatchesValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReferralPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForReferrerMatches_WithNonMatchingValue_ReturnsFalse()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = string.Format(DefinitionFormat, "http://www.another-example.com/", "MatchesValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReferralPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForReferrerDoesNotMatch_WithNonMatchingValue_ReturnsTrue()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = string.Format(DefinitionFormat, "http://www.another-example.com/", "DoesNotMatchValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReferralPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForReferrerDoesNotMatch_WithMatchingValue_ReturnsFalse()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = string.Format(DefinitionFormat, "http://www.example.com/", "DoesNotMatchValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void ReferralPersonalisationGroupCriteria_ContainsVisitor_WithDefinitionForReferrerContains_WithMatchingValue_ReturnsTrue()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = string.Format(DefinitionFormat, "example", "ContainsValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReferralPersonalisationGroupCriteria_ContainsVisitor_WithDefinitionForReferrerContains_WithNonMatchingValue_ReturnsFalse()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = string.Format(DefinitionFormat, "another-example", "ContainsValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReferralPersonalisationGroupCriteria_ContainsVisitor_WithDefinitionForReferrerDoesNotMatch_WithNonMatchingValue_ReturnsTrue()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = string.Format(DefinitionFormat, "another-example", "DoesNotContainValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReferralPersonalisationGroupCriteria_ContainsVisitor_WithDefinitionForReferrerDoesNotMatch_WithMatchingValue_ReturnsFalse()
        {
            // Arrange
            var mockReferralProvider = MockReferralProvider();
            var criteria = new ReferralPersonalisationGroupCriteria(mockReferralProvider.Object);
            var definition = string.Format(DefinitionFormat, "example", "DoesNotContainValue");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        #region Mocks

        private static Mock<IReferrerProvider> MockReferralProvider()
        {
            var mock = new Mock<IReferrerProvider>();

            mock.Setup(x => x.GetReferrer()).Returns("http://www.example.com/");

            return mock;
        }

        #endregion
    }
}
