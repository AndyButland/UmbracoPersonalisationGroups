namespace Zone.UmbracoPersonalisationGroups.Configuration
{
    using System.Threading;
    using Umbraco.Core.Configuration;

    /// <summary>
    /// Provides extension methods for the <see cref="UmbracoConfig"/> class.
    /// </summary>
    public static class UmbracoConfigExtensions
    {
        private static PersonalisationGroupsConfig _config;

        /// <summary>
        /// Gets configuration for personalisation groups.
        /// </summary>
        /// <param name="umbracoConfig">The umbraco configuration.</param>
        /// <returns>The personalisation groups configuration.</returns>
        /// <remarks> 
        /// Getting the personalisation groups configuration freezes its state, and 
        /// any attempt at modifying the configuration will be ignored. 
        /// </remarks>
        public static PersonalisationGroupsConfig PersonalisationGroups(this UmbracoConfig umbracoConfig)
        {
            LazyInitializer.EnsureInitialized(ref _config, () => PersonalisationGroupsConfig.Value);
            return _config;
        }

        // internal for tests
        internal static void ResetConfig()
        {
            _config = null;
        }
    }
}
