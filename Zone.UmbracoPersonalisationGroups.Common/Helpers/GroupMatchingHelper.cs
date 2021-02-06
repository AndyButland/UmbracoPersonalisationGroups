namespace Zone.UmbracoPersonalisationGroups.Common.Helpers
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.GroupDefinition;
    using Zone.UmbracoPersonalisationGroups.Common.Providers.Cookie;

    internal static class GroupMatchingHelper
    {
        internal static bool IsStickyMatch(PersonalisationGroupDefinition definition, int groupNodeId)
        {
            if (definition.Duration == PersonalisationGroupDefinitionDuration.Page)
            {
                return false;
            }

            var key = GetCookieKeyForMatchedGroups(definition.Duration);
            var cookieProvider = new HttpContextCookieProvider();
            var cookieValue = cookieProvider.GetCookieValue(key);
            return !string.IsNullOrEmpty(cookieValue) && IsGroupMatched(cookieValue, groupNodeId);
        }

        /// <summary>
        /// Makes a matched group sticky for the visitor via a cookie setting according to group definition
        /// </summary>
        /// <param name="definition">Matched group definition</param>
        /// <param name="groupNodeId">Id of the matched groups node</param>
        internal static void MakeStickyMatch(PersonalisationGroupDefinition definition, int groupNodeId)
        {
            if (definition.Duration == PersonalisationGroupDefinitionDuration.Page)
            {
                return;
            }

            var key = GetCookieKeyForMatchedGroups(definition.Duration);
            var cookieProvider = new HttpContextCookieProvider();
            var cookie = cookieProvider.GetCookie(key);
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
                var cookieExpiryInDays = PersonalisationGroupsConfig.Value.PersistentMatchedGroupsCookieExpiryInDays;
                cookie.Expires = DateTime.Now.AddDays(cookieExpiryInDays);
            }

            cookieProvider.SetCookie(cookie);
        }

        /// <summary>
        /// Retrieves the cookie key to use for the matched groups
        /// </summary>
        /// <param name="duration">Match group duration</param>
        /// <returns>Cookie key to use</returns>
        private static string GetCookieKeyForMatchedGroups(PersonalisationGroupDefinitionDuration duration)
        {
            var config = PersonalisationGroupsConfig.Value;
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

        public static void AppendMatchedGroupDetailToVisitorHashString(StringBuilder sb, PersonalisationGroupDefinition definition, string name)
        {
            var matchCount = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);
            var matched = (definition.Match == PersonalisationGroupDefinitionMatch.Any && matchCount > 0) || 
                          (definition.Match == PersonalisationGroupDefinitionMatch.All && matchCount == definition.Details.Count());

            if (sb.Length > 0)
            {
                sb.Append(",");
            }

            sb.AppendFormat("{0}={1}", name, matched);
        }
    }
}