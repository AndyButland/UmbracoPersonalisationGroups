namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria;

    [TestClass]
    public class PersonalisationGroupCriteriaBaseTests
    {
        private StubPersonalisationGroupCriteria _criteria;

        private class StubPersonalisationGroupCriteria : PersonalisationGroupCriteriaBase
        {
            public bool TestCompareValues(string value, string definitionValue, Comparison comparison)
            {
                return CompareValues(value, definitionValue, comparison);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _criteria = new StubPersonalisationGroupCriteria();
        }

        [TestMethod]
        public void PersonalisationGroupCriteriaBase_TestCompareValues_DateComparisons_ReturnsCorrectValues()
        {
            // Arrange
            const string value1 = "1-APR-2015";
            const string value2 = "1-MAY-2015";
            const string value3 = "1-JUN-2015";

            // Act
            var result = _criteria.TestCompareValues(value2, value1, Comparison.GreaterThan);
            var result2 = _criteria.TestCompareValues(value2, value2, Comparison.GreaterThan);
            var result3 = _criteria.TestCompareValues(value2, value3, Comparison.GreaterThan);

            var result4 = _criteria.TestCompareValues(value2, value1, Comparison.GreaterThanOrEqual);
            var result5 = _criteria.TestCompareValues(value2, value2, Comparison.GreaterThanOrEqual);
            var result6 = _criteria.TestCompareValues(value2, value3, Comparison.GreaterThanOrEqual);

            var result7 = _criteria.TestCompareValues(value2, value1, Comparison.LessThan);
            var result8 = _criteria.TestCompareValues(value2, value2, Comparison.LessThan);
            var result9 = _criteria.TestCompareValues(value2, value3, Comparison.LessThan);

            var result10 = _criteria.TestCompareValues(value2, value1, Comparison.LessThanOrEqual);
            var result11 = _criteria.TestCompareValues(value2, value2, Comparison.LessThanOrEqual);
            var result12 = _criteria.TestCompareValues(value2, value3, Comparison.LessThanOrEqual);

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
        public void PersonalisationGroupCriteriaBase_TestCompareValues_NumericComparisons_ReturnsCorrectValues()
        {
            // Arrange
            const string value1 = "1.1";
            const string value2 = "2.2";
            const string value3 = "3.3";

            // Act
            var result = _criteria.TestCompareValues(value2, value1, Comparison.GreaterThan);
            var result2 = _criteria.TestCompareValues(value2, value2, Comparison.GreaterThan);
            var result3 = _criteria.TestCompareValues(value2, value3, Comparison.GreaterThan);

            var result4 = _criteria.TestCompareValues(value2, value1, Comparison.GreaterThanOrEqual);
            var result5 = _criteria.TestCompareValues(value2, value2, Comparison.GreaterThanOrEqual);
            var result6 = _criteria.TestCompareValues(value2, value3, Comparison.GreaterThanOrEqual);

            var result7 = _criteria.TestCompareValues(value2, value1, Comparison.LessThan);
            var result8 = _criteria.TestCompareValues(value2, value2, Comparison.LessThan);
            var result9 = _criteria.TestCompareValues(value2, value3, Comparison.LessThan);

            var result10 = _criteria.TestCompareValues(value2, value1, Comparison.LessThanOrEqual);
            var result11 = _criteria.TestCompareValues(value2, value2, Comparison.LessThanOrEqual);
            var result12 = _criteria.TestCompareValues(value2, value3, Comparison.LessThanOrEqual);

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
        public void PersonalisationGroupCriteriaBase_TestCompareValues_StringComparisons_ReturnsCorrectValues()
        {
            // Arrange
            const string value1 = "a";
            const string value2 = "b";
            const string value3 = "c";

            // Act
            var result = _criteria.TestCompareValues(value2, value1, Comparison.GreaterThan);
            var result2 = _criteria.TestCompareValues(value2, value2, Comparison.GreaterThan);
            var result3 = _criteria.TestCompareValues(value2, value3, Comparison.GreaterThan);

            var result4 = _criteria.TestCompareValues(value2, value1, Comparison.GreaterThanOrEqual);
            var result5 = _criteria.TestCompareValues(value2, value2, Comparison.GreaterThanOrEqual);
            var result6 = _criteria.TestCompareValues(value2, value3, Comparison.GreaterThanOrEqual);

            var result7 = _criteria.TestCompareValues(value2, value1, Comparison.LessThan);
            var result8 = _criteria.TestCompareValues(value2, value2, Comparison.LessThan);
            var result9 = _criteria.TestCompareValues(value2, value3, Comparison.LessThan);

            var result10 = _criteria.TestCompareValues(value2, value1, Comparison.LessThanOrEqual);
            var result11 = _criteria.TestCompareValues(value2, value2, Comparison.LessThanOrEqual);
            var result12 = _criteria.TestCompareValues(value2, value3, Comparison.LessThanOrEqual);

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
