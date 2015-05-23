namespace Zone.UmbracoPersonalisationGroups.Criteria.PagesViewed
{
    using System.Collections.Generic;

    public interface IPagesViewedProvider
    {
        IEnumerable<int> GetNodeIdsViewed();
    }
}
