namespace Zone.UmbracoPersonalisationGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.MemberProfileField;

    [TestClass]
    public class MemberProfileFieldPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"alias\": \"{0}\", \"match\": \"{1}\", \"value\": \"{2}\" }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionMatchingProfileField_WithMatchingField_ReturnsTrue()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "abc", "MatchesValue", "xyz");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionMatchingProfileField_WithNonMatchingField_ReturnsFalse()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "abc", "MatchesValue", "zyx");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionNotMatchingProfileField_WithMatchingField_ReturnsFalse()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "abc", "DoesNotMatchValue", "xyz");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionMatchingProfileField_WithNonMatchingField_ReturnsTrue()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "abc", "DoesNotMatchValue", "zyx");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        #region Mocks

        private static Mock<IMemberProfileFieldProvider> MockMemberProfileFieldProvider()
        {
            var mock = new Mock<IMemberProfileFieldProvider>();

            mock.Setup(x => x.GetMemberProfileFieldValue(It.Is<string>(y => y == "abc"))).Returns("xyz");

            return mock;
        }

        #endregion
    }
}