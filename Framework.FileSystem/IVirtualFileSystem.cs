namespace Framework.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for virtual file system.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/13/2013 4:57 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public interface IVirtualFileSystem
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the pathname of the root folder.
        /// </summary>
        ///
        /// <value>
        ///     The pathname of the root folder.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFolder RootFolder { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the pathname of the temporary folder.
        /// </summary>
        ///
        /// <value>
        ///     The pathname of the temporary folder.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFolder TempFolder { get; }

        string RootWebUrl { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement folder creation.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:18 PM.
        /// </remarks>
        ///
        /// <param name="parent">
        ///     The parent.
        /// </param>
        /// <param name="folderName">
        ///     The name.
        /// </param>
        ///
        /// <returns>
        ///     The new folder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFolder CreateFolder(IVirtualFolder parent, string folderName);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement file deletion.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:18 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void DeleteFile(IVirtualFile file);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement folder deletion.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:18 PM.
        /// </remarks>
        ///
        /// <param name="folder">
        ///     Pathname of the folder.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void DeleteFolder(IVirtualFolder folder);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement the file existence verification.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:18 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        ///
        /// <returns>
        ///     true if the file exists within a file manager; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool FileExists(IVirtualFile file);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement the file existence verification.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:18 PM.
        /// </remarks>
        ///
        /// <param name="relativePath">
        ///     Name of the full.
        /// </param>
        ///
        /// <returns>
        ///     true if the file exists within a file manager; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool FileExists(string relativePath);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement the folder existence verification.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:18 PM.
        /// </remarks>
        ///
        /// <param name="folder">
        ///     Pathname of the folder.
        /// </param>
        ///
        /// <returns>
        ///     true if the folder exists within a file manager; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool FolderExists(IVirtualFolder folder);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement the folder existence verification.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:18 PM.
        /// </remarks>
        ///
        /// <param name="relativePath">
        ///     Name of the full.
        /// </param>
        ///
        /// <returns>
        ///     true if the folder exists within a file manager; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool FolderExists(string relativePath);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement getting a collection of files that are located in the
        ///     current folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="folder">
        ///     Pathname of the folder.
        /// </param>
        ///
        /// <returns>
        ///     A collection of IVirtualFile objects.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IVirtualFile> GetFiles(IVirtualFolder folder);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement getting a collection of folders that are located in the
        ///     current folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="folder">
        ///     Pathname of the parent folder.
        /// </param>
        ///
        /// <returns>
        ///     A collection of IVirtualFolder objects.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IVirtualFolder> GetFolders(IVirtualFolder folder);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement getting the time of the last file modification.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        ///
        /// <returns>
        ///     A DateTime value that is the last write time.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        DateTime GetLastWriteTime(IVirtualFile file);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement moving a file.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        /// <param name="newParentFolder">
        ///     Pathname of the new parent folder.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile MoveFile(IVirtualFile file, IVirtualFolder newParentFolder);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement moving a folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="folder">
        ///     Pathname of the folder.
        /// </param>
        /// <param name="newParentFolder">
        ///     Pathname of the new parent folder.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFolder MoveFolder(IVirtualFolder folder, IVirtualFolder newParentFolder);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement file reading.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        ///
        /// <returns>
        ///     A Stream object that points to the processed file.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Stream ReadFile(IVirtualFile file);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement file writing.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        ///
        /// <returns>
        ///     A Stream object that points to the processed file.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Stream WriteFile(IVirtualFile file);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement file reading.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        /// <param name="bufferSize">
        ///     (optional) size of the buffer.
        /// </param>
        ///
        /// <returns>
        ///     A Stream object that points to the processed file.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Stream ReadFileAsync(IVirtualFile file, int bufferSize = FileSystemConstants.BufferSize);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement file writing.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        /// <param name="bufferSize">
        ///     (optional) size of the buffer.
        /// </param>
        ///
        /// <returns>
        ///     A Stream object that points to the processed file.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Stream WriteFileAsync(IVirtualFile file, int bufferSize = FileSystemConstants.BufferSize);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement renaming a file.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="file">
        ///     The file.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile RenameFile(IVirtualFile file, string name);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement renaming a folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="folder">
        ///     Pathname of the folder.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFolder RenameFolder(IVirtualFolder folder, string name);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement a file upload.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="folder">
        ///     Pathname of the folder.
        /// </param>
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        /// <param name="content">
        ///     (optional) the content.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile CreateFile(IVirtualFolder folder, string fileName, Stream content = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement a file upload.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="folder">
        ///     Pathname of the folder.
        /// </param>
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        /// <param name="content">
        ///     (optional) the content.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile CreateFile(IVirtualFolder folder, string fileName, string content = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement folder creation.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="name">
        ///     The name.
        /// </param>
        ///
        /// <returns>
        ///     The new folder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFolder CreateFolder(string name);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement getting a collection of folders that are located in the
        ///     root folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <returns>
        ///     A collection of IVirtualFolder objects.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IVirtualFolder> GetFolders();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement getting a collection of files that are located in the
        ///     root folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <returns>
        ///     A collection of IVirtualFile objects.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IVirtualFile> GetFiles();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a file.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="relativePath">
        ///     Name of the full.
        /// </param>
        ///
        /// <returns>
        ///     The file.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile GetFile(string relativePath);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a folder.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="relativePath">
        ///     Name of the full.
        /// </param>
        ///
        /// <returns>
        ///     The folder.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFolder GetFolder(string relativePath);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets absolute file/folder path.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="item">
        ///     The item.
        /// </param>
        ///
        /// <returns>
        ///     The absolute path.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string GetAbsolutePath(IVirtualFileItem item);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets web URL.
        /// </summary>
        ///
        /// <param name="item">
        ///     The item.
        /// </param>
        ///
        /// <returns>
        ///     The web URL.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string GetWebUrl(IVirtualFileItem item);


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement a file upload.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        /// <param name="content">
        ///     (optional) the content.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile CreateFile(string fileName, Stream content = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement a file upload.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        /// <param name="content">
        ///     (optional) the content.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile CreateTempFile(string fileName, Stream content = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement a file upload.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        /// <param name="content">
        ///     (optional) the content.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile CreateFile(string fileName, string content = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to implement a file upload.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:21 PM.
        /// </remarks>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        /// <param name="content">
        ///     (optional) the content.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFile CreateTempFile(string fileName, string content = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get The Virtual File System for specified user.
        /// </summary>
        ///
        /// <param name="name">
        ///     The name for which file system needed.
        /// </param>
        /// <param name="newCallback">
        ///     (optional) the callback.
        /// </param>
        /// <param name="rootWebUrl">
        ///     (optional) URL of the root web.
        /// </param>
        ///
        /// <returns>
        ///     A new <see cref="IVirtualFileSystem"/> for specified user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFileSystem With(string name, Action<IVirtualFileSystem> newCallback = null, string rootWebUrl = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get The Virtual File System for specified user.
        /// </summary>
        ///
        /// <param name="id">
        ///     The id for which file system needed.
        /// </param>
        /// <param name="newCallback">
        ///     (optional) the callback.
        /// </param>
        /// <param name="rootWebUrl">
        ///     (optional) URL of the root web.
        /// </param>
        ///
        /// <returns>
        ///     A new <see cref="IVirtualFileSystem"/> for specified user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IVirtualFileSystem With(Guid id, Action<IVirtualFileSystem> newCallback = null, string rootWebUrl = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets absolute file/folder path.
        /// </summary>
        ///
        /// <param name="relativePath">
        ///     Name of the full.
        /// </param>
        ///
        /// <returns>
        ///     The absolute path.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string GetAbsolutePath(string relativePath);
    }
}