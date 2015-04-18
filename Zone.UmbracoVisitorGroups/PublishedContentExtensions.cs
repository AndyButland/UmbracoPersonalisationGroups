namespace Umbraco.Web
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Models;
    using Zone.UmbracoVisitorGroups;

    public static class PublishedContentExtensions
    {
        public static bool ShowToVisitor(this IPublishedContent content)
        {
            var pickerProperty = GetPickerProperty(content);
            if (pickerProperty == null || pickerProperty.Value == null)
            {
                return true;
            }

            var pickedVisitorGroups = GetPickedVisitorGroups(pickerProperty).ToList();
            if (!pickedVisitorGroups.Any())
            {
                // No visitor groups picked, so we assume available to all
                return true;
            }

            foreach (var visitorGroup in pickedVisitorGroups)
            {
                var definition = visitorGroup.GetPropertyValue<VisitorGroupDefinition>("definition");
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

        private static IPublishedProperty GetPickerProperty(IPublishedContent content)
        {
            return content.Properties
                .FirstOrDefault(x => x.PropertyTypeAlias == "visitorGroupPicker");
        }

        private static IEnumerable<IPublishedContent> GetPickedVisitorGroups(IPublishedProperty pickerProperty)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var pickedVisitorGroupIds = pickerProperty.Value.ToString()
                .Split(',')
                .Select(x => int.Parse(x));
            return umbracoHelper.TypedContent(pickedVisitorGroupIds);
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
