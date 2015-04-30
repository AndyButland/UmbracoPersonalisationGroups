namespace Zone.UmbracoPersonalisationGroups.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.MemberGroup;

    [TestClass]
    public class MemberGroupPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"groupName\": \"{0}\", \"match\": \"{1}\" }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MemberGroupPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockMemberGroupProvider = MockMemberGroupProvider();
            var criteria = new MemberGroupPersonalisationGroupCriteria(mockMemberGroupProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MemberGroupPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockMemberGroupProvider = MockMemberGroupProvider();
            var criteria = new MemberGroupPersonalisationGroupCriteria(mockMemberGroupProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void MemberGroupPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionMatchingGroup_WithMatchingGroup_ReturnsTrue()
        {
            // Arrange
            var mockMemberGroupProvider = MockMemberGroupProvider();
            var criteria = new MemberGroupPersonalisationGroupCriteria(mockMemberGroupProvider.Object);
            var definition = string.Format(DefinitionFormat, "Group A", "IsInGroup");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberGroupPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionMatchingGroup_WithNonMatchingGroup_ReturnsFalse()
        {
            // Arrange
            var mockMemberGroupProvider = MockMemberGroupProvider("Group B");
            var criteria = new MemberGroupPersonalisationGroupCriteria(mockMemberGroupProvider.Object);
            var definition = string.Format(DefinitionFormat, "Group A", "IsInGroup");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberGroupPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionNotMatchingGroup_WithMatchingGroup_ReturnsFalse()
        {
            // Arrange
            var mockMemberGroupProvider = MockMemberGroupProvider();
            var criteria = new MemberGroupPersonalisationGroupCriteria(mockMemberGroupProvider.Object);
            var definition = string.Format(DefinitionFormat, "Group A", "IsNotInGroup");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberGroupPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionNotMatchingGroup_WithNonMatchingGroup_ReturnsTrue()
        {
            // Arrange
            var mockMemberGroupProvider = MockMemberGroupProvider("Group B");
            var criteria = new MemberGroupPersonalisationGroupCriteria(mockMemberGroupProvider.Object);
            var definition = string.Format(DefinitionFormat, "Group A", "IsNotInGroup");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        #region Mocks

        private static Mock<IMemberGroupProvider> MockMemberGroupProvider(string groupName = "Group A")
        {
            var mock = new Mock<IMemberGroupProvider>();

            mock.Setup(x => x.GetMemberGroups()).Returns(new string[] { groupName });

            return mock;
        }

        #endregion
    }
}