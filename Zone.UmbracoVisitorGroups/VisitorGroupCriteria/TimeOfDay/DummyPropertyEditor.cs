namespace Zone.UmbracoVisitorGroups.VisitorGroupCriteria.TimeOfDay
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    /// <summary>
    /// This isn't a "real" property editor, it's rather a hack to inject the angular controller for this criteria definition builder.
    /// The PropertyEditorAsset attribute seems the only way currently to inject additional angular controllers before the application is bootstrapped.
    /// </summary>
    [PropertyEditor("visitorGroupDefinitionDayOfWeek", "Visitor group definition (dummy)")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoVisitorGroups/ResourceForCriteria/timeOfDay/definition.editor.controller.js")]
    public class DummyDefinitionPropertyEditor : PropertyEditor
    {
    }
}
