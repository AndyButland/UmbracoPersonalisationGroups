namespace Zone.UmbracoPersonalisationGroups.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Umbraco.Core;
    using Zone.UmbracoPersonalisationGroups.Common.Controllers;

    /// <summary>
    /// Controller making available member details to HTTP requests
    /// </summary>
    public class MemberController : BaseJsonResultController
    {
        /// <summary>
        /// Gets a JSON list of the available member types
        /// </summary>
        /// <returns>JSON response of available criteria</returns>
        /// <remarks>Using ContentResult so can serialize with camel case for consistency in client-side code</remarks>
        public ContentResult GetMemberTypes()
        {
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
            var memberTypes = memberTypeService.GetAll()
                .OrderBy(x => x.Alias)
                .Select(x => x.Alias);
            return CamelCasedJsonResult(memberTypes);
        }

        /// <summary>
        /// Gets a JSON list of the available member groups
        /// </summary>
        /// <returns>JSON response of available criteria</returns>
        /// <remarks>Using ContentResult so can serialize with camel case for consistency in client-side code</remarks>
        public ContentResult GetMemberGroups()
        {
            var memberService = ApplicationContext.Current.Services.MemberService;
            var memberGroups = memberService.GetAllRoles()
                .OrderBy(x => x);
            return CamelCasedJsonResult(memberGroups);
       }

        /// <summary>
        /// Gets a JSON list of the available member profile fields
        /// </summary>
        /// <returns>JSON response of available criteria</returns>
        /// <remarks>Using ContentResult so can serialize with camel case for consistency in client-side code</remarks>
        public ContentResult GetMemberProfileFields()
        {
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
            var memberTypes = memberTypeService.GetAll();
            var fields = memberTypes
                .SelectMany(x => x.PropertyTypes)
                .Select(x => x.Alias)
                .Distinct()
                .OrderBy(x => x);

            return CamelCasedJsonResult(fields);
        }
    }
}
