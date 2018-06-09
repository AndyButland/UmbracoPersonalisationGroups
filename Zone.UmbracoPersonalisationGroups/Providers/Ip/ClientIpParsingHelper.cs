namespace Zone.UmbracoPersonalisationGroups.Providers.Ip
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public static class ClientIpParsingHelper
    {
        private static readonly IEnumerable<string> ServerVariableNames = new[]
            {
                "CF-Connecting-IP", "HTTP_X_FORWARDED_FOR", "REMOTE_ADDR",
                "HTTP_CLIENT_IP", "HTTP_X_FORWARDED", "HTTP_X_CLUSTER_CLIENT_IP",
                "HTTP_FORWARDED_FOR", "HTTP_FORWARDED"
            };

        public static string ParseClientIp(NameValueCollection requestServerVariables)
        {
            foreach (var variable in ServerVariableNames)
            {
                if (TryParseIpFromServerVariable(requestServerVariables, variable, out string ip))
                {
                    return ip;
                }
            }

            return string.Empty;
        }

        private static bool TryParseIpFromServerVariable(NameValueCollection serverVariables, string variable, out string ip)
        {
            var value = serverVariables[variable];
            if (string.IsNullOrWhiteSpace(value))
            {
                ip = string.Empty;
                return false;
            }

            // We don't want local ips
            if (value.StartsWith("192."))
            {
                ip = string.Empty;
                return false;
            }

            // We might not have a single IP here, as it's possible if the request has passed through multiple proxies, there will be 
            // additional ones in the header
            // If so, the original requesting IP is the first one in a comma+space delimited list
            value = value.Split(new[] { ", " }, StringSplitOptions.None).First();
            ip = RemovePortNumberFromIp(value);
            return true;
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
