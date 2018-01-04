namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using Umbraco.Core.Configuration;
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using Zone.UmbracoPersonalisationGroups.Configuration;

    internal static class UmbracoExtensionsHelper
    {
        internal static bool MatchGroup(IPublishedContent pickedGroup)
        {
            // Package is disabled, return default
            if (UmbracoConfig.For.PersonalisationGroups().DisablePackage)
            {
                return true;
            }

            return MatchGroups(new List<IPublishedContent> { pickedGroup });
        }

        internal static bool MatchGroups(IList<IPublishedContent> pickedGroups)
        {
            // Package is disabled, return default
            if (UmbracoConfig.For.PersonalisationGroups().DisablePackage)
            {
                return true;
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

        internal static bool MatchGroupsByName(string[] groupNames, IList<IPublishedContent> groups, PersonalisationGroupDefinitionMatch matchType)
        {
            // Package is disabled, return default
            if (UmbracoConfig.For.PersonalisationGroups().DisablePackage)
            {
                return true;
            }

            var matches = 0;
            foreach (var groupName in groupNames)
            {
                var group = groups
                    .FirstOrDefault(x => string.Equals(x.Name, groupName, StringComparison.InvariantCultureIgnoreCase));
                if (group != null)
                {
                    if (MatchGroup(group))
                    {
                        if (matchType == PersonalisationGroupDefinitionMatch.Any)
                        {
                            return true;
                        }
                        else
                        {
                            matches++;
                        }
                    }
                    else
                    {
                        if (matchType == PersonalisationGroupDefinitionMatch.All)
                        {
                            return false;
                        }
                    }
                }
            }

            return matches == groupNames.Count();
        }

        internal static int ScoreGroups(IList<IPublishedContent> pickedGroups)
        {
            // Package is disabled, return default
            if (UmbracoConfig.For.PersonalisationGroups().DisablePackage)
            {
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
                    continue;
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
                var cookieExpiryInDays = UmbracoConfig.For.PersonalisationGroups().PersistentMatchedGroupsCookieExpiryInDays;
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
            var config = UmbracoConfig.For.PersonalisationGroups();
            switch (duration)
            {
                case PersonalisationGroupDefinitionDuration.Session:
                    return config.CookieKeyForSessionMatchedGroups;
                case PersonalisationGroupDefinitionDuration.Visitor:
                    return config.CookieKeyForPersistentMatchedGroups;
                default:
                    throw new InvalidOperationException("Only session and visitor personalisation groups can be tracked.");
            }
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
