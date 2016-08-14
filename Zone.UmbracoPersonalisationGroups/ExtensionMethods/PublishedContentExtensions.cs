namespace Umbraco.Web
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Umbraco.Core.Models;
    using Zone.UmbracoPersonalisationGroups;

    /// <summary>
    /// Provides extension methods to IPublishedContent
    /// </summary>
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// Adds an extension method to IPublishedContent to determine if the content item should be shown to the current site
        /// visitor, based on the personalisation groups associated with it.
        /// </summary>
        /// <param name="content">Instance of IPublishedContent</param>
        /// <param name="showIfNoGroupsDefined">Indicates the response to return if groups cannot be found on the content</param>
        /// <returns>True if content should be shown to visitor</returns>
        public static bool ShowToVisitor(this IPublishedContent content, bool showIfNoGroupsDefined = true)
        {
            var pickedGroups = GetPickedGroups(content);
            if (!pickedGroups.Any())
            {
                // No personalisation groups picked or no property for picker, so we return the provided default
                return showIfNoGroupsDefined;
            }

            // Check each personalisation group assigned for a match with the current site visitor
            foreach (var group in pickedGroups)
            {
                var definition = group.GetPropertyValue<PersonalisationGroupDefinition>(AppConstants.PersonalisationGroupDefinitionPropertyAlias);
                var matchCount = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);

                if ((definition.Match == PersonalisationGroupDefinitionMatch.Any && matchCount > 0) ||
                    (definition.Match == PersonalisationGroupDefinitionMatch.All && matchCount == definition.Details.Count()))
                {
                    // We've matched one of the definitions associated with a selected personalisation group
                    return true;
                }
            }

            // If we've got here, we haven't found a match
            return false;
        }

        /// <summary>
        /// Gets the list of personalisation group content items associated with the current content item
        /// </summary>
        /// <param name="content">Instance of IPublished content</param>
        /// <returns>List of personalisation group content items</returns>
        private static IList<IPublishedContent> GetPickedGroups(IPublishedContent content)
        {
            var propertyAlias = GetGroupPickerAlias();
            if (content.HasProperty(propertyAlias))
            {
                var propertyValue = content.GetProperty(propertyAlias).DataValue.ToString();
                if (!string.IsNullOrEmpty(propertyValue))
                {
                    var pickedGroupIds = propertyValue
                        .Split(',')
                        .Select(x => int.Parse(x));

                    var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                    return umbracoHelper.TypedContent(pickedGroupIds).ToList();
                }
            }

            return new List<IPublishedContent>();
        }

        /// <summary>
        /// Gets the alias used for identifying the picked personalisation groups
        /// </summary>
        /// <returns>Alias string</returns>
        private static string GetGroupPickerAlias()
        {
            // First check if defined in config
            var groupPickerAlias = ConfigurationManager.AppSettings[AppConstants.ConfigKeys.CustomPersonalisationGroupPickerAlias];
            if (string.IsNullOrEmpty(groupPickerAlias))
            {
                // If not, use the convention alias
                groupPickerAlias = AppConstants.DefaultPersonalisationGroupPickerAlias;
            }

            return groupPickerAlias;
        }
    }
}
