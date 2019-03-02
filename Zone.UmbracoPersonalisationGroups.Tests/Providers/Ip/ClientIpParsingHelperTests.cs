namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Providers.Ip
{
    using System.Collections.Specialized;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Ip;

    [TestClass]
    public class ClientIpParsingHelperTests
    {
        [TestMethod]
        public void ParseClientIp_WithNoServerVariables_ReturnsEmptyString()
        {
            // Arrange
            var requestServerVariables = new NameValueCollection();

            // Act
            var result = ClientIpParsingHelper.ParseClientIp(requestServerVariables);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ParseClientIp_WithNoValuesInServerVariables_ReturnsEmptyString()
        {
            // Arrange
            var requestServerVariables = new NameValueCollection { { "HTTP_FORWARDED_FOR", string.Empty } };

            // Act
            var result = ClientIpParsingHelper.ParseClientIp(requestServerVariables);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ParseClientIp_WithSingleIpInServerVariable_ReturnsIp()
        {
            // Arrange
            var requestServerVariables = new NameValueCollection { { "HTTP_FORWARDED_FOR", "12.34.56.78" } };

            // Act
            var result = ClientIpParsingHelper.ParseClientIp(requestServerVariables);

            // Assert
            Assert.AreEqual("12.34.56.78", result);
        }

        [TestMethod]
        public void ParseClientIp_WithSingleInvalidIpInServerVariable_ReturnsEmptyString()
        {
            // Arrange
            var requestServerVariables = new NameValueCollection { { "HTTP_FORWARDED_FOR", "xxxx" } };

            // Act
            var result = ClientIpParsingHelper.ParseClientIp(requestServerVariables);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ParseClientIp_WithMultipleIpsInServerVariable_ReturnsIp()
        {
            // Arrange
            var requestServerVariables = new NameValueCollection { { "HTTP_FORWARDED_FOR", "12.34.56.78, 12.34.56.79" } };

            // Act
            var result = ClientIpParsingHelper.ParseClientIp(requestServerVariables);

            // Assert
            Assert.AreEqual("12.34.56.78", result);
        }

        [TestMethod]
        public void ParseClientIp_WithSingleIpInServerVariableAndALocalIp_ReturnsIp()
        {
            // Arrange
            var requestServerVariables = new NameValueCollection { { "HTTP_FORWARDED_FOR", "12.34.56.78" }, { "HTTP_X_FORWARDED_FOR", "192.168.1.1" } };

            // Act
            var result = ClientIpParsingHelper.ParseClientIp(requestServerVariables);

            // Assert
            Assert.AreEqual("12.34.56.78", result);
        }

        [TestMethod]
        public void ParseClientIp_WithSingleIpInServerVariableAndALocalIpInReverseOrder_ReturnsIp()
        {
            // Arrange
            var requestServerVariables = new NameValueCollection { { "HTTP_FORWARDED_FOR", "192.168.1.1" }, { "HTTP_X_FORWARDED_FOR", "12.34.56.78" } };

            // Act
            var result = ClientIpParsingHelper.ParseClientIp(requestServerVariables);

            // Assert
            Assert.AreEqual("12.34.56.78", result);
        }
    }
}
