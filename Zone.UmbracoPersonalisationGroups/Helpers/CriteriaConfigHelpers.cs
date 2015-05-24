namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System.Configuration;
    using System.Linq;

    public static class CriteriaConfigHelpers
    {
        public static bool IsCriteriaInUse(string alias)
        {
            var includeCriteria = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.IncludeCriteria];
            if (!string.IsNullOrEmpty(includeCriteria))
            {
                return includeCriteria
                    .Split(',')
                    .Contains(alias);
            }

            var excludeCriteria = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.ExcludeCriteria];
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