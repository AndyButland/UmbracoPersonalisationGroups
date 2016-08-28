namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.Querystring
{
    using System;
    using System.Collections.Specialized;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.Querystring;

    [TestClass]
    public class QuerystringPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"key\": \"{0}\", \"match\": \"{1}\", \"value\": \"{2}\" }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }


        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForQuerystringMatchingValue_WithMatchingQuerystring_ReturnsTrue()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "MatchesValue", "aaa,bbb,ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForQuerystringMatchingValue_WithNonMatchingQuerystring_ReturnsFalse()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "MatchesValue", "aaa,bbb,xxx");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForQuerystringContainingValue_WithMatchingQuerystring_ReturnsTrue()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "ContainsValue", "bbb");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForQuerystringContainingValue_WithNonMatchingQuerystring_ReturnsFalse()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "key", "ContainsValue", "xxx");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanDateValue_WithMatchingQuerystring_ReturnsTrue()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "GreaterThanValue", "1-APR-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanDateValue_WithNonMatchingQuerystring_ReturnsFalse()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "GreaterThanValue", "1-JUN-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanNumericValue_WithMatchingQuerystring_ReturnsTrue()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "GreaterThanValue", "3");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanNumericValue_WithNonMatchingQuerystring_ReturnsFalse()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "GreaterThanValue", "7");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanStringValue_WithMatchingQuerystring_ReturnsTrue()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "GreaterThanValue", "aaa");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForGreaterThanStringValue_WithNonMatchingQuerystring_ReturnsFalse()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "GreaterThanValue", "ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanDateValue_WithMatchingQuerystring_ReturnsTrue()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "LessThanValue", "1-JUN-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanDateValue_WithNonMatchingQuerystring_ReturnsFalse()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "dateCompareTest", "LessThanValue", "1-APR-2015");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanNumericValue_WithMatchingQuerystring_ReturnsTrue()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "LessThanValue", "7");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanNumericValue_WithNonMatchingQuerystring_ReturnsFalse()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "numericCompareTest", "LessThanValue", "3");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanStringValue_WithMatchingQuerystring_ReturnsTrue()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "LessThanValue", "ccc");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void QuerystringPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForLessThanStringValue_WithNonMatchingQuerystring_ReturnsFalse()
        {
            // Arrange
            var mockQuerystringProvider = MockQuerystringProvider();
            var criteria = new QuerystringPersonalisationGroupCriteria(mockQuerystringProvider.Object);
            var definition = string.Format(DefinitionFormat, "stringCompareTest", "LessThanValue", "aaa");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        #region Mocks

        private static Mock<IQuerystringProvider> MockQuerystringProvider()
        {
            var mock = new Mock<IQuerystringProvider>();

            mock.Setup(x => x.GetQuerystring()).Returns(new NameValueCollection
            {
                 { "key", "aaa,bbb,ccc"},
                 { "dateCompareTest", "1-MAY-2015 10:30:00" },
                 { "numericCompareTest", "5" },
                 { "stringCompareTest", "bbb" }
            });

            return mock;
        }

        #endregion
    }
}
