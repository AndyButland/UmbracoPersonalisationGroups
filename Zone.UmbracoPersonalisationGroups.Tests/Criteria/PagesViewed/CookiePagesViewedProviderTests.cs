namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria.PagesViewed
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed;

    [TestClass]
    public class CookiePagesViewedProviderTests
    {
        [TestMethod]
        public void ParseCookieValue_ShouldParseEmptyValue()
        {
            // Arrange
            var expected = new List<int>();
            var cookieValue = string.Empty;

            // Act
            var actual = CookiePagesViewedProvider.ParseCookieValue(cookieValue);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCookieValue_ShouldParseValidCookie()
        {
            // Arrange
            var expected = new List<int> { 1, 2, 3, 4 };
            var cookieValue = "1,2,3,4";

            // Act
            var actual = CookiePagesViewedProvider.ParseCookieValue(cookieValue);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCookieValue_ShouldRemoveInvalidValuesFromCookie()
        {
            // Arrange
            var expected = new List<int> { 1, 2, 3, 4 };
            var cookieValue = "1,invalid,2,3,####,4,@!@!";

            // Act
            var actual = CookiePagesViewedProvider.ParseCookieValue(cookieValue);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCookieValue_ShouldIgnoreInvalidCookie()
        {
            // Arrange
            var expected = new List<int>();
            var cookieValue = "ThisIsABadCookie";

            // Act
            var actual = CookiePagesViewedProvider.ParseCookieValue(cookieValue);

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
