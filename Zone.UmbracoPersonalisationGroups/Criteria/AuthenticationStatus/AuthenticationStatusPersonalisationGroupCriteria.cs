namespace Zone.UmbracoPersonalisationGroups.Criteria.AuthenticationStatus
{
    using System;
    using Newtonsoft.Json;
    using Umbraco.Core;

    /// <summary>
    /// Implements a personalisation group criteria based on whether the user is logged on or not
    /// </summary>
    public class AuthenticationStatusPersonalisationGroupCriteria : IPersonalisationGroupCriteria
    {
        private readonly IAuthenticationStatusProvider _authenticationStatusProvider;

        public AuthenticationStatusPersonalisationGroupCriteria()
        {
            _authenticationStatusProvider = new HttpContextAuthenticationStatusProvider();
        }

        public AuthenticationStatusPersonalisationGroupCriteria(IAuthenticationStatusProvider authenticationStatusProvider)
        {
            _authenticationStatusProvider = authenticationStatusProvider;
        }

        public string Name
        {
            get { return "Authentication status"; }
        }

        public string Alias
        {
            get { return "authenticationStatus"; }
        }

        public string Description
        {
            get { return "Matches visitor session with their authentication status"; }
        }

        public bool MatchesVisitor(string definition)
        {
            Mandate.ParameterNotNullOrEmpty(definition, "definition");

            AuthenticationStatusSetting authenticationStatusSetting;
            try
            {
                authenticationStatusSetting = JsonConvert.DeserializeObject<AuthenticationStatusSetting>(definition);
            }
            catch (JsonReaderException)
            {
                throw new ArgumentException(string.Format("Provided definition is not valid JSON: {0}", definition));
            }

            return (authenticationStatusSetting.IsAuthenticated && _authenticationStatusProvider.IsAuthenticated()) ||
                   (!authenticationStatusSetting.IsAuthenticated && !_authenticationStatusProvider.IsAuthenticated());
        }
    }
}
