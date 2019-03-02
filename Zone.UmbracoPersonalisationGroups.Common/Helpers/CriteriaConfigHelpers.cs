namespace Zone.UmbracoPersonalisationGroups.Common.Helpers
{
    using System.Linq;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;

    public static class CriteriaConfigHelpers
    {
        public static bool IsCriteriaInUse(string alias)
        {
            var config = PersonalisationGroupsConfig.Value;
            var includeCriteria = config.IncludeCriteria;
            if (!string.IsNullOrEmpty(includeCriteria))
            {
                return includeCriteria
                    .Split(',')
                    .Contains(alias);
            }

            var excludeCriteria = config.ExcludeCriteria;
            if (!string.IsNullOrEmpty(excludeCriteria))
            {
                return !excludeCriteria
                    .Split(',')
                    .Contains(alias);
            }

            return true;
        }
    }
}