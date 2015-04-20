namespace Zone.UmbracoVisitorGroups.PropertyEditors
{
    using Umbraco.Core.PropertyEditors;

    [PropertyEditor(Constants.VisitorGroupDefinitionPropertyEditorAlias, "Visitor group definition", "/App_Plugins/UmbracoVisitorGroups/Resource/editor.html", ValueType = "JSON")]
    public class VisitorGroupDefinitionPropertyEditor : PropertyEditor
    {
    }
}
