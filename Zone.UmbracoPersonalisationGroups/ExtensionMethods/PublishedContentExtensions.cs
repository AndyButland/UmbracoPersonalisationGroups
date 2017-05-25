namespace Umbraco.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
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

            // Check each personalisation group assigned for a match with the current site visitor
            foreach (var group in pickedGroups)
            {
                var definition = group.GetPropertyValue<PersonalisationGroupDefinition>(AppConstants.PersonalisationGroupDefinitionPropertyAlias);
                if (IsStickyMatch(definition, group.Id))
                {
                    return true;
                }

                var matchCount = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);

                // If matching any and matched at least one, or matching all and matched all - we've matched one of the definitions 
                // associated with a selected personalisation group
                if ((definition.Match == PersonalisationGroupDefinitionMatch.Any && matchCount > 0) ||
                    (definition.Match == PersonalisationGroupDefinitionMatch.All && matchCount == definition.Details.Count()))
                {
                    MakeStickyMatch(definition, group.Id);
                    return true;
                }
            }

            // If we've got here, we haven't found a match
            return false;
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

            // Check each personalisation group assigned for a match with the current site visitor
            var score = 0;
            foreach (var group in pickedGroups)
            {
                var definition = group.GetPropertyValue<PersonalisationGroupDefinition>(AppConstants.PersonalisationGroupDefinitionPropertyAlias);
                if (IsStickyMatch(definition, group.Id))
                {
                    score += definition.Score;
                }

                var matchCount = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);

                // If matching any and matched at least one, or matching all and matched all - we've matched one of the definitions 
                // associated with a selected personalisation group
                if ((definition.Match == PersonalisationGroupDefinitionMatch.Any && matchCount > 0) ||
                    (definition.Match == PersonalisationGroupDefinitionMatch.All && matchCount == definition.Details.Count()))
                {
                    MakeStickyMatch(definition, group.Id);
                    score += definition.Score;
                }
            }

            return score;
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
                var list = content.GetPropertyValue<IEnumerable<IPublishedContent>>(propertyAlias);

                if (list == null)
                {
                    var item = content.GetPropertyValue<IPublishedContent>(propertyAlias);

                    if (item == null)
                    {
                        return new List<IPublishedContent>();
                    }

                    return new List<IPublishedContent> { item };
                }

                return list.ToList();

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

        private static bool IsStickyMatch(PersonalisationGroupDefinition definition, int groupNodeId)
        {
            if (definition.Duration == PersonalisationGroupDefinitionDuration.Page)
            {
                return false;
            }

            var httpContext = HttpContext.Current;
            var key = GetCookieKeyForMatchedGroups(definition.Duration);
            var cookie = httpContext.Request.Cookies[key];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                return IsGroupMatched(cookie.Value, groupNodeId);
            }

            return false;
        }

        /// <summary>
        /// Makes a matched group sticky for the visitor via a cookie setting according to group definition
        /// </summary>
        /// <param name="definition">Matched group definition</param>
        /// <param name="groupNodeId">Id of the matched groups node</param>
        private static void MakeStickyMatch(PersonalisationGroupDefinition definition, int groupNodeId)
        {
            if (definition.Duration == PersonalisationGroupDefinitionDuration.Page)
            {
                return;
            }

            var httpContext = HttpContext.Current;
            var key = GetCookieKeyForMatchedGroups(definition.Duration);
            var cookie = httpContext.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Value = AppendGroupNodeId(cookie.Value, groupNodeId);
            }
            else
            {
                cookie = new HttpCookie(key)
                {
                    Value = groupNodeId.ToString(),
                    HttpOnly = true,
                };
            }

            if (definition.Duration == PersonalisationGroupDefinitionDuration.Visitor)
            {
                int cookieExpiryInDays;
                if (!int.TryParse(ConfigurationManager.AppSettings[AppConstants.ConfigKeys.PersistentMatchedGroupsCookieExpiryInDays], out cookieExpiryInDays))
                {
                    cookieExpiryInDays = AppConstants.DefaultMatchedGroupsTrackingCookieExpiryInDays;
                }

                cookie.Expires = DateTime.Now.AddDays(cookieExpiryInDays);
            }

            httpContext.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Retrieves the cookie key to use for the matched groups
        /// </summary>
        /// <param name="duration"></param>
        /// <returns>Cookie key to use</returns>
        private static string GetCookieKeyForMatchedGroups(PersonalisationGroupDefinitionDuration duration)
        {
            string defaultKey, appSettingKey;
            switch (duration)
            {
                case PersonalisationGroupDefinitionDuration.Session:
                    defaultKey = "sessionMatchedGroups";
                    appSettingKey = AppConstants.ConfigKeys.CookieKeyForSessionMatchedGroups;
                    break;
                case PersonalisationGroupDefinitionDuration.Visitor:
                    defaultKey = "persistentMatchedGroups";
                    appSettingKey = AppConstants.ConfigKeys.CookieKeyForPersistentMatchedGroups;
                    break;
                default:
                    throw new InvalidOperationException("Only session and visitor personalisation groups can be tracked.");
            }

            // First check if key defined in config
            var cookieKey = ConfigurationManager.AppSettings[appSettingKey];
            if (string.IsNullOrEmpty(cookieKey))
            {
                // If not, use the convention key
                cookieKey = defaultKey;
            }

            return cookieKey;
        }

        /// <summary>
        /// Adds a matched group to the cookie for sticky groups
        /// </summary>
        /// <param name="matchedGroupIds">Existing cookie value of matched group node Ids</param>
        /// <param name="groupNodeId">Id of the matched groups node</param>
        /// <returns>Updated cookie value</returns>
        private static string AppendGroupNodeId(string matchedGroupIds, int groupNodeId)
        {
            // Shouldn't exist as we don't try to append an already sticky group match, but just to be sure
            if (!IsGroupMatched(matchedGroupIds, groupNodeId))
            {
                matchedGroupIds = matchedGroupIds + "," + groupNodeId;
            }

            return matchedGroupIds;
        }

        /// <summary>
        /// Checks if group is matched in tracking cookie value
        /// </summary>
        /// <param name="matchedGroupIds">Existing cookie value of matched group node Ids</param>
        /// <param name="groupNodeId">Id of the matched groups node</param>
        /// <returns>True if matched</returns>
        private static bool IsGroupMatched(string matchedGroupIds, int groupNodeId)
        {
            return matchedGroupIds
                .Split(',')
                .Any(x => int.Parse(x) == groupNodeId);
        }
    }
}
