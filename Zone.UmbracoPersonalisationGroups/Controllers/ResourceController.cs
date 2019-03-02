namespace Zone.UmbracoPersonalisationGroups.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.ExtensionMethods;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;
    using Zone.UmbracoPersonalisationGroups.Helpers;

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
        public ActionResult GetResource(string fileName)
        {
            Mandate.ParameterNotNullOrEmpty(fileName, nameof(fileName));

            // Get this assembly.
            var assembly = typeof(ResourceController).Assembly;
            string resourceName;
            var resourceStream = EmbeddedResourceHelper.GetResource(assembly, fileName, out resourceName);

            if (resourceStream != null)
            {
                return new FileStreamResult(resourceStream, GetMimeType(resourceName));
            }

            return this.HttpNotFound();
        }

        /// <summary>
        /// Gets an embedded resource for a given criteria, that may be from the main assembly or another one
        /// </summary>
        /// <param name="criteriaAlias">Alias of criteria</param>
        /// <param name="fileName">Name of resource</param>
        /// <returns>File stream of resource</returns>
        public ActionResult GetResourceForCriteria(string criteriaAlias, string fileName)
        {
            Mandate.ParameterNotNullOrEmpty(criteriaAlias, nameof(criteriaAlias));
            Mandate.ParameterNotNullOrEmpty(fileName, nameof(fileName));

            var criteria = PersonalisationGroupMatcher.GetAvailableCriteria()
                .SingleOrDefault(x => x.Alias.InvariantEquals(criteriaAlias));

            if (criteria != null)
            {
                string resourceName;
                var resourceStream = EmbeddedResourceHelper.GetResource(criteria.GetType().Assembly, criteriaAlias + "." + fileName, out resourceName);

                if (resourceStream != null)
                {
                    return new FileStreamResult(resourceStream, GetMimeType(resourceName));
                }
            }

            return HttpNotFound();
        }

        /// <summary>
        /// Helper to set the MIME type for a given file name
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <returns>MIME type for file</returns>
        private static string GetMimeType(string fileName)
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
