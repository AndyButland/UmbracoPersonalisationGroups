using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed;

namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.PagesViewed
{
    [TestClass]
    public class RegisterApplicationEventsTests
    {
        [TestMethod]
        public void AppendPageIdIfNotPreviouslyViewed_ShouldAddPageIdToEmptyCookie()
        {
            var expected = "1000";

            // Arrange
            var cookieValue = "";
            var pageId = 1000;
            var actual = RegisterApplicationEvents.AppendPageIdIfNotPreviouslyViewed(cookieValue, pageId);

            // Act
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AppendPageIdIfNotPreviouslyViewed_ShouldAddPageIdToValidCookie()
        {
            var expected = "1,2,3,4,1000";

            // Arrange
            var cookieValue = "1,2,3,4";
            var pageId = 1000;
            var actual = RegisterApplicationEvents.AppendPageIdIfNotPreviouslyViewed(cookieValue, pageId);

            // Act
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AppendPageIdIfNotPreviouslyViewed_ShouldRemoveInvalidValuesAndAddPageIdToCookie()
        {
            var expected = "1,2,3,4,1000";

            // Arrange
            var cookieValue = "1,invalid,2,3,####,4,@!@!";
            var pageId = 1000;
            var actual = RegisterApplicationEvents.AppendPageIdIfNotPreviouslyViewed(cookieValue, pageId);

            // Act
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AppendPageIdIfNotPreviouslyViewed_ShouldIgnoreInvalidCookieAndAddPageId()
        {
            var expected = "1000";

            // Arrange
            var cookieValue = "ThisIsABadCookie";
            var pageId = 1000;
            var actual = RegisterApplicationEvents.AppendPageIdIfNotPreviouslyViewed(cookieValue, pageId);

            // Act
            Assert.AreEqual(expected, actual);
        }
    }
}
