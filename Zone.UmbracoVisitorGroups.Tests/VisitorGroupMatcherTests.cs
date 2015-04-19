namespace Zone.UmbracoVisitorGroups.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VisitorGroupMatcherTests
    {
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void VisitorGroupMatcher_IsMatch_WithMissingCritieria_ThrowsException()
        {
            // Arrange
            var definitionDetail = new VisitorGroupDefinitionDetail
            {
                Alias = "invalidAlias",
                Definition = string.Empty,
            };

            // Act
            VisitorGroupMatcher.IsMatch(definitionDetail);
        }

        [TestMethod]
        public void VisitorGroupMatcher_IsMatch_WithMatchingCriteria_ReturnsTrue()
        {
            // Arrange
            var definitionDetail = new VisitorGroupDefinitionDetail
            {
                Alias = "dayOfWeek",
                Definition = string.Format("[ {0} ]", 
                    (int)(DateTime.Now.DayOfWeek)),
            };

            // Act
            var result = VisitorGroupMatcher.IsMatch(definitionDetail);

            // Arrange
            Assert.IsTrue(result);
        }
    }
}
