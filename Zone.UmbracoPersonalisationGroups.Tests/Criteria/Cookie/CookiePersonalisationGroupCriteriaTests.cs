namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.Cookie
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.Cookie;

    [TestClass]
    public class CookiePersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"key\": \"{0}\", \"match\": \"{1}\", \"value\": \"{2}\" }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForCookieExists_WithExistingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "Exists", string.Empty);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForCookieExists_WithMissingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "missing-key", "Exists", string.Empty);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForCookieAbsent_WithAbsentCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "missing-key", "DoesNotExist", string.Empty);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForCookieAbsent_WithExistingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "DoesNotExist", string.Empty);

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForCookieMatchingValue_WithMatchingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "MatchesValue", "aaa,bbb,ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForCookieMatchingValue_WithNonMatchingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "MatchesValue", "aaa,bbb,xxx");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForCookieContainingValue_WithMatchingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "ContainsValue", "bbb");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForCookieContainingValue_WithNonMatchingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "ContainsValue", "xxx");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanDateValue_WithMatchingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "GreaterThanValue", "1-APR-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanDateValue_WithNonMatchingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "GreaterThanValue", "1-JUN-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanNumericValue_WithMatchingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "GreaterThanValue", "3");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanNumericValue_WithNonMatchingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "GreaterThanValue", "7");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanStringValue_WithMatchingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "GreaterThanValue", "aaa");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanStringValue_WithNonMatchingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "GreaterThanValue", "ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanDateValue_WithMatchingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "LessThanValue", "1-JUN-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanDateValue_WithNonMatchingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "LessThanValue", "1-APR-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanNumericValue_WithMatchingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "LessThanValue", "7");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanNumericValue_WithNonMatchingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "LessThanValue", "3");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanStringValue_WithMatchingCookie_ReturnsTrue()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "LessThanValue", "ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CookiePersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanStringValue_WithNonMatchingCookie_ReturnsFalse()
        {
            // Arrange
            var mockCookieProvider = MockCookieProvider();
            var criteria = new CookiePersonalisationGroupCriteria(mockCookieProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "LessThanValue", "aaa");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        #region Mocks

        private static Mock<ICookieProvider> MockCookieProvider()
        {
            var mock = new Mock<ICookieProvider>();

            mock.Setup(x => x.CookieExists(It.Is<string>(y => y == "key"))).Returns(true);
            mock.Setup(x => x.CookieExists(It.Is<string>(y => y == "dateCompareTest"))).Returns(true);
            mock.Setup(x => x.CookieExists(It.Is<string>(y => y == "numericCompareTest"))).Returns(true);
            mock.Setup(x => x.CookieExists(It.Is<string>(y => y == "stringCompareTest"))).Returns(true);
            mock.Setup(x => x.CookieExists(It.Is<string>(y => y == "missing-key"))).Returns(false);
            mock.Setup(x => x.GetCookieValue(It.Is<string>(y => y == "key"))).Returns("aaa,bbb,ccc");
            mock.Setup(x => x.GetCookieValue(It.Is<string>(y => y == "dateCompareTest"))).Returns("1-MAY-2015 10:30:00");
            mock.Setup(x => x.GetCookieValue(It.Is<string>(y => y == "numericCompareTest"))).Returns("5");
            mock.Setup(x => x.GetCookieValue(It.Is<string>(y => y == "stringCompareTest"))).Returns("bbb");

            return mock;
        }

        #endregion
    }
}
