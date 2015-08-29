namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Umbraco.Core;

    using Zone.UmbracoPersonalisationGroups.Controllers;
    using Zone.UmbracoPersonalisationGroups.Criteria;

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
            string resourceRoot = Constants.ResourceRoot;
            string criteriaRoot = Constants.ResourceForCriteriaRoot;
            string extension = Constants.ResourceExtension;

            if (resource.StartsWith(resourceRoot))
            {
                resource = resource.TrimStart(resourceRoot).Replace("/", ".").TrimEnd(extension);
            }
            else if (resource.StartsWith(criteriaRoot))
            {
                resource = resource.TrimStart(criteriaRoot).Replace("/", ".").TrimEnd(extension);
            }
            else if (resource.EndsWith(extension))
            {
                resource = resource.TrimEnd(extension);
            }

            // Check this assembly first.
            Assembly assembly = typeof(ResourceController).Assembly;

            // Find the resource name; not case sensitive.
            string resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                // We need to loop through the loaded criteria and check each one.
                Assembly localAssembly = assembly;
                IEnumerable<IPersonalisationGroupCriteria> criteria =
                    PersonalisationGroupMatcher.GetAvailableCriteria().Where(a => a.GetType().Assembly != localAssembly);

                foreach (IPersonalisationGroupCriteria criterion in criteria)
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
            if (resourceName != null)
            {
                return assembly.GetManifestResourceStream(resourceName);
            }

            return null;
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

            return resource;
        }
    }
}
