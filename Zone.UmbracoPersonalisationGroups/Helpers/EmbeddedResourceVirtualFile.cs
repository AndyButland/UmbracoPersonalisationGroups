namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System.IO;
    using System.Linq;
    using ClientDependency.Core.CompositeFiles;
    using Zone.UmbracoPersonalisationGroups.Common;
    using Zone.UmbracoPersonalisationGroups.Common.ExtensionMethods;
    using Zone.UmbracoPersonalisationGroups.Controllers;

    /// <summary>
    /// The embedded resource virtual file.
    /// </summary>
    internal class EmbeddedResourceVirtualFile : IVirtualFile
    {
        /// <summary>
        /// The virtual path to the resource.
        /// </summary>
        private readonly string _virtualPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path to the resource represented by this instance. 
        /// </param>
        public EmbeddedResourceVirtualFile(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        /// <summary>
        /// Gets the path to the virtual resource.
        /// </summary>
        public string Path => _virtualPath;

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public Stream Open()
        {
            // Get this assembly.
            var assembly = typeof(ResourceController).Assembly;
            var output = EmbeddedResourceHelper.GetResource(assembly, this._virtualPath, out string resourceName);
            if (output != null)
            {
                return output;
            }

            // We need to loop through the loaded criteria and check each one.
            var localAssembly = assembly;
            var criteria = PersonalisationGroupMatcher .GetAvailableCriteria()
                .Where(a => a.GetType().Assembly != localAssembly);

            foreach (var criterion in criteria)
            {
                var resource = EmbeddedResourceHelper.SanitizeCriteriaResourceName(this._virtualPath);

                assembly = criterion.GetType().Assembly;
                resourceName = assembly.GetManifestResourceNames().FirstOrDefault(r => r.InvariantEndsWith(resource));

                if (!string.IsNullOrWhiteSpace(resourceName))
                {
                    return EmbeddedResourceHelper.GetResource(assembly, resource, out resourceName);
                }
            }

            return null;
        }
    }
}
