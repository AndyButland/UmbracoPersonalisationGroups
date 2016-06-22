namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.MemberProfileField
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

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanDateValue_WithMatchingMemberProfileField_ReturnsTrue()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "GreaterThanValue", "1-APR-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanDateValue_WithNonMatchingMemberProfileField_ReturnsFalse()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "GreaterThanValue", "1-JUN-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanNumericValue_WithMatchingMemberProfileField_ReturnsTrue()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "GreaterThanValue", "3");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanNumericValue_WithNonMatchingMemberProfileField_ReturnsFalse()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "GreaterThanValue", "7");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanStringValue_WithMatchingMemberProfileField_ReturnsTrue()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "GreaterThanValue", "aaa");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanStringValue_WithNonMatchingMemberProfileField_ReturnsFalse()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "GreaterThanValue", "ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanDateValue_WithMatchingMemberProfileField_ReturnsTrue()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "LessThanValue", "1-JUN-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanDateValue_WithNonMatchingMemberProfileField_ReturnsFalse()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "LessThanValue", "1-APR-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanNumericValue_WithMatchingMemberProfileField_ReturnsTrue()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "LessThanValue", "7");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanNumericValue_WithNonMatchingMemberProfileField_ReturnsFalse()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "LessThanValue", "3");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanStringValue_WithMatchingMemberProfileField_ReturnsTrue()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "LessThanValue", "ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MemberProfileFieldPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanStringValue_WithNonMatchingMemberProfileField_ReturnsFalse()
        {
            // Arrange
            var mockMemberProfileFieldProvider = MockMemberProfileFieldProvider();
            var criteria = new MemberProfileFieldPersonalisationGroupCriteria(mockMemberProfileFieldProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "LessThanValue", "aaa");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        #region Mocks

        private static Mock<IMemberProfileFieldProvider> MockMemberProfileFieldProvider()
        {
            var mock = new Mock<IMemberProfileFieldProvider>();

            mock.Setup(x => x.GetMemberProfileFieldValue(It.Is<string>(y => y == "abc"))).Returns("xyz");
            mock.Setup(x => x.GetMemberProfileFieldValue(It.Is<string>(y => y == "dateCompareTest"))).Returns("1-MAY-2015 10:30:00");
            mock.Setup(x => x.GetMemberProfileFieldValue(It.Is<string>(y => y == "numericCompareTest"))).Returns("5");
            mock.Setup(x => x.GetMemberProfileFieldValue(It.Is<string>(y => y == "stringCompareTest"))).Returns("bbb");

            return mock;
        }

        #endregion
    }
}