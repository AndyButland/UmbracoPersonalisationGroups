namespace Zone.UmbracoPersonalisationGroups.Common.Providers.Ip
{
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

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
            var testIp = PersonalisationGroupsConfig.Value.TestFixedIp;
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
