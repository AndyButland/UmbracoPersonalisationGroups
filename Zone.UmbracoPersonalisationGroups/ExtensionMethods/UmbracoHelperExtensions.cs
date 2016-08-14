namespace Umbraco.Web
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Zone.UmbracoPersonalisationGroups;

    /// <summary>
    /// Provides extension methods to UmbracoHelper
    /// </summary>
    public static class UmbracoHelperExtensions
    {
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
            var personalisationGroupsRootNode = helper.TypedContent(personalisationGroupsRootNodeId);
            if (personalisationGroupsRootNode.DocumentTypeAlias != AppConstants.DocumentTypeAliases.PersonalisationGroupsFolder)
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
            var personalisationGroupsRootNode = helper.TypedContent(personalisationGroupsRootNodeId);
            if (personalisationGroupsRootNode.DocumentTypeAlias != AppConstants.DocumentTypeAliases.PersonalisationGroupsFolder)
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
            return (string)UmbracoContext.Current.Application.ApplicationCache.RuntimeCache
                .GetCacheItem(cacheKey,
                    () =>
                    {
                        var groups = personalisationGroupsRootNode.Descendants(AppConstants.DocumentTypeAliases.PersonalisationGroup);
                        var sb = new StringBuilder();
                        foreach (var group in groups)
                        {
                            var definition = group.GetPropertyValue<PersonalisationGroupDefinition>(AppConstants.PersonalisationGroupDefinitionPropertyAlias);
                            var matchCount = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);
                            var matched = ((definition.Match == PersonalisationGroupDefinitionMatch.Any && matchCount > 0) ||
                                (definition.Match == PersonalisationGroupDefinitionMatch.All && matchCount == definition.Details.Count()));

                            if (sb.Length > 0)
                            {
                                sb.Append(",");
                            }

                            sb.AppendFormat("{0}={1}", group.Name, matched);
                        }

                        return sb.ToString().GetHashCode().ToString();
                    }, timeout: TimeSpan.FromSeconds(cacheForSeconds));

        }
    }
}
