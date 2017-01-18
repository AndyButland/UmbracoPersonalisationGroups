namespace Zone.UmbracoPersonalisationGroups.Providers.Ip
{
    using System;
    using System.Collections.Generic;
    using System.Web;

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

        private IEnumerable<string> GetServerVariablesForPublicIpDetection()
        {
            return new string[]
            {
                "HTTP_X_FORWARDED_FOR", "REMOTE_ADDR", "HTTP_CLIENT_IP",
                "HTTP_X_FORWARDED", "HTTP_X_CLUSTER_CLIENT_IP",
                "HTTP_FORWARDED_FOR", "HTTP_FORWARDED"
            };
        }

        private bool IsServerVariableForClientIpAvailableAndNotSetToAPrivateIp(HttpContext httpContext, string variable)
        {
            return !string.IsNullOrEmpty(httpContext.Request.ServerVariables[variable]) &&
                   !httpContext.Request.ServerVariables[variable].StartsWith("192.");
        }

        private string RemovePortNumberFromIp(string ip)
        {
            if (ip.Contains(":"))
            {
                ip = ip.Substring(0, ip.IndexOf(":", StringComparison.Ordinal));
            }

            return ip;
        }
    }
}
