namespace Zone.UmbracoVisitorGroups.PropertyEditors
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using Constants = UmbracoVisitorGroups.Constants;

    /// <summary>
    /// Property editor for managing the definition of a visitor group
    /// </summary>
    [PropertyEditor(Constants.VisitorGroupDefinitionPropertyEditorAlias, "Visitor group definition", "/App_Plugins/UmbracoVisitorGroups/Resource/editor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoVisitorGroups/Resource/editor.controller.js")]
    public class VisitorGroupDefinitionPropertyEditor : PropertyEditor
    {
    }
}
