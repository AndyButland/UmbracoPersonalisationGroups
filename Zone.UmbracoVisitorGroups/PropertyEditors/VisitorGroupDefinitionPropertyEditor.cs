namespace Zone.UmbracoVisitorGroups.PropertyEditors
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using Constants = UmbracoVisitorGroups.Constants;

    [PropertyEditor(Constants.VisitorGroupDefinitionPropertyEditorAlias, "Visitor group definition", "/App_Plugins/UmbracoVisitorGroups/Resource/editor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoVisitorGroups/Resource/editor.controller.js")]
    
    // TODO: remove these as want to load at run-time
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoVisitorGroups/ResourceForCriteria/dayOfWeek/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoVisitorGroups/ResourceForCriteria/timeOfDay/definition.editor.controller.js")]
    public class VisitorGroupDefinitionPropertyEditor : PropertyEditor
    {
    }
}
