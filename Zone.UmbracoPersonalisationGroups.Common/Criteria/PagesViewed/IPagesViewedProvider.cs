namespace Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed
{
    using System.Collections.Generic;

    public interface IPagesViewedProvider
    {
        IEnumerable<int> GetNodeIdsViewed();
    }
}
