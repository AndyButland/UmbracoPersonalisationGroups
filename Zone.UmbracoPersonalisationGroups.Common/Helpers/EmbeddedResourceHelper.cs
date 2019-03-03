namespace Zone.UmbracoPersonalisationGroups.Common.Helpers
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.Controllers;
    using Zone.UmbracoPersonalisationGroups.Common.ExtensionMethods;

    /// <summary>
    /// Provides methods for retrieving embedded resources.
    /// </summary>
    internal static class EmbeddedResourceHelper
    {
        /// <summary>
        /// Returns a value indicating whether the given resource exists.
        /// </summary>
        /// <param name="resource">The resource name.</param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ResourceExists(string resource)
        {
            // Sanitize the resource request.
            if (resource.StartsWith(AppConstants.ResourceRoot))
            {
                resource = resource.TrimStart(AppConstants.ResourceRoot).Replace("/", ".").TrimEnd(AppConstants.ResourceExtension);
            }
            else if (resource.StartsWith(AppConstants.ResourceForCriteriaRoot))
            {
                resource = resource.TrimStart(AppConstants.ResourceForCriteriaRoot).Replace("/", ".").TrimEnd(AppConstants.ResourceExtension);
            }
            else if (resource.EndsWith(AppConstants.ResourceExtension))
            {
                resource = resource.TrimEnd(AppConstants.ResourceExtension);
            }

            // Check this assembly first.
            var assembly = typeof(ResourceController).Assembly;

            // Find the resource name; not case sensitive.
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                // We need to loop through the loaded criteria and check each one.
                var localAssembly = assembly;
                var criteria = PersonalisationGroupMatcher.GetAvailableCriteria()
                    .Where(a => a.GetType().Assembly != localAssembly);

                foreach (var criterion in criteria)
                {
                    assembly = criterion.GetType().Assembly;
                    resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

                    if (!string.IsNullOrWhiteSpace(resourceName))
                    {
                        return true;
                    }
                }
            }

            return !string.IsNullOrWhiteSpace(resourceName);
        }

        /// <summary>
        /// Gets a stream containing the content of the embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly containing the resource.</param>
        /// <param name="resource">The path to the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public static Stream GetResource(Assembly assembly, string resource, out string resourceName)
        {
            // Sanitize the resource request.
            resource = SanitizeCriteriaResourceName(resource);

            // Find the resource name; not case sensitive.
            resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));
            return resourceName != null 
                ? assembly.GetManifestResourceStream(resourceName) 
                : null;
        }

        /// <summary>
        /// Gets a sanitized name for an embedded criteria resource.
        /// </summary>
        /// <param name="resource">The path to the resource.</param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public static string SanitizeCriteriaResourceName(string resource)
        {
            // Sanitize the resource request.
            if (resource.StartsWith(AppConstants.ResourceRoot))
            {
                resource = AppConstants.ResourceRootNameSpace + resource.TrimStart(AppConstants.ResourceRoot).Replace("/", ".").TrimEnd(AppConstants.ResourceExtension);
            }
            else if (resource.StartsWith(AppConstants.ResourceForCriteriaRoot))
            {
                resource = AppConstants.ResourceForCriteriaRootNameSpace + resource.TrimStart(AppConstants.ResourceForCriteriaRoot).Replace("/", ".").TrimEnd(AppConstants.ResourceExtension);
            }
            else if (resource.EndsWith(AppConstants.ResourceExtension))
            {
                resource = resource.TrimEnd(AppConstants.ResourceExtension);
            }

            return resource;
        }
    }
}
