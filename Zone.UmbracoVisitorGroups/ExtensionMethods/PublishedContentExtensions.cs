namespace Umbraco.Web
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Umbraco.Core.Models;
    using Zone.UmbracoVisitorGroups;

    /// <summary>
    /// Provides extension methods to IPublishedContent
    /// </summary>
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// Adds an extension method to IPublishedContent to determine if the content item should be shown to the current site
        /// visitor, based on the visitor groups associated with it.
        /// </summary>
        /// <param name="content">Instance of IPublishedContent</param>
        /// <returns>True if content should be shown to visitor</returns>
        public static bool ShowToVisitor(this IPublishedContent content)
        {
            var pickedVisitorGroups = GetPickedVisitorGroups(content);
            if (!pickedVisitorGroups.Any())
            {
                // No visitor groups picked or no property for picker, so we assume available to all
                return true;
            }

            // Check each visitor group assigned for a match with the current site visitor
            foreach (var visitorGroup in pickedVisitorGroups)
            {
                var definition = visitorGroup.GetPropertyValue<VisitorGroupDefinition>(Constants.VisitorGroupDefinitionPropertyAlias);
                var matchCount = CountMatchingDefinitionDetails(definition);

                if (definition.Match == VisitorGroupDefinitionMatch.Any && matchCount > 0 ||
                    definition.Match == VisitorGroupDefinitionMatch.All && matchCount == definition.Details.Count())
                {
                    // We've matched one of the definitions associated with a selected visitor group
                    return true;
                }
            }

            // If we've got here, we haven't found a match
            return false;
        }

        /// <summary>
        /// Gets the list of visitor group content items associated with the current content item
        /// </summary>
        /// <param name="content">Instance of IPublished content</param>
        /// <returns>List of visitor group content items</returns>
        private static IList<IPublishedContent> GetPickedVisitorGroups(IPublishedContent content)
        {
            var propertyAlias = GetVisitorGroupPickerAlias();
            if (content.HasProperty(propertyAlias))
            {
                var propertyValue = content.GetProperty(propertyAlias).DataValue.ToString();
                if (!string.IsNullOrEmpty(propertyValue))
                {
                    var pickedVisitorGroupIds = propertyValue
                        .Split(',')
                        .Select(x => int.Parse(x));

                    var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                    return umbracoHelper.TypedContent(pickedVisitorGroupIds).ToList();
                }
            }

            return new List<IPublishedContent>();
        }

        /// <summary>
        /// Gets the alias used for identifying the picked visitor groups
        /// </summary>
        /// <returns>Alias string</returns>
        private static string GetVisitorGroupPickerAlias()
        {
            // First check if defined in config
            var visitorGroupPickerAlias = ConfigurationManager.AppSettings["visitorGroups.visitorGroupPickerAlias"];
            if (string.IsNullOrEmpty(visitorGroupPickerAlias))
            {
                // If not, use the convention alias
                visitorGroupPickerAlias = Constants.DefaultVisitorGroupPickerAlias;
            }

            return visitorGroupPickerAlias;
        }

        /// <summary>
        /// Gets a count of the number of the definition details for a given visitor group definition that matches
        /// the current site visitor
        /// </summary>
        /// <param name="definition">Visitor group definition</param>
        /// <returns>Number of definition details that match</returns>
        private static int CountMatchingDefinitionDetails(VisitorGroupDefinition definition)
        {
            var matchCount = 0;
            foreach (var detail in definition.Details)
            {
                var isMatch = VisitorGroupMatcher.IsMatch(detail);
                if (isMatch)
                {
                    matchCount++;
                }

                // We can short-cut here if matching any and found one match, or matching all and found one mismatch
                if (isMatch && definition.Match == VisitorGroupDefinitionMatch.Any ||
                    !isMatch && definition.Match == VisitorGroupDefinitionMatch.All)
                {
                    break;
                }
            }

            return matchCount;
        }
    }
}
