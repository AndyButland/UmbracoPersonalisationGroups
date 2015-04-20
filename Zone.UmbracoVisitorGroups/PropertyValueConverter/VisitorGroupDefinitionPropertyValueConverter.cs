namespace Zone.UmbracoVisitorGroups
{
    using Newtonsoft.Json;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web;

    [PropertyValueType(typeof(VisitorGroupDefinition))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.ContentCache)]
    public class VisitorGroupDefinitionPropertyValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals("visitorGroupDefinition");
        }

        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null || UmbracoContext.Current == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<VisitorGroupDefinition>(source.ToString());
        }
    }
}