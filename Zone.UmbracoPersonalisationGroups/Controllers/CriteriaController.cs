namespace Zone.UmbracoPersonalisationGroups.Controllers
{
    using System.Web.Mvc;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.Controllers;

    /// <summary>
    /// Controller making available criteria details to HTTP requests
    /// </summary>
    public class CriteriaController : BaseJsonResultController
    {
        /// <summary>
        /// Gets a JSON list of the available criteria
        /// </summary>
        /// <returns>JSON response of available criteria</returns>
        /// <remarks>Using ContentResult so can serialize with camel case for consistency in client-side code</remarks>
        public ContentResult Index()
        {
            var criteria = PersonalisationGroupMatcher.GetAvailableCriteria();
            return CamelCasedJsonResult(criteria);
        }
    }
}
