namespace Zone.UmbracoPersonsalisationGroups.Tests.Criteria.Country
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Criteria.Country;
    using Zone.UmbracoPersonalisationGroups.Providers.GeoLocation;
    using Zone.UmbracoPersonalisationGroups.Providers.Ip;

    [TestClass]
    public class CountryPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"match\": \"{0}\", \"codes\": [ \"{1}\", \"{2}\" ] }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyCountryLists_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var definition = "{ \"match\": \"IsLocatedIn\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentCountryList_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var definition = string.Format(DefinitionFormat, "IsLocatedIn", "ES", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingCountryList_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var definition = string.Format(DefinitionFormat, "IsLocatedIn", "GB", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentCountryListAndNotInCheck_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var definition = string.Format(DefinitionFormat, "IsNotLocatedIn", "ES", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingCountryListAndNotInCheck_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var definition = string.Format(DefinitionFormat, "IsNotLocatedIn", "GB", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCannotLocate_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider(canGeolocate: false);
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var definition = "{ \"match\": \"CouldNotBeLocated\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCanLocate_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new CountryPersonalisationGroupCriteria(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var definition = "{ \"match\": \"CouldNotBeLocated\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        #region Mocks

        private static Mock<IIpProvider> MockIpProvider()
        {
            var mock = new Mock<IIpProvider>();

            mock.Setup(x => x.GetIp()).Returns("1.2.3.4");

            return mock;
        }

        private static Mock<IGeoLocationProvider> MockGeoLocationProvider(bool canGeolocate = true)
        {
            var mock = new Mock<IGeoLocationProvider>();

            mock.Setup(x => x.GetCountryFromIp(It.IsAny<string>()))
                .Returns(canGeolocate
                    ? new Country
                        {
                            Code = "GB", Name = "United Kingdom"
                        }
                    : null);

            return mock;
        }

        #endregion
    }
}
