namespace Zone.UmbracoPersonalisationGroups.V8.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Umbraco.Core.Services;
    using Zone.UmbracoPersonalisationGroups.Common.Controllers;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Controller making available member details to HTTP requests
    /// </summary>
    public class MemberController : BaseJsonResultController
    {
        private readonly IMemberTypeService _memberTypeService;
        private readonly IMemberService _memberService;

        public MemberController(IMemberTypeService memberTypeService, IMemberService memberService)
        {
            Mandate.ParameterNotNull(memberTypeService, nameof(memberTypeService));
            Mandate.ParameterNotNull(memberService, nameof(memberService));

            _memberTypeService = memberTypeService;
            _memberService = memberService;
        }

        /// <summary>
        /// Gets a JSON list of the available member types
        /// </summary>
        /// <returns>JSON response of available criteria</returns>
        /// <remarks>Using ContentResult so can serialize with camel case for consistency in client-side code</remarks>
        public ContentResult GetMemberTypes()
        {
            var memberTypes = _memberTypeService.GetAll()
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
            var memberGroups = _memberService.GetAllRoles()
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
            var memberTypes = _memberTypeService.GetAll();
            var fields = memberTypes
                .SelectMany(x => x.PropertyTypes)
                .Select(x => x.Alias)
                .Distinct()
                .OrderBy(x => x);

            return CamelCasedJsonResult(fields);
        }
    }
}
