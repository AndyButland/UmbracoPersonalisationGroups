namespace Zone.UmbracoPersonalisationGroups.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Umbraco.Core;

    using Zone.UmbracoPersonalisationGroups.Criteria;
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
            Mandate.ParameterNotNullOrEmpty(fileName, "fileName");

            // Get this assembly.
            Assembly assembly = typeof(ResourceController).Assembly;
            string resourceName;
            Stream resourceStream = EmbeddedResourceHelper.GetResource(assembly, fileName, out resourceName);

            if (resourceStream != null)
            {
                return new FileStreamResult(resourceStream, this.GetMimeType(resourceName));
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
            Mandate.ParameterNotNullOrEmpty(criteriaAlias, "criteriaAlias");
            Mandate.ParameterNotNullOrEmpty(fileName, "fileName");

            IPersonalisationGroupCriteria criteria = 
                PersonalisationGroupMatcher.GetAvailableCriteria()
                                           .SingleOrDefault(x => x.Alias.InvariantEquals(criteriaAlias));

            if (criteria != null)
            {
                string resourceName;
                Stream resourceStream = EmbeddedResourceHelper.GetResource(criteria.GetType().Assembly, criteriaAlias + "." + fileName, out resourceName);

                if (resourceStream != null)
                {
                    return new FileStreamResult(resourceStream, this.GetMimeType(resourceName));
                }
            }

            return this.HttpNotFound();
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
