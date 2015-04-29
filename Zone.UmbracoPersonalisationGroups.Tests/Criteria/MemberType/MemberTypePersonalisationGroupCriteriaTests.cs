namespace Zone.UmbracoPersonalisationGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.MemberType;

    [TestClass]
    public class MemberTypePersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"typeName\": \"{0}\", \"match\": \"{1}\" }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MemberTypePersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockMemberTypeProvider = MockMemberTypeProvider();
            var criteria = new MemberTypePersonalisationGroupCriteria(mockMemberTypeProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MemberTypePersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockMemberTypeProvider = MockMemberTypeProvider();
            var criteria = new MemberTypePersonalisationGroupCriteria(mockMemberTypeProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void MemberTypePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionMatchingType_WithMatchingType_ReturnsTrue()
        {
            // Arrange
            var mockMemberTypeProvider = MockMemberTypeProvider();
            var criteria = new MemberTypePersonalisationGroupCriteria(mockMemberTypeProvider.Object);
            var definition = string.Format(DefinitionFormat, "Member", "IsOfType");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberTypePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionMatchingType_WithNonMatchingType_ReturnsFalse()
        {
            // Arrange
            var mockMemberTypeProvider = MockMemberTypeProvider("anotherType");
            var criteria = new MemberTypePersonalisationGroupCriteria(mockMemberTypeProvider.Object);
            var definition = string.Format(DefinitionFormat, "Member", "IsOfType");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberTypePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionNotMatchingType_WithMatchingType_ReturnsFalse()
        {
            // Arrange
            var mockMemberTypeProvider = MockMemberTypeProvider();
            var criteria = new MemberTypePersonalisationGroupCriteria(mockMemberTypeProvider.Object);
            var definition = string.Format(DefinitionFormat, "Member", "IsNotOfType");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberTypePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionNotMatchingType_WithNonMatchingType_ReturnsTrue()
        {
            // Arrange
            var mockMemberTypeProvider = MockMemberTypeProvider("anotherType");
            var criteria = new MemberTypePersonalisationGroupCriteria(mockMemberTypeProvider.Object);
            var definition = string.Format(DefinitionFormat, "Member", "IsNotOfType");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        #region Mocks

        protected static Mock<IMemberTypeProvider> MockMemberTypeProvider(string typeName = "member")
        {
            var mock = new Mock<IMemberTypeProvider>();

            mock.Setup(x => x.GetMemberType()).Returns(typeName);

            return mock;
        }

        #endregion
    }
}