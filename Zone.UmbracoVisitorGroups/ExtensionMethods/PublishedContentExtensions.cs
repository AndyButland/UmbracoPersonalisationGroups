namespace Umbraco.Web
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Umbraco.Core.Models;
    using Zone.UmbracoVisitorGroups;

    public static class PublishedContentExtensions
    {
        public static bool ShowToVisitor(this IPublishedContent content)
        {
            var pickedVisitorGroups = GetPickedVisitorGroups(content).ToList();
            if (!pickedVisitorGroups.Any())
            {
                // No visitor groups picked or no property for picker, so we assume available to all
                return true;
            }

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

        private static IEnumerable<IPublishedContent> GetPickedVisitorGroups(IPublishedContent content)
        {
            if (content.HasProperty(GetVisitorGroupPickerAlias()))
            {
                var pickedVisitorGroupIds = content.GetProperty("propertyAlias").DataValue.ToString()
                    .Split(',')
                    .Select(x => int.Parse(x));

                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                return umbracoHelper.TypedContent(pickedVisitorGroupIds);
            }

            return Enumerable.Empty<IPublishedContent>();
        }

        private static string GetVisitorGroupPickerAlias()
        {
            var visitorGroupPickerAlias = ConfigurationManager.AppSettings["visitorGroups.visitorGroupPickerAlias"];
            if (string.IsNullOrEmpty(visitorGroupPickerAlias))
            {
                visitorGroupPickerAlias = Constants.DefaultVisitorGroupPickerAlias;
            }

            return visitorGroupPickerAlias;
        }

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
