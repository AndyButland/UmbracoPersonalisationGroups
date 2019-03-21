namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria.PagesViewed
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed;

    [TestClass]
    public class UserActivityTrackerTests
    {
        [TestMethod]
        public void AppendPageIdIfNotPreviouslyViewed_ShouldAddPageIdToEmptyCookie()
        {
            // Arrange
            var expected = "1000";
            var cookieValue = string.Empty;
            var pageId = 1000;

            // Act
            var actual = UserActivityTracker.AppendPageIdIfNotPreviouslyViewed(cookieValue, pageId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AppendPageIdIfNotPreviouslyViewed_ShouldAddPageIdToValidCookie()
        {
            // Arrange
            var expected = "1,2,3,4,1000";
            var cookieValue = "1,2,3,4";
            var pageId = 1000;

            // Act
            var actual = UserActivityTracker.AppendPageIdIfNotPreviouslyViewed(cookieValue, pageId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AppendPageIdIfNotPreviouslyViewed_ShouldRemoveInvalidValuesAndAddPageIdToCookie()
        {
            // Arrange
            var expected = "1,2,3,4,1000";
            var cookieValue = "1,invalid,2,3,####,4,@!@!";
            var pageId = 1000;

            // Act
            var actual = UserActivityTracker.AppendPageIdIfNotPreviouslyViewed(cookieValue, pageId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AppendPageIdIfNotPreviouslyViewed_ShouldIgnoreInvalidCookieAndAddPageId()
        {
            // Arrange
            var expected = "1000";
            var cookieValue = "ThisIsABadCookie";
            var pageId = 1000;

            // Act
            var actual = UserActivityTracker.AppendPageIdIfNotPreviouslyViewed(cookieValue, pageId);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
