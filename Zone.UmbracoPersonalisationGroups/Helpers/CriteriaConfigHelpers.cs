namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System.Linq;
    using Umbraco.Core.Configuration;
    using Zone.UmbracoPersonalisationGroups.Configuration;

    public static class CriteriaConfigHelpers
    {
        public static bool IsCriteriaInUse(string alias)
        {
            var config = UmbracoConfig.For.PersonalisationGroups();
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