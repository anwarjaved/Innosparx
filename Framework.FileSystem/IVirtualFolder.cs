namespace Framework.FileSystem
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for virtual folder.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/13/2013 4:54 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public interface IVirtualFolder : IVirtualFileItem
    {
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
        bool FileExists(string fileName);

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
        IVirtualFile GetFile(string fileName);

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
        string GetRelativeFilePath(string fileName);

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
        string GetAbsoluteFilePath(string fileName);

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
        IVirtualFolder GetFolder(string folderName);

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
        bool FolderExists(string folderName);

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
        IVirtualFolder CreateFolder(string folderName);
    }
}