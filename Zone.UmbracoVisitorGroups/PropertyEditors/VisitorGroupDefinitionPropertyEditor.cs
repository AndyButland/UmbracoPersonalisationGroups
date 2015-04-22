namespace Zone.UmbracoVisitorGroups.PropertyEditors
{
    using System.Collections.Generic;
    using System.Web;
    using ClientDependency.Core;
    using ClientDependency.Core.Config;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using Constants = UmbracoVisitorGroups.Constants;

    [PropertyEditor(Constants.VisitorGroupDefinitionPropertyEditorAlias, "Visitor group definition", "/App_Plugins/UmbracoVisitorGroups/Resource/editor.html", ValueType = "JSON")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoVisitorGroups/Resource/editor.controller.js")]
    // TODO: load this at runtime
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoVisitorGroups/ResourceForCriteria/DayOfWeek/definition.editor.controller.js")]
    public class VisitorGroupDefinitionPropertyEditor : PropertyEditor
    {
        public VisitorGroupDefinitionPropertyEditor()
        {
            // TODO: get this working
            LoadClientDependencies();
        }

        private void LoadClientDependencies()
        {
            var files = new List<IClientDependencyFile>();
            var file = new BasicFile(ClientDependencyType.Javascript)
            {
                FilePath =
                    "/App_Plugins/UmbracoVisitorGroups/ResourceForCriteria/DayOfWeek/definition.editor.controller.js"
            };
            files.Add(file);

            string jsOut;
            string cssOut;
            var renderer = ClientDependencySettings.Instance.MvcRendererCollection["Umbraco.DependencyPathRenderer"];
            renderer.RegisterDependencies(files, new HashSet<IClientDependencyPath>(), out jsOut, out cssOut, new HttpContextWrapper(HttpContext.Current));
        }
    }
}
