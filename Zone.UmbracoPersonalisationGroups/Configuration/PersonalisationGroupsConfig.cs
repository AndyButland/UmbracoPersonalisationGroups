namespace Zone.UmbracoPersonalisationGroups.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Configuration for personalisation groups
    /// </summary>
    public class PersonalisationGroupsConfig
    {
        private static PersonalisationGroupsConfig _value;

        internal static PersonalisationGroupsConfig Value => _value ?? new PersonalisationGroupsConfig();

        public static void Setup(PersonalisationGroupsConfig config)
        {
            _value = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalisationGroupsConfig"/> class.
        /// </summary>
        private PersonalisationGroupsConfig()
        {
            DisablePackage = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.DisablePackage] == "true";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalisationGroupsConfig"/> class.
        /// </summary>
        public PersonalisationGroupsConfig(bool disablePackage = false)
        {
            DisablePackage = disablePackage;
        }

        /// <summary>
        /// Disables matching of content
        /// </summary>
        public bool DisablePackage { get; }
    }
}
