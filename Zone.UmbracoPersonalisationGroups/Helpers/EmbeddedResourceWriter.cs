namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System;
    using System.IO;
    using System.Web;

    using ClientDependency.Core;
    using ClientDependency.Core.CompositeFiles;
    using ClientDependency.Core.CompositeFiles.Providers;

    /// <summary>
    /// The embedded resource writer.
    /// </summary>
    public sealed class EmbeddedResourceWriter : IVirtualFileWriter
    {
        /// <summary>
        /// Gets the file provider.
        /// </summary>
        public IVirtualFileProvider FileProvider
        {
            get { return new EmbeddedResourceVirtualPathProvider(); }
        }

        /// <summary>
        /// Writes the virtual file to a stream for serving.
        /// </summary>
        /// <param name="provider">The file processing provider.</param>
        /// <param name="streamWriter">The <see cref="StreamWriter"/>.</param>
        /// <param name="virtualFile">The <see cref="IVirtualFile"/> to write.</param>
        /// <param name="type">The <see cref="ClientDependencyType"/> the virtual file matches.</param>
        /// <param name="originalUrl">The original url to the virtual file.</param>
        /// <param name="context">The <see cref="HttpContextBase"/>.</param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool WriteToStream(
            BaseCompositeFileProcessingProvider provider, 
            StreamWriter streamWriter, 
            IVirtualFile virtualFile, 
            ClientDependencyType type, 
            string originalUrl, 
            HttpContextBase context)
        {
            try
            {
                using (Stream readStream = virtualFile.Open())
                using (StreamReader streamReader = new StreamReader(readStream))
                {
                    string output = streamReader.ReadToEnd();
                    DefaultFileWriter.WriteContentToStream(provider, streamWriter, output, type, context, originalUrl);
                    return true;
                }
            }
            catch (Exception)
            {
                // The file must have failed to open
                return false;
            }
        }
    }
}
