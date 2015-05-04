namespace Zone.UmbracoPersonalisationGroups.Controllers
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Umbraco.Core;

    /// <summary>
    /// Controller providing access to embedded client-side angular resource
    /// </summary>
    public class ResourceController : Controller
    {
        /// <summary>
        /// Gets an embedded resource from the main assembly
        /// </summary>
        /// <param name="fileName">Name of resource</param>
        /// <returns>File stream of resource</returns>
        public FileStreamResult GetResource(string fileName)
        {
            Mandate.ParameterNotNullOrEmpty(fileName, "fileName");

            var resourceName =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceNames()
                    .ToList()
                    .FirstOrDefault(f => f.EndsWith(fileName));

            var assembly = typeof(ResourceController).Assembly;

            return new FileStreamResult(assembly.GetManifestResourceStream(resourceName), GetMimeType(fileName));
        }

        /// <summary>
        /// Gets an embedded resource for a given criteria, that may be from the main assembly or another one
        /// </summary>
        /// <param name="criteriaAlias">Alias of criteria</param>
        /// <param name="fileName">Name of resource</param>
        /// <returns>File stream of resource</returns>
        public FileStreamResult GetResourceForCriteria(string criteriaAlias, string fileName)
        {
            Mandate.ParameterNotNullOrEmpty(criteriaAlias, "criteriaAlias");
            Mandate.ParameterNotNullOrEmpty(fileName, "fileName");

            var criteria = PersonalisationGroupMatcher.GetAvailableCriteria()
                .SingleOrDefault(x => string.Equals(x.Alias, criteriaAlias, StringComparison.InvariantCultureIgnoreCase));
            if (criteria != null)
            {
                var resourceName =
                    criteria.GetType().Assembly
                        .GetManifestResourceNames()
                        .ToList()
                        .FirstOrDefault(f => f.EndsWith(criteriaAlias + "." + fileName, StringComparison.InvariantCultureIgnoreCase));

                var assembly = criteria.GetType().Assembly;

                return new FileStreamResult(assembly.GetManifestResourceStream(resourceName), GetMimeType(fileName));
            }

            return null;
        }

        /// <summary>
        /// Helper to set the MIME type for a given file name
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <returns>MIME type for file</returns>
        private string GetMimeType(string fileName)
        {
            if (fileName.EndsWith(".js"))
            {
                return "text/javascript";
            }

            if (fileName.EndsWith(".html"))
            {
                return "text/html";
            }

            if (fileName.EndsWith(".css"))
            {
                return "text/stylesheet";
            }

            return "text/plain";
        }
    }
}
