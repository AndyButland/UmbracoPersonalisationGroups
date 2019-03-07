namespace Zone.UmbracoPersonalisationGroups.V8.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web;
    using Umbraco.Web.Composing;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.GroupDefinition;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    /// <summary>
    /// Provides extension methods to UmbracoHelper
    /// </summary>
    public static class UmbracoHelperExtensions
    {
        /// <summary>
        /// Adds an extension method to UmbracoHelper to determine if the current site
        /// visitor matches a single personalisation group.
        /// </summary>
        /// <param name="helper">Instance of UmbracoHelper</param>
        /// <param name="groupName">Name of group node to match</param>
        /// <returns>True if visitor matches group</returns>
        public static bool MatchesGroup(this UmbracoHelper helper, string groupName)
        {
            return MatchesGroups(helper, new string[] { groupName }, PersonalisationGroupDefinitionMatch.Any);
        }

        /// <summary>
        /// Adds an extension method to UmbracoHelper to determine if the current site
        /// visitor matches any of a set of personalisation groups.
        /// </summary>
        /// <param name="helper">Instance of UmbracoHelper</param>
        /// <param name="groupNames">Names of group nodes to match</param>
        /// <returns>True if visitor matches any group</returns>
        public static bool MatchesAnyGroup(this UmbracoHelper helper, string[] groupNames)
        {
            return MatchesGroups(helper, groupNames, PersonalisationGroupDefinitionMatch.Any);
        }

        /// <summary>
        /// Adds an extension method to UmbracoHelper to determine if the current site
        /// visitor matches all of a set of personalisation groups.
        /// </summary>
        /// <param name="helper">Instance of UmbracoHelper</param>
        /// <param name="groupNames">Names of group nodes to match</param>
        /// <returns>True if visitor matches all groups</returns>
        public static bool MatchesAllGroups(this UmbracoHelper helper, string[] groupNames)
        {
            return MatchesGroups(helper, groupNames, PersonalisationGroupDefinitionMatch.All);
        }

        private static bool MatchesGroups(this UmbracoHelper helper, string[] groupNames, PersonalisationGroupDefinitionMatch matchType)
        {
            var groupsRootFolder = GetGroupsRootFolder(helper);
            if (groupsRootFolder == null)
            {
                return false;
            }

            var groups = GetGroups(groupsRootFolder);
            return UmbracoExtensionsHelper.MatchGroupsByName(groupNames, groups, matchType);
        }

        private static IPublishedContent GetGroupsRootFolder(UmbracoHelper helper)
        {
            return helper.ContentAtRoot()
                .FirstOrDefault(x => x.IsDocumentType(AppConstants.DocumentTypeAliases.PersonalisationGroupsFolder));
        }

        private static IList<IPublishedContent> GetGroups(IPublishedContent groupsRootFolder)
        {
            return groupsRootFolder.Descendants()
                .Where(x => x.IsDocumentType(AppConstants.DocumentTypeAliases.PersonalisationGroup))
                .ToList();
        }

        /// <summary>
        /// Adds an extension method to UmbracoHelper to calculate a hash for the current visitor for all visitor groups
        /// </summary>
        /// <param name="helper">Instance of UmbracoHelper</param>
        /// <param name="personalisationGroupsRootNodeId">Id of root node for the personalisation groups</param>
        /// <param name="cacheUserIdentifier">Identifier for the user to use in the cache key (likely the session Id)</param>
        /// <param name="cacheForSeconds">Length of time in seconds to cache the generated personalisation group hash for the visitor</param>
        /// <returns>Has for the visitor for all groups</returns>
        public static string GetPersonalisationGroupsHashForVisitor(this UmbracoHelper helper, Guid personalisationGroupsRootNodeId, 
            string cacheUserIdentifier, int cacheForSeconds)
        {
            var personalisationGroupsRootNode = helper.Content(personalisationGroupsRootNodeId);
            if (!personalisationGroupsRootNode.IsDocumentType(AppConstants.DocumentTypeAliases.PersonalisationGroupsFolder))
            {
                throw new InvalidOperationException(
                    $"The personalisation groups hash for a visitor can only be calculated for a root node of type {AppConstants.DocumentTypeAliases.PersonalisationGroupsFolder}");
            }

            return GetPersonalisationGroupsHashForVisitor(helper, personalisationGroupsRootNode, cacheUserIdentifier, cacheForSeconds);
        }

        /// <summary>
        /// Adds an extension method to UmbracoHelper to calculate a hash for the current visitor for all visitor groups
        /// </summary>
        /// <param name="helper">Instance of UmbracoHelper</param>
        /// <param name="personalisationGroupsRootNodeId">Id of root node for the personalisation groups</param>
        /// <param name="cacheUserIdentifier">Identifier for the user to use in the cache key (likely the session Id)</param>
        /// <param name="cacheForSeconds">Length of time in seconds to cache the generated personalisation group hash for the visitor</param>
        /// <returns>Has for the visitor for all groups</returns>
        public static string GetPersonalisationGroupsHashForVisitor(this UmbracoHelper helper, int personalisationGroupsRootNodeId,
            string cacheUserIdentifier, int cacheForSeconds)
        {
            var personalisationGroupsRootNode = helper.Content(personalisationGroupsRootNodeId);
            if (!personalisationGroupsRootNode.IsDocumentType(AppConstants.DocumentTypeAliases.PersonalisationGroupsFolder))
            {
                throw new InvalidOperationException(
                    $"The personalisation groups hash for a visitor can only be calculated for a root node of type {AppConstants.DocumentTypeAliases.PersonalisationGroupsFolder}");
            }

            return GetPersonalisationGroupsHashForVisitor(helper, personalisationGroupsRootNode, cacheUserIdentifier, cacheForSeconds);
        }

        /// <summary>
        /// Adds an extension method to UmbracoHelper to calculate a hash for the current visitor for all visitor groups
        /// </summary>
        /// <param name="helper">Instance of UmbracoHelper</param>
        /// <param name="personalisationGroupsRootNode">Root node for the personalisation groups</param>
        /// <param name="cacheUserIdentifier">Identifier for the user to use in the cache key (likely the session Id)</param>
        /// <param name="cacheForSeconds">Length of time in seconds to cache the generated personalisation group hash for the visitor</param>
        /// <returns>Has for the visitor for all groups</returns>
        public static string GetPersonalisationGroupsHashForVisitor(this UmbracoHelper helper, IPublishedContent personalisationGroupsRootNode,
            string cacheUserIdentifier, int cacheForSeconds)
        {
            Mandate.ParameterNotNull(personalisationGroupsRootNode, "personalisationGroupsRootNode");

            var cacheKey = $"{cacheUserIdentifier}-{AppConstants.CacheKeys.PersonalisationGroupsVisitorHash}";
            return Current.AppCaches.RuntimeCache.GetCacheItem(cacheKey,
                () => CreatePersonalisationGroupsHashForVisitor(personalisationGroupsRootNode), 
                timeout: TimeSpan.FromSeconds(cacheForSeconds));
        }

        private static string CreatePersonalisationGroupsHashForVisitor(IPublishedContent personalisationGroupsRootNode)
        {
            var groups = personalisationGroupsRootNode.Descendants(AppConstants.DocumentTypeAliases.PersonalisationGroup);
            var sb = new StringBuilder();
            foreach (var group in groups)
            {
                var definition = group.Value<PersonalisationGroupDefinition>(AppConstants.PersonalisationGroupDefinitionPropertyAlias);
                GroupMatchingHelper.AppendMatchedGroupDetailToVisitorHashString(sb, definition, group.CreatorName);
            }

            return sb.ToString().GetHashCode().ToString();
        }
    }
}
