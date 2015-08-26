namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Umbraco.Core;

    using Zone.UmbracoPersonalisationGroups.Controllers;

    using Constants = Zone.UmbracoPersonalisationGroups.AppConstants;

    /// <summary>
    /// Provides methods for retrieving embedded resources.
    /// </summary>
    internal class EmbeddedResourceHelper
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
            string root = Constants.ResourceRoot;
            string criteria = Constants.ResourceForCriteriaRoot;
            string extension = Constants.ResourceExtension;

            if (resource.StartsWith(root))
            {
                resource = resource.TrimStart(root).Replace("/", ".").TrimEnd(extension);
            }
            else if (resource.StartsWith(criteria))
            {
                resource = resource.TrimStart(criteria).Replace("/", ".").TrimEnd(extension);
            }
            else if (resource.EndsWith(extension))
            {
                resource = resource.TrimEnd(extension);
            }

            // Get this assembly.
            Assembly assembly = typeof(ResourceController).Assembly;

            // Find the resource name; not case sensitive.
            string resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

            return !string.IsNullOrEmpty(resourceName);
        }

        /// <summary>
        /// Gets a stream containing the content of the embedded resource.
        /// </summary>
        /// <param name="resource">The path to the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public static Stream GetResource(string resource, out string resourceName)
        {
            // Sanitize the resource request.
            string root = Constants.ResourceRoot;
            string criteria = Constants.ResourceForCriteriaRoot;
            string extension = Constants.ResourceExtension;

            if (resource.StartsWith(root))
            {
                resource = Constants.ResourceRootNameSpace + resource.TrimStart(root).Replace("/", ".").TrimEnd(extension);
            }
            else if (resource.StartsWith(criteria))
            {
                resource = Constants.ResourceForCriteriaRootNameSpace + resource.TrimStart(criteria).Replace("/", ".").TrimEnd(extension);
            }
            else if (resource.EndsWith(extension))
            {
                resource = resource.TrimEnd(extension);
            }

            // Get this assembly.
            Assembly assembly = typeof(ResourceController).Assembly;

            // Find the resource name; not case sensitive.
            resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

            if (resourceName != null)
            {
                return assembly.GetManifestResourceStream(resourceName);
            }

            return null;
        }
    }
}
