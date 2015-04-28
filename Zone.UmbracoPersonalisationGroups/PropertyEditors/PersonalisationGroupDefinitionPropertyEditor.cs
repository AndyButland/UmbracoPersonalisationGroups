namespace Zone.UmbracoPersonalisationGroups.PropertyEditors
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using Constants = Constants;

    /// <summary>
    /// Property editor for managing the definition of a personalisation group
    /// </summary>
    [PropertyEditor(Constants.PersonalisationGroupDefinitionPropertyEditorAlias, "Personalisation group definition", "/App_Plugins/UmbracoPersonalisationGroups/Resource/editor.html", ValueType = "JSON")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/Resource/editor.controller.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/authenticationStatus/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/authenticationStatus/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/cookie/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/cookie/definition.translator.js")]
    
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/dayOfWeek/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/dayOfWeek/definition.translator.js")]
    
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/session/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/session/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/timeOfDay/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/timeOfDay/definition.translator.js")]
    public class PersonalisationGroupDefinitionPropertyEditor : PropertyEditor
    {
    }
}
