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

        public string Name => "Authentication status";

        public string Alias => "authenticationStatus";

        public string Description => "Matches visitor session with their authentication status";

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
                throw new ArgumentException($"Provided definition is not valid JSON: {definition}");
            }

            return (authenticationStatusSetting.IsAuthenticated && _authenticationStatusProvider.IsAuthenticated()) ||
                   (!authenticationStatusSetting.IsAuthenticated && !_authenticationStatusProvider.IsAuthenticated());
        }
    }
}
