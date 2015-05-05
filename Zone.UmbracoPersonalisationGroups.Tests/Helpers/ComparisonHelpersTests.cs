namespace Zone.UmbracoPersonsalisationGroups.Tests.Helpers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UmbracoPersonalisationGroups.Criteria.Session;
    using UmbracoPersonalisationGroups.Helpers;
    using Zone.UmbracoPersonalisationGroups;

    [TestClass]
    public class ComparisonHelpersTests
    {
        [TestMethod]
        public void ComparisonHelpers_CompareValues_DateComparisons_ReturnsCorrectValues()
        {
            // Arrange
            const string value1 = "1-APR-2015";
            const string value2 = "1-MAY-2015";
            const string value3 = "1-JUN-2015";

            // Act
            var result = ComparisonHelpers.CompareValues(value2, value1, Comparison.GreaterThan);
            var result2 = ComparisonHelpers.CompareValues(value2, value2, Comparison.GreaterThan);
            var result3 = ComparisonHelpers.CompareValues(value2, value3, Comparison.GreaterThan);

            var result4 = ComparisonHelpers.CompareValues(value2, value1, Comparison.GreaterThanOrEqual);
            var result5 = ComparisonHelpers.CompareValues(value2, value2, Comparison.GreaterThanOrEqual);
            var result6 = ComparisonHelpers.CompareValues(value2, value3, Comparison.GreaterThanOrEqual);

            var result7 = ComparisonHelpers.CompareValues(value2, value1, Comparison.LessThan);
            var result8 = ComparisonHelpers.CompareValues(value2, value2, Comparison.LessThan);
            var result9 = ComparisonHelpers.CompareValues(value2, value3, Comparison.LessThan);

            var result10 = ComparisonHelpers.CompareValues(value2, value1, Comparison.LessThanOrEqual);
            var result11 = ComparisonHelpers.CompareValues(value2, value2, Comparison.LessThanOrEqual);
            var result12 = ComparisonHelpers.CompareValues(value2, value3, Comparison.LessThanOrEqual);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);

            Assert.IsTrue(result4);
            Assert.IsTrue(result5);
            Assert.IsFalse(result6);

            Assert.IsFalse(result7);
            Assert.IsFalse(result8);
            Assert.IsTrue(result9);

            Assert.IsFalse(result10);
            Assert.IsTrue(result11);
            Assert.IsTrue(result12);
        }

        [TestMethod]
        public void ComparisonHelpers_CompareValues_NumericComparisons_ReturnsCorrectValues()
        {
            // Arrange
            const string value1 = "1.1";
            const string value2 = "2.2";
            const string value3 = "3.3";

            // Act
            var result = ComparisonHelpers.CompareValues(value2, value1, Comparison.GreaterThan);
            var result2 = ComparisonHelpers.CompareValues(value2, value2, Comparison.GreaterThan);
            var result3 = ComparisonHelpers.CompareValues(value2, value3, Comparison.GreaterThan);

            var result4 = ComparisonHelpers.CompareValues(value2, value1, Comparison.GreaterThanOrEqual);
            var result5 = ComparisonHelpers.CompareValues(value2, value2, Comparison.GreaterThanOrEqual);
            var result6 = ComparisonHelpers.CompareValues(value2, value3, Comparison.GreaterThanOrEqual);

            var result7 = ComparisonHelpers.CompareValues(value2, value1, Comparison.LessThan);
            var result8 = ComparisonHelpers.CompareValues(value2, value2, Comparison.LessThan);
            var result9 = ComparisonHelpers.CompareValues(value2, value3, Comparison.LessThan);

            var result10 = ComparisonHelpers.CompareValues(value2, value1, Comparison.LessThanOrEqual);
            var result11 = ComparisonHelpers.CompareValues(value2, value2, Comparison.LessThanOrEqual);
            var result12 = ComparisonHelpers.CompareValues(value2, value3, Comparison.LessThanOrEqual);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);

            Assert.IsTrue(result4);
            Assert.IsTrue(result5);
            Assert.IsFalse(result6);

            Assert.IsFalse(result7);
            Assert.IsFalse(result8);
            Assert.IsTrue(result9);

            Assert.IsFalse(result10);
            Assert.IsTrue(result11);
            Assert.IsTrue(result12);
        }

        [TestMethod]
        public void ComparisonHelpers_CompareValues_StringComparisons_ReturnsCorrectValues()
        {
            // Arrange
            const string value1 = "a";
            const string value2 = "b";
            const string value3 = "c";

            // Act
            var result = ComparisonHelpers.CompareValues(value2, value1, Comparison.GreaterThan);
            var result2 = ComparisonHelpers.CompareValues(value2, value2, Comparison.GreaterThan);
            var result3 = ComparisonHelpers.CompareValues(value2, value3, Comparison.GreaterThan);

            var result4 = ComparisonHelpers.CompareValues(value2, value1, Comparison.GreaterThanOrEqual);
            var result5 = ComparisonHelpers.CompareValues(value2, value2, Comparison.GreaterThanOrEqual);
            var result6 = ComparisonHelpers.CompareValues(value2, value3, Comparison.GreaterThanOrEqual);

            var result7 = ComparisonHelpers.CompareValues(value2, value1, Comparison.LessThan);
            var result8 = ComparisonHelpers.CompareValues(value2, value2, Comparison.LessThan);
            var result9 = ComparisonHelpers.CompareValues(value2, value3, Comparison.LessThan);

            var result10 = ComparisonHelpers.CompareValues(value2, value1, Comparison.LessThanOrEqual);
            var result11 = ComparisonHelpers.CompareValues(value2, value2, Comparison.LessThanOrEqual);
            var result12 = ComparisonHelpers.CompareValues(value2, value3, Comparison.LessThanOrEqual);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);

            Assert.IsTrue(result4);
            Assert.IsTrue(result5);
            Assert.IsFalse(result6);

            Assert.IsFalse(result7);
            Assert.IsFalse(result8);
            Assert.IsTrue(result9);

            Assert.IsFalse(result10);
            Assert.IsTrue(result11);
            Assert.IsTrue(result12);
        }
    }
}
