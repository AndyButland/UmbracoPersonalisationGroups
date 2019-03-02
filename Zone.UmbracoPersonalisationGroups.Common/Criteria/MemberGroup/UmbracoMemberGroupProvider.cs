namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.MemberGroup
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    public class UmbracoMemberGroupProvider : IMemberGroupProvider
    {
        public IEnumerable<string> GetMemberGroups()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                return Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);

            }

            return Enumerable.Empty<string>();
        }
    }
}
