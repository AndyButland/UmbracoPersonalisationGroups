namespace Zone.UmbracoPersonalisationGroups.PropertyEditors
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using Constants = Zone.UmbracoPersonalisationGroups.AppConstants;

    /// <summary>
    /// Property editor for managing the definition of a personalisation group
    /// </summary>
    [PropertyEditor(Constants.PersonalisationGroupDefinitionPropertyEditorAlias, "Personalisation group definition", Constants.ResourceRoot + "editor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceRoot + "editor.controller.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "authenticationStatus/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "authenticationStatus/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "cookie/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "cookie/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "country/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "country/definition.translator.js")]
    
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "dayOfWeek/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "dayOfWeek/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberGroup/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberGroup/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberProfileField/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberProfileField/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberType/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberType/definition.translator.js")]
    
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "pagesViewed/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "pagesViewed/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "referral/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "referral/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "session/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "session/definition.translator.js")]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "timeOfDay/definition.editor.controller.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "timeOfDay/definition.translator.js")]
    public class PersonalisationGroupDefinitionPropertyEditor : PropertyEditor
    {
    }
}
