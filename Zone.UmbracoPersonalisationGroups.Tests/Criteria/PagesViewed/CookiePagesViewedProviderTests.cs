using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.PagesViewed
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed;

    [TestClass]
    public class CookiePagesViewedProviderTests
    {
        [TestMethod]
        public void ParseCookieValue_ShouldParseEmptyValue()
        {
            var expected = new List<int>();

            var cookieValue = "";
            var actual = CookiePagesViewedProvider.ParseCookieValue(cookieValue);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCookieValue_ShouldParseValidCookie()
        {
            var expected = new List<int> {1, 2, 3, 4};

            var cookieValue = "1,2,3,4";
            var actual = CookiePagesViewedProvider.ParseCookieValue(cookieValue);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCookieValue_ShouldRemoveInvalidValuesFromCookie()
        {
            var expected = new List<int> { 1, 2, 3, 4 };

            var cookieValue = "1,invalid,2,3,####,4,@!@!";
            var actual = CookiePagesViewedProvider.ParseCookieValue(cookieValue);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCookieValue_ShouldIgnoreInvalidCookie()
        {
            var expected = new List<int>();

            var cookieValue = "ThisIsABadCookie";
            var actual = CookiePagesViewedProvider.ParseCookieValue(cookieValue);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
