namespace Zone.UmbracoPersonalisationGroups.Providers.Ip
{
    using System;
    using System.Collections.Generic;
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

        private string GetIpFromHttpContext()
        {
            // Return a test Ip if we've configured one
            var testIp = UmbracoConfig.For.PersonalisationGroups().TestFixedIp;
            if (!string.IsNullOrEmpty(testIp))
            {
                return testIp;
            }

            // Otherwise retrieve from the HTTP context
            var httpContext = HttpContext.Current;
            var variables = GetServerVariablesForPublicIpDetection();

            foreach (var variable in variables)
            {
                if (IsServerVariableForClientIpAvailableAndNotSetToAPrivateIp(httpContext, variable))
                {
                    return RemovePortNumberFromIp(httpContext.Request.ServerVariables[variable]);
                }
            }

            return string.Empty;
        }

        private static IEnumerable<string> GetServerVariablesForPublicIpDetection()
        {
            return new[]
            {
                "CF-Connecting-IP", "HTTP_X_FORWARDED_FOR", "REMOTE_ADDR",
                "HTTP_CLIENT_IP", "HTTP_X_FORWARDED", "HTTP_X_CLUSTER_CLIENT_IP",
                "HTTP_FORWARDED_FOR", "HTTP_FORWARDED"
            };
        }

        private static bool IsServerVariableForClientIpAvailableAndNotSetToAPrivateIp(HttpContext httpContext, string variable)
        {
            return !string.IsNullOrEmpty(httpContext.Request.ServerVariables[variable]) &&
                   !httpContext.Request.ServerVariables[variable].StartsWith("192.");
        }

        private static string RemovePortNumberFromIp(string ip)
        {
            if (ip.Contains(":"))
            {
                ip = ip.Substring(0, ip.IndexOf(":", StringComparison.Ordinal));
            }

            return ip;
        }
    }
}
