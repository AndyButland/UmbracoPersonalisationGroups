namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria.Continent
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.Continent;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.GeoLocation;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Ip;

    [TestClass]
    public class ContinentPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"match\": \"{0}\", \"codes\": [ \"{1}\", \"{2}\" ] }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithEmptyContinentLists_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);
            var definition = "{ \"match\": \"IsLocatedIn\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentContinentList_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);
            var definition = string.Format(DefinitionFormat, "IsLocatedIn", "AF", "AS");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingContinentList_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);
            var definition = string.Format(DefinitionFormat, "IsLocatedIn", "EU", "AF");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithDifferentContinentListAndNotInCheck_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);
            var definition = string.Format(DefinitionFormat, "IsNotLocatedIn", "AF", "AS");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionWithMatchingContinentListAndNotInCheck_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);
            var definition = string.Format(DefinitionFormat, "IsNotLocatedIn", "EU", "AF");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCannotLocate_ReturnsTrue()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider(canGeolocate: false);
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);
            var definition = "{ \"match\": \"CouldNotBeLocated\", \"codes\": [] }";

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContinentPersonalisationGroupCriteria_MatchesVisitor_WithValidDefinitionForCouldNotBeLocatedWhenCanLocate_ReturnsFalse()
        {
            // Arrange
            var mockIpProvider = MockIpProvider();
            var mockContinentGeoLocationProvider = MockGeoLocationProvider();
            var criteria = new ContinentPersonalisationGroupCriteria(mockIpProvider.Object, mockContinentGeoLocationProvider.Object);
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

            mock.Setup(x => x.GetContinentFromIp(It.IsAny<string>()))
                .Returns(canGeolocate
                    ? new Continent
                        {
                            Code = "EU", Name = "Europe"
                        }
                    : null);

            return mock;
        }

        #endregion
    }
}
