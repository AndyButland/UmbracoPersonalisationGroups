namespace Zone.UmbracoPersonalisationGroups.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.Configuration;
    using Zone.UmbracoPersonalisationGroups.Common.GroupDefinition;
    using Zone.UmbracoPersonalisationGroups.Common.Helpers;

    internal static class UmbracoExtensionsHelper
    {
        internal static bool MatchGroup(IPublishedContent pickedGroup)
        {
            // Package is disabled, return default
            if (PersonalisationGroupsConfig.Value.DisablePackage)
            {
                return true;
            }

            return MatchGroups(new List<IPublishedContent> { pickedGroup });
        }

        internal static bool MatchGroups(IList<IPublishedContent> pickedGroups)
        {
            // Package is disabled, return default
            if (PersonalisationGroupsConfig.Value.DisablePackage)
            {
                return true;
            }

            // Check each personalisation group assigned for a match with the current site visitor
            foreach (var group in pickedGroups)
            {
                var definition = group.GetPropertyValue<PersonalisationGroupDefinition>(AppConstants.PersonalisationGroupDefinitionPropertyAlias);
                if (GroupMatchingHelper.IsStickyMatch(definition, group.Id))
                {
                    return true;
                }

                var matchCount = PersonalisationGroupMatcher.CountMatchingDefinitionDetails(definition);

                // If matching any and matched at least one, or matching all and matched all - we've matched one of the definitions 
                // associated with a selected personalisation group
                if ((definition.Match == PersonalisationGroupDefinitionMatch.Any && matchCount > 0) ||
                    (definition.Match == PersonalisationGroupDefinitionMatch.All && matchCount == definition.Details.Count()))
                {
                    GroupMatchingHelper.MakeStickyMatch(definition, group.Id);
                    return true;
                }
            }

            // If we've got here, we haven't found a match
            return false;
        }

        internal static bool MatchGroupsByName(string[] groupNames, IList<IPublishedContent> groups, PersonalisationGroupDefinitionMatch matchType)
        {
            // Package is disabled, return default
            if (PersonalisationGroupsConfig.Value.DisablePackage)
            {
                return true;
            }

            var matches = 0;
            foreach (var groupName in groupNames)
            {
                var group = groups
                    .FirstOrDefault(x => string.Equals(x.Name, groupName, StringComparison.InvariantCultureIgnoreCase));
                if (@group == null)
                {
                    continue;
                }

                if (MatchGroup(@group))
                {
                    if (matchType == PersonalisationGroupDefinitionMatch.Any)
                    {
                        return true;
                    }

                    matches++;
                }
                else
                {
                    if (matchType == PersonalisationGroupDefinitionMatch.All)
                    {
                        return false;
                    }
                }
            }

            return matches == groupNames.Length;
        }

        internal static int ScoreGroups(IList<IPublishedContent> pickedGroups)
        {
            // Package is disabled, return default
            if (PersonalisationGroupsConfig.Value.DisablePackage)
            {
                return 0;
            }

            // Check each personalisation group assigned for a match with the current site visitor
            var score = 0;
            foreach (var group in pickedGroups)
            {
                var definition = group.GetPropertyValue<PersonalisationGroupDefinition>(AppConstants.PersonalisationGroupDefinitionPropertyAlias);
                if (GroupMatchingHelper.IsStickyMatch(definition, group.Id))
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
                    GroupMatchingHelper.MakeStickyMatch(definition, group.Id);
                    score += definition.Score;
                }
            }

            return score;
        }
    }
}
