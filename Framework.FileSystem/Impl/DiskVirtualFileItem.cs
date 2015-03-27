namespace Framework.FileSystem.Impl
{
    using System.Diagnostics;
    using System.Globalization;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Base item.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal abstract class DiskVirtualFileItem : IVirtualFileItem
    {
        private readonly string name;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the DiskVirtualFileItem class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:52 PM.
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
        protected DiskVirtualFileItem(IVirtualFileSystem fileSystem, string relativePath, string name)
        {
            this.name = name.ToLower(CultureInfo.CurrentCulture);
            this.FileSystem = fileSystem;
            this.RelativePath = relativePath.Replace(@"\", "/").ToLower(CultureInfo.CurrentCulture);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the full pathname of the relative file.
        /// </summary>
        ///
        /// <value>
        ///     The full pathname of the relative file.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string RelativePath { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the full pathname of the absolute file.
        /// </summary>
        ///
        /// <value>
        ///     The full pathname of the absolute file.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string AbsolutePath
        {
            get
            {
                return this.FileSystem.GetAbsolutePath(this);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets Web URL for item.
        /// </summary>
        ///
        /// <value>
        ///     The web URL.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string WebUrl
        {
            get
            {
                return this.FileSystem.GetWebUrl(this);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether this DiskVirtualFileItem is folder.
        /// </summary>
        ///
        /// <value>
        ///     true if this DiskVirtualFileItem is folder, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool IsFolder { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the file system.
        /// </summary>
        ///
        /// <value>
        ///     The file system.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public IVirtualFileSystem FileSystem { get; private set; }

        // ReSharper disable UnusedMember.Local
        private string DebuggerDisplay
            // ReSharper restore UnusedMember.Local
        {
            get { return string.Format("Name: {0}, RelativePath: {1}", this.Name, this.RelativePath); }
        }
    }
}
