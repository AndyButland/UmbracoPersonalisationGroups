namespace Umbraco.Web
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.V8.ExtensionMethods;

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
        public static bool ShowToVisitor(this IPublishedElement content, bool showIfNoGroupsDefined = true)
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
        public static int ScoreForVisitor(this IPublishedElement content)
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
            var groups = umbraco.Content(groupIds).ToList();
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
            var groups = umbraco.Content(groupIds).ToList();
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
        private static IList<IPublishedContent> GetPickedGroups(IPublishedElement content)
        {
            var propertyAlias = PersonalisationGroupsConfig.Value.GroupPickerAlias;
            if (content.HasProperty(propertyAlias))
            {
                var rawValue = content.Value(propertyAlias);
                switch (rawValue)
                {
                    case IEnumerable<IPublishedContent> list:
                        return list.ToList();
                    case IPublishedContent group:
                        return new List<IPublishedContent> { group };
                }
            }

            return new List<IPublishedContent>();
        }
     }
}
