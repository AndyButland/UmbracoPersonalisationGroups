namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria.Country
{
    using System;
    using System.Collections.Specialized;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.Country;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Ip;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.RequestHeaders;

    [TestClass]
    public class CountryPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"match\": \"{0}\", \"codes\": [ \"{1}\", \"{2}\" ] }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithEmptyCountryLists_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = "{ \"match\": \"IsLocatedIn\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithDifferentCountryList_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = string.Format(DefinitionFormat, "IsLocatedIn", "ES", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithMatchingCountryList_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = string.Format(DefinitionFormat, "IsLocatedIn", "GB", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithDifferentCountryListAndNotInCheck_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = string.Format(DefinitionFormat, "IsNotLocatedIn", "ES", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithMatchingCountryListAndNotInCheck_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = string.Format(DefinitionFormat, "IsNotLocatedIn", "GB", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCannotLocate_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider(canGeolocate: false);
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = "{ \"match\": \"CouldNotBeLocated\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingMockedMaxMindDatabaseCountryCodeProvider_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCanLocate_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockCountryGeoLocationProvider = MockGeoLocationProvider();
            var countryCodeProvider = new MaxMindCountryCodeFromIpProvider(mockIpProvider.Object, mockCountryGeoLocationProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = "{ \"match\": \"CouldNotBeLocated\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider();
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider();
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithEmptyCountryLists_ReturnsFalse()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider();
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = "{ \"match\": \"IsLocatedIn\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithDifferentCountryList_ReturnsFalse()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider();
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = string.Format(DefinitionFormat, "IsLocatedIn", "ES", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithMatchingCountryList_ReturnsTrue()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider();
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = string.Format(DefinitionFormat, "IsLocatedIn", "GB", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithDifferentCountryListAndNotInCheck_ReturnsTrue()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider();
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = string.Format(DefinitionFormat, "IsNotLocatedIn", "ES", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithValidDefinitionWithMatchingCountryListAndNotInCheck_ReturnsFalse()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider();
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = string.Format(DefinitionFormat, "IsNotLocatedIn", "GB", "IT");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCannotLocateAsNoHeader_ReturnsTrue()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider(withHeader: false);
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = "{ \"match\": \"CouldNotBeLocated\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCannotLocateAsEmptyHeader_ReturnsTrue()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider(withValue: false);
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
            var definition = "{ \"match\": \"CouldNotBeLocated\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CountryPersonalisationGroupCriteria_UsingCloudFlareCdnHeaderCountryCodeProvider_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCanLocate_ReturnsFalse()
        {
            // Arrange
            var mockRequestHeadersProvider = MockRequestHeadersProvider();
            var countryCodeProvider = new CdnHeaderCountryCodeProvider(mockRequestHeadersProvider.Object);
            var criteria = new CountryPersonalisationGroupCriteria(countryCodeProvider);
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

        private static Mock<IRequestHeadersProvider> MockRequestHeadersProvider(bool withHeader = true, bool withValue = true)
        {
            var mock = new Mock<IRequestHeadersProvider>();

            var resultHeaders = new NameValueCollection();
            if (withHeader)
            {
                resultHeaders.Add(AppConstants.DefaultCdnCountryCodeHttpHeaderName, withValue ? "GB" : string.Empty);
            }

            mock.Setup(x => x.GetHeaders()).Returns(resultHeaders);

            return mock;
        }

        #endregion
    }
}
