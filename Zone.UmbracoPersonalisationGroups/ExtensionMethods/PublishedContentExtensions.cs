namespace Umbraco.Web
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Umbraco.Core.Models;
    using Zone.UmbracoPersonalisationGroups;
    using Zone.UmbracoPersonalisationGroups.Helpers;

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
            return ShowToVisitor(pickedGroups, showIfNoGroupsDefined);
        }

        /// <summary>
        /// Adds an extension method to IPublishedContent to score the content item for the current site
        /// visitor, based on the personalisation groups associated with it.
        /// </summary>
        /// <param name="content">Instance of IPublishedContent</param>
        /// <returns>True if content should be shown to visitor</returns>
        public static int ScoreForVisitor(this IPublishedContent content)
        {
            var pickedGroups = GetPickedGroups(content);
            return ScoreForVisitor(pickedGroups);
        }

        /// <summary>
        /// Adds an extension method to UmbracoHelper to determine if the content item should be shown to the current site
        /// visitor, based on the personalisation groups associated with the Ids passed into the method
        /// </summary>
        /// <param name="umbraco">Instance of UmbracoHelper</param>
        /// <param name="groupIds">List of group Ids</param>
        /// <param name="showIfNoGroupsDefined">Indicates the response to return if groups cannot be found on the content</param>
        /// <returns>True if content should be shown to visitor</returns>
        public static bool ShowToVisitor(this UmbracoHelper umbraco, IEnumerable<int> groupIds, bool showIfNoGroupsDefined = true)
        {
            var groups = umbraco.TypedContent(groupIds).ToList();
            return ShowToVisitor(groups, showIfNoGroupsDefined);
        }

        /// <summary>
        /// Adds an extension method to UmbracoHelper to score the content item for the current site
        /// visitor, based on the personalisation groups associated with the Ids passed into the method
        /// </summary>
        /// <param name="umbraco">Instance of UmbracoHelper</param>
        /// <param name="groupIds">List of group Ids</param>
        /// <returns>True if content should be shown to visitor</returns>
        public static int ScoreForVisitor(this UmbracoHelper umbraco, IEnumerable<int> groupIds)
        {
            var groups = umbraco.TypedContent(groupIds).ToList();
            return ScoreForVisitor(groups);
        }

        /// <summary>
        /// Determines if the content item should be shown to the current site visitor, based on the personalisation groups associated with it.
        /// </summary>
        /// <param name="pickedGroups">List of IPublishedContent items that are the groups you want to check against.</param>
        /// <param name="showIfNoGroupsDefined">Indicates the response to return if groups cannot be found on the content</param>
        /// <returns>True if content should be shown to visitor</returns>
        private static bool ShowToVisitor(IList<IPublishedContent> pickedGroups, bool showIfNoGroupsDefined = true)
        {
            if (!pickedGroups.Any())
            {
                // No personalisation groups picked or no property for picker, so we return the provided default
                return showIfNoGroupsDefined;
            }

            return UmbracoExtensionsHelper.MatchGroups(pickedGroups);
        }
        
        /// <summary>
        /// Scores the content item for the current site visitor, based on the personalisation groups associated with it.
        /// </summary>
        /// <param name="pickedGroups">List of IPublishedContent items that are the groups you want to check against.</param>
        /// <returns>True if content should be shown to visitor</returns>
        private static int ScoreForVisitor(IList<IPublishedContent> pickedGroups)
        {
            if (!pickedGroups.Any())
            {
                // No personalisation groups picked or no property for picker, so we score zero
                return 0;
            }

            return UmbracoExtensionsHelper.ScoreGroups(pickedGroups);
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
                // If on v7.6 (or if Umbraco Core Property Converters package installed on an earlier version)
                // we can retrieve typed property values.
                var propertyValueAsEnumerable = content.GetPropertyValue<IEnumerable<IPublishedContent>>(propertyAlias);
                if (propertyValueAsEnumerable != null)
                {
                    return propertyValueAsEnumerable.ToList();
                }

                // Fall-back check for CSV of integers (format used for MNTP before v7.6)
                var propertyValueAsCsv = content.GetProperty(propertyAlias).DataValue.ToString();
                if (!string.IsNullOrEmpty(propertyValueAsCsv))
                {
                    var pickedGroupIds = propertyValueAsCsv
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
