namespace Zone.UmbracoPersonalisationGroups.Providers.Ip
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;
    using Umbraco.Core.Configuration;
    using Zone.UmbracoPersonalisationGroups.Configuration;

    public class HttpContextIpProvider : IIpProvider
    {
        public string GetIp()
        {
            var ip = GetIpFromHttpContext();
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }

            return ip;
        }

        private static string GetIpFromHttpContext()
        {
            // Return a test Ip if we've configured one
            var testIp = UmbracoConfig.For.PersonalisationGroups().TestFixedIp;
            if (!string.IsNullOrEmpty(testIp))
            {
                return testIp;
            }

            // Otherwise retrieve from the HTTP context
            var requestServerVariables = HttpContext.Current.Request.ServerVariables;
            return ClientIpParsingHelper.ParseClientIp(requestServerVariables);
        }
    }
}
