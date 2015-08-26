﻿namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System.IO;

    using ClientDependency.Core.CompositeFiles;

    /// <summary>
    /// The embedded resource virtual file.
    /// </summary>
    internal class EmbeddedResourceVirtualFile : IVirtualFile
    {
        /// <summary>
        /// The virtual path to the resource.
        /// </summary>
        private readonly string virtualPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path to the resource represented by this instance. 
        /// </param>
        public EmbeddedResourceVirtualFile(string virtualPath)
        {
            this.virtualPath = virtualPath;
        }

        /// <summary>
        /// Gets the path to the virtual resource.
        /// </summary>
        public string Path
        {
            get { return this.virtualPath; }
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public Stream Open()
        {
            string resourceName;
            return EmbeddedResourceHelper.GetResource(this.virtualPath, out resourceName);
        }
    }
}
