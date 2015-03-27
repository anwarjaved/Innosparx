namespace Framework.FileSystem.Impl
{
    using System.IO;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Virtual folder.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/13/2013 12:55 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    internal class DiskVirtualFolder : DiskVirtualFileItem, IVirtualFolder
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the DiskVirtualFolder class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:55 PM.
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
        public DiskVirtualFolder(IVirtualFileSystem fileSystem, string relativePath, string name)
            : base(fileSystem, relativePath, name)
        {
            this.IsFolder = true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Queries if a given file exists.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:55 PM.
        /// </remarks>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual bool FileExists(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                string filePath = Path.Combine(this.RelativePath, fileName);

                return this.FileSystem.FileExists(filePath);
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a file.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:59 PM.
        /// </remarks>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        ///
        /// <returns>
        ///     The file.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual IVirtualFile GetFile(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                string filePath = Path.Combine(this.RelativePath, fileName);

                if (this.FileSystem.FileExists(filePath))
                {
                    IVirtualFileItem item = this.FileSystem.GetFile(filePath);

                    if (!item.IsFolder)
                    {
                        return (DiskVirtualFile)item;
                    }
                }
            }

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets relative file path.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:59 PM.
        /// </remarks>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        ///
        /// <returns>
        ///     The relative file path.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual string GetRelativeFilePath(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                string filePath = Path.Combine(this.RelativePath, fileName);

                if (this.FileSystem.FileExists(filePath))
                {
                    IVirtualFileItem item = this.FileSystem.GetFile(filePath);

                    if (!item.IsFolder)
                    {
                        return item.RelativePath;
                    }
                }
            }

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets absolute file path.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:59 PM.
        /// </remarks>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        ///
        /// <returns>
        ///     The absolute file path.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual string GetAbsoluteFilePath(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                string filePath = Path.Combine(this.RelativePath, fileName);

                if (this.FileSystem.FileExists(filePath))
                {
                    IVirtualFileItem item = this.FileSystem.GetFile(filePath);

                    if (!item.IsFolder)
                    {
                        return this.FileSystem.GetAbsolutePath(item);
                    }
                }
            }

            return string.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:59 PM.
        /// </remarks>
        ///
        /// <param name="folderName">
        ///     Pathname of the folder.
        /// </param>
        ///
        /// <returns>
        ///     The folder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual IVirtualFolder GetFolder(string folderName)
        {
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                string path = Path.Combine(this.RelativePath, folderName);

                return this.FileSystem.GetFolder(path);
            }

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Queries if a given folder exists.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 12:59 PM.
        /// </remarks>
        ///
        /// <param name="folderName">
        ///     Pathname of the folder.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual bool FolderExists(string folderName)
        {
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                string path = Path.Combine(this.RelativePath, folderName);

                return this.FileSystem.FolderExists(path);
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 3:27 PM.
        /// </remarks>
        ///
        /// <param name="folderName">
        ///     Pathname of the folder.
        /// </param>
        ///
        /// <returns>
        ///     The new folder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual IVirtualFolder CreateFolder(string folderName)
        {
            return this.FileSystem.CreateFolder(this, folderName);
        }
    }

}
