namespace Zone.UmbracoPersonalisationGroups.V8.PropertyEditors
{
    using ClientDependency.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using Constants = Zone.UmbracoPersonalisationGroups.Common.AppConstants;

    /// <summary>
    /// Property editor for managing the definition of a personalisation group
    /// </summary>
    [DataEditor(Constants.PersonalisationGroupDefinitionPropertyEditorAlias, "Personalisation group definition", Constants.ResourceRoot + "editor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceRoot + "editor.controller.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "authenticationStatus/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "authenticationStatus/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "cookie/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "cookie/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "country/geolocation.service.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "country/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "country/definition.translator.js" + Constants.ResourceExtension)]
    
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "dayOfWeek/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "dayOfWeek/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "host/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "host/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberGroup/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberGroup/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberProfileField/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberProfileField/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberType/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "memberType/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "numberOfVisits/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "numberOfVisits/definition.translator.js" + Constants.ResourceExtension)]
    
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "pagesViewed/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "pagesViewed/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "querystring/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "querystring/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "referral/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "referral/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "region/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "region/definition.translator.js" + Constants.ResourceExtension)]
    
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "session/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "session/definition.translator.js" + Constants.ResourceExtension)]

    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "timeOfDay/definition.editor.controller.js" + Constants.ResourceExtension)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, Constants.ResourceForCriteriaRoot + "timeOfDay/definition.translator.js" + Constants.ResourceExtension)]
    public class PersonalisationGroupDefinitionPropertyEditor : DataEditor
    {
        public PersonalisationGroupDefinitionPropertyEditor(ILogger logger, EditorType type = EditorType.PropertyValue)
            : base(logger, type)
        {
        }
    }
}
