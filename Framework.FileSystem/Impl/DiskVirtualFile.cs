namespace Framework.FileSystem.Impl
{
    using System.IO;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Virtual file.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/13/2013 12:54 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    internal class DiskVirtualFile : DiskVirtualFileItem, IVirtualFile
    {
        private readonly string extension;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the DiskVirtualFile class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:54 PM.
        /// </remarks>
        ///
        /// <param name="fileSystem">
        ///     The file system.
        /// </param>
        /// <param name="relativePath">
        ///     Full pathname of the relative file.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public DiskVirtualFile(IVirtualFileSystem fileSystem, string relativePath, string name)
            : base(fileSystem, relativePath, name)
        {
            this.extension = Path.GetExtension(name);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the extension.
        /// </summary>
        ///
        /// <value>
        ///     The extension.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Extension
        {
            get
            {
                return this.extension;
            }
        }
    }
}
