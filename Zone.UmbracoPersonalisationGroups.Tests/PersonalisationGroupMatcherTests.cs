namespace Zone.UmbracoPersonalisationGroups.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PersonalisationGroupMatcherTests
    {
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void PersonalisationGroupMatcher_IsMatch_WithMissingCritieria_ThrowsException()
        {
            // Arrange
            var definitionDetail = new PersonalisationGroupDefinitionDetail
            {
                Alias = "invalidAlias",
                Definition = string.Empty,
            };

            // Act
            PersonalisationGroupMatcher.IsMatch(definitionDetail);
        }

        [TestMethod]
        public void PersonalisationGroupMatcher_IsMatch_WithMatchingCriteria_ReturnsTrue()
        {
            // Arrange
            var definitionDetail = new PersonalisationGroupDefinitionDetail
            {
                Alias = "dayOfWeek",
                Definition = string.Format("[ {0} ]", 
                    (int)(DateTime.Now.DayOfWeek)),
            };

            // Act
            var result = PersonalisationGroupMatcher.IsMatch(definitionDetail);

            // Arrange
            Assert.IsTrue(result);
        }
    }
}
