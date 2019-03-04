namespace Zone.UmbracoPersonalisationGroups.V8.PropertyValueConverter
{
    using System;
    using Newtonsoft.Json;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.GroupDefinition;

    /// <summary>
    /// Property converter to convert the saved JSON representation of a personalisation group definition to the
    /// strongly typed model
    /// </summary>
    public class PersonalisationGroupDefinitionPropertyValueConverter : PropertyValueConverterBase
    {
        public override Type GetPropertyValueType(PublishedPropertyType propertyType)
            => typeof(PersonalisationGroupDefinition);

        public override PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType)
            => PropertyCacheLevel.Element;

        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.Equals(AppConstants.PersonalisationGroupDefinitionPropertyEditorAlias);
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, PublishedPropertyType propertyType, object source, bool preview)
        {
            return source;
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, PublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            if (inter == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<PersonalisationGroupDefinition>(inter.ToString());
        }
    }
}