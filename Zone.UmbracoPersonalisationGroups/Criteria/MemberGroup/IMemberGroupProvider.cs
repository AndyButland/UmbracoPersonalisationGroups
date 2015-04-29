namespace Zone.UmbracoPersonalisationGroups.Criteria.MemberGroup
{
    using System.Collections.Generic;

    public interface IMemberGroupProvider
    {
        IEnumerable<string> GetMemberGroups();
    }
}
