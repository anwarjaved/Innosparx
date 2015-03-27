namespace Framework.FileSystem.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Framework.Ioc;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Physical file system.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/13/2013 1:30 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public sealed class DiskStorageFileSystem : IVirtualFileSystem
    {
        private readonly string rootFolderName;

        private readonly Action<IVirtualFileSystem> callback;

        private readonly string rootPath;

        private readonly string folderID;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the DiskStorageFileSystem class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 1:31 PM.
        /// </remarks>
        ///
        /// <param name="rootPath">
        ///     Full pathname of the root file.
        /// </param>
        /// <param name="callback">
        ///     The callback.
        /// </param>
        /// <param name="rootFolderName">
        ///     (optional) pathname of the root folder.
        /// </param>
        /// <param name="rootWebUrl">
        ///     (optional) URL of the root web.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public DiskStorageFileSystem(string rootPath, Action<IVirtualFileSystem> callback = null, string rootFolderName = FileSystemConstants.RootFolderName, string rootWebUrl = "/" + FileSystemConstants.RootFolderName)
        {
            this.rootFolderName = rootFolderName;
            this.callback = callback;
            this.RootWebUrl = rootWebUrl;
            rootPath.ThrowIfNull("rootPath");
            this.rootPath = rootPath.Replace("/", @"\");
            this.Initialize();
        }

        private DiskStorageFileSystem(string rootPath, string id, Action<IVirtualFileSystem> callback = null, string rootFolderName = FileSystemConstants.RootFolderName, string rootWebUrl = "/" + FileSystemConstants.RootFolderName)
        {
            rootPath.ThrowIfNull("rootPath");
            this.rootFolderName = rootFolderName;
            this.callback = callback;
            this.RootWebUrl = rootWebUrl;
            this.folderID = id;
            this.rootPath = rootPath.Replace("/", @"\");
            this.Initialize();
        }

        public IVirtualFolder RootFolder { get; private set; }
        public IVirtualFolder TempFolder { get; private set; }

        public string RootWebUrl { get; private set; }


        public IVirtualFolder CreateFolder(IVirtualFolder parent, string folderName)
        {
            if (parent != null && !string.IsNullOrWhiteSpace(folderName))
            {
                if (parent.FileSystem != this)
                {
                    parent.FileSystem.CreateFolder(parent, folderName);
                }

                string relativePath = Path.Combine(parent.RelativePath, folderName);
                string path = this.GetAbsolutePath(relativePath);

                DirectoryInfo directory = new DirectoryInfo(path);
                if (!directory.Exists)
                {
                    directory.Create();
                }

                return new DiskVirtualFolder(this, relativePath, folderName);
            }
            return null;
        }

        public void DeleteFile(IVirtualFile file)
        {
            if (file != null)
            {
                if (file.FileSystem != this)
                {
                    file.FileSystem.DeleteFile(file);
                }

                string path = this.GetAbsolutePath(file.RelativePath);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public void DeleteFolder(IVirtualFolder folder)
        {
            if (folder != null)
            {
                if (folder.FileSystem != this)
                {
                    folder.FileSystem.DeleteFolder(folder);
                }

                string path = this.GetAbsolutePath(folder.RelativePath);

                if (Directory.Exists(path))
                {
                    DeleteDirectory(path);
                }
            }
        }

        private static void DeleteDirectory(string targetDirectory)
        {
            string[] files = Directory.GetFiles(targetDirectory);
            string[] dirs = Directory.GetDirectories(targetDirectory);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDirectory, false);
        }

        public bool FileExists(IVirtualFile file)
        {
            if (file != null)
            {
                if (file.FileSystem != this)
                {
                    return file.FileSystem.FileExists(file);
                }

                return this.FileExists(file.RelativePath);
            }

            return false;
        }

        public bool FileExists(string relativePath)
        {
            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                string path = this.GetAbsolutePath(relativePath);

                return File.Exists(path);
            }

            return false;
        }

        public bool FolderExists(IVirtualFolder folder)
        {
            if (folder != null)
            {
                if (folder.FileSystem != this)
                {
                    return folder.FileSystem.FolderExists(folder);
                }

                return this.FolderExists(folder.RelativePath);
            }

            return false;
        }

        public bool FolderExists(string relativePath)
        {
            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                string path = this.GetAbsolutePath(relativePath);

                return Directory.Exists(path);
            }

            return false;
        }

        public IReadOnlyList<IVirtualFile> GetFiles(IVirtualFolder folder)
        {
            List<IVirtualFile> files = new List<IVirtualFile>();
            if (folder != null)
            {
                if (folder.FileSystem != this)
                {
                    return folder.FileSystem.GetFiles(folder);
                }

                string path = this.GetAbsolutePath(folder);
                DirectoryInfo directory = new DirectoryInfo(path);
                if (directory.Exists)
                {
                    files.AddRange(
                        directory.GetFiles()
                                     .Where(info => ((info.Attributes & FileAttributes.Hidden) == 0) || (info.Attributes & FileAttributes.System) == 0)
                                     .Select(fileInfo => new DiskVirtualFile(this, Path.Combine(folder.RelativePath, fileInfo.Name), fileInfo.Name)));
                }

            }

            return files;
        }

        public IReadOnlyList<IVirtualFolder> GetFolders(IVirtualFolder folder)
        {
            List<IVirtualFolder> folders = new List<IVirtualFolder>();
            if (folder != null)
            {
                if (folder.FileSystem != this)
                {
                    return folder.FileSystem.GetFolders(folder);
                }

                string path = this.GetAbsolutePath(folder);
                DirectoryInfo directory = new DirectoryInfo(path);
                if (directory.Exists)
                {
                    folders.AddRange(
                        directory.GetDirectories()
                                     .Where(info => ((info.Attributes & FileAttributes.Hidden) == 0) || (info.Attributes & FileAttributes.System) == 0)
                                     .Select(fileInfo => new DiskVirtualFolder(this, Path.Combine(folder.RelativePath, fileInfo.Name), fileInfo.Name)));
                }

            }

            return folders;
        }

        public DateTime GetLastWriteTime(IVirtualFile file)
        {
            if (file != null)
            {
                if (file.FileSystem != this)
                {
                    return file.FileSystem.GetLastWriteTime(file);
                }

                string path = this.GetAbsolutePath(file.RelativePath);

                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    return fileInfo.LastWriteTimeUtc;
                }
            }

            return DateTime.MinValue;
        }

        public IVirtualFile MoveFile(IVirtualFile file, IVirtualFolder newParentFolder)
        {
            if (file != null && newParentFolder != null)
            {
                string sourcePath = file.AbsolutePath;
                string destinationParentPath = newParentFolder.AbsolutePath;


                if (File.Exists(sourcePath) && Directory.Exists(destinationParentPath))
                {
                    string relativePath = Path.Combine(newParentFolder.RelativePath, file.Name);
                    string destinationPath = newParentFolder.FileSystem.GetAbsolutePath(relativePath);

                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }

                    File.Move(sourcePath, destinationPath);
                    return new DiskVirtualFile(newParentFolder.FileSystem, relativePath, file.Name);
                }
            }

            return null;
        }

        public IVirtualFolder MoveFolder(IVirtualFolder folder, IVirtualFolder newParentFolder)
        {
            if (folder != null && newParentFolder != null)
            {
                string sourcePath = folder.AbsolutePath;
                string destinationParentPath = newParentFolder.AbsolutePath;


                if (Directory.Exists(sourcePath) && Directory.Exists(destinationParentPath))
                {
                    string relativePath = Path.Combine(newParentFolder.RelativePath, folder.Name);
                    string destinationPath = newParentFolder.FileSystem.GetAbsolutePath(relativePath);

                    Directory.Move(sourcePath, destinationPath);
                    return new DiskVirtualFolder(newParentFolder.FileSystem, relativePath, folder.Name);
                }
            }

            return null;
        }

        public Stream ReadFile(IVirtualFile file)
        {
            if (file != null)
            {
                if (file.FileSystem != this)
                {
                    return file.FileSystem.ReadFile(file);
                }

                string path = this.GetAbsolutePath(file);

                if (File.Exists(path))
                {
                    return File.OpenRead(path);
                }
            }

            return Stream.Null;
        }

        public Stream WriteFile(IVirtualFile file)
        {
            if (file != null)
            {
                if (file.FileSystem != this)
                {
                    return file.FileSystem.WriteFile(file);
                }

                string path = this.GetAbsolutePath(file);

                if (File.Exists(path))
                {
                    return File.OpenWrite(path);
                }
            }

            return Stream.Null;
        }

        public Stream ReadFileAsync(IVirtualFile file, int bufferSize = FileSystemConstants.BufferSize)
        {
            if (file != null)
            {
                if (file.FileSystem != this)
                {
                    return file.FileSystem.ReadFileAsync(file);
                }

                string path = this.GetAbsolutePath(file);

                if (File.Exists(path))
                {
                    return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true);
                }
            }

            return Stream.Null;
        }

        public Stream WriteFileAsync(IVirtualFile file, int bufferSize = FileSystemConstants.BufferSize)
        {
            if (file != null)
            {
                if (file.FileSystem != this)
                {
                    return file.FileSystem.WriteFileAsync(file);
                }

                string path = this.GetAbsolutePath(file.RelativePath);

                if (File.Exists(path))
                {
                    return new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, bufferSize, FileOptions.Asynchronous);
                }
            }

            return Stream.Null;
        }

        public IVirtualFile RenameFile(IVirtualFile file, string name)
        {
            if (file != null && !string.IsNullOrWhiteSpace(name))
            {
                string sourcePath = file.AbsolutePath;

                if (File.Exists(sourcePath))
                {
                    string relativePath = Path.Combine(file.RelativePath.ReplaceLast(file.Name, string.Empty), name);
                    string destinationPath = file.FileSystem.GetAbsolutePath(relativePath);

                    File.Move(sourcePath, destinationPath);
                    return new DiskVirtualFile(file.FileSystem, relativePath, name);
                }
            }

            return null;
        }

        public IVirtualFolder RenameFolder(IVirtualFolder folder, string name)
        {
            if (folder != null && !string.IsNullOrWhiteSpace(name))
            {
                string sourcePath = folder.AbsolutePath;


                if (Directory.Exists(sourcePath))
                {
                    string relativePath = Path.Combine(folder.RelativePath.ReplaceLast(folder.Name, string.Empty), name);
                    string destinationPath = folder.FileSystem.GetAbsolutePath(relativePath);

                    Directory.Move(sourcePath, destinationPath);
                    return new DiskVirtualFolder(folder.FileSystem, relativePath, name);
                }
            }

            return null;
        }

        public IVirtualFile CreateFile(IVirtualFolder folder, string fileName, Stream content = null)
        {
            if (folder != null)
            {
                if (folder.FileSystem != this)
                {
                    return folder.FileSystem.CreateFile(folder, fileName, content);
                }

                if (this.FolderExists(folder) && !string.IsNullOrWhiteSpace(fileName))
                {
                    string relativePath = Path.Combine(folder.RelativePath, fileName);
                    string path = this.GetAbsolutePath(relativePath);


                    using (Stream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        if (content != null)
                        {
                            content.CopyTo(writeStream);
                        }
                    }

                    return new DiskVirtualFile(this, relativePath, fileName);
                }
            }

            return null;
        }

        public IVirtualFile CreateFile(IVirtualFolder folder, string fileName, string content = null)
        {
            if (folder != null)
            {
                if (folder.FileSystem != this)
                {
                    return folder.FileSystem.CreateFile(folder, fileName, content);
                }

                if (this.FolderExists(folder) && !string.IsNullOrWhiteSpace(fileName))
                {
                    string relativePath = Path.Combine(folder.RelativePath, fileName);
                    string path = this.GetAbsolutePath(relativePath);


                    File.WriteAllText(path, content);

                    return new DiskVirtualFile(this, relativePath, fileName);
                }
            }

            return null;
        }

        public IVirtualFolder CreateFolder(string name)
        {
            return this.CreateFolder(this.RootFolder, name);
        }

        public IReadOnlyList<IVirtualFolder> GetFolders()
        {
            return this.GetFolders(this.RootFolder);
        }

        public IReadOnlyList<IVirtualFile> GetFiles()
        {
            return this.GetFiles(this.RootFolder);
        }

        public IVirtualFile GetFile(string relativePath)
        {
            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                string path = this.GetAbsolutePath(relativePath);

                FileInfo file = new FileInfo(path);

                if (file.Exists)
                {
                    var fileRelativePath = path.Remove(0, this.rootPath.Length - 1);
                    return new DiskVirtualFile(this, fileRelativePath, file.Name);
                }
            }

            return null;
        }

        public IVirtualFolder GetFolder(string relativePath)
        {
            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                string path = this.GetAbsolutePath(relativePath);

                DirectoryInfo directory = new DirectoryInfo(path);

                if (directory.Exists)
                {
                    return new DiskVirtualFolder(this, relativePath, directory.Name);
                }
            }

            return null;
        }

        public string GetAbsolutePath(IVirtualFileItem item)
        {
            item.ThrowIfNull("item");

            if (item.FileSystem != this)
            {
                return item.FileSystem.GetAbsolutePath(item);
            }

            return this.GetAbsolutePath(item.RelativePath);
        }

        public string GetWebUrl(IVirtualFileItem item)
        {
            item.ThrowIfNull("item");

            if (item.FileSystem != this)
            {
                return item.FileSystem.GetWebUrl(item);
            }

            if (!string.IsNullOrEmpty(this.RootWebUrl))
            {
                UrlBuilder builder = new UrlBuilder(this.RootWebUrl);

                var relativePath = item.RelativePath;
                var rootFolderPath = this.rootFolderName + "/";
                if (relativePath.StartsWith(rootFolderPath, StringComparison.OrdinalIgnoreCase) || relativePath.StartsWith("/" + rootFolderPath, StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = relativePath.ReplaceFirst(rootFolderPath, string.Empty);
                }

                builder.AppendUrl(relativePath);

                return builder.ToString();
            }

            return string.Empty;
        }

        public IVirtualFile CreateFile(string fileName, Stream content = null)
        {
            return this.CreateFile(this.RootFolder, fileName, content);
        }

        public IVirtualFile CreateTempFile(string fileName, Stream content = null)
        {
            return this.CreateFile(this.TempFolder, fileName, content);
        }

        public IVirtualFile CreateFile(string fileName, string content = null)
        {
            return this.CreateFile(this.RootFolder, fileName, content);
        }

        public IVirtualFile CreateTempFile(string fileName, string content = null)
        {
            return this.CreateFile(this.TempFolder, fileName, content);
        }

        public IVirtualFileSystem With(string name, Action<IVirtualFileSystem> newCallback, string rootWebUrl = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrWhiteSpace(rootWebUrl))
            {
                rootWebUrl = this.RootWebUrl;
            }

            string uniqueName = this.BuildUniqueName(name);

            if (!Container.Contains<IVirtualFileSystem>(uniqueName))
            {
                Container.Bind<IVirtualFileSystem>(uniqueName).ToMethod(() => new DiskStorageFileSystem(this.rootPath, name, newCallback, this.rootFolderName, rootWebUrl)).InSingletonScope();
            }

            return Container.Get<IVirtualFileSystem>(uniqueName);
        }

        private string BuildUniqueName(string name)
        {
            return "{0}:{1}".FormatString(this.rootPath, name);
        }

        public IVirtualFileSystem With(Guid id, Action<IVirtualFileSystem> newCallback, string rootWebUrl = null)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("id");
            }

            return this.With(id.ToStringValue(), newCallback, rootWebUrl);
        }

        public string GetAbsolutePath(string relativePath)
        {
            string storagePath = this.rootPath;

            if (!relativePath.StartsWith(this.RootFolder.RelativePath))
            {
                relativePath = Path.Combine(this.RootFolder.RelativePath, relativePath);
            }

            relativePath = relativePath.Replace('\\', '/');

            while (relativePath.StartsWith("/"))
            {
                relativePath = relativePath.Substring(1);
            }

            if (!string.IsNullOrEmpty(relativePath))
            {
                return Path.Combine(storagePath, relativePath).Replace("/", @"\");
            }

            return storagePath;
        }

        private void Initialize()
        {
            string storagePath = Path.Combine(this.rootPath, this.rootFolderName);
            DirectoryInfo directoryInfo = new DirectoryInfo(storagePath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            DirectoryInfo tempDirectoryInfo = new DirectoryInfo(Path.Combine(storagePath, FileSystemConstants.TempFolderName));
            if (!tempDirectoryInfo.Exists)
            {
                tempDirectoryInfo.Create();
            }

            this.RootFolder = new DiskVirtualFolder(this, "/" + this.rootFolderName, FileSystemConstants.StorageName);
            this.TempFolder = new DiskVirtualFolder(this, "/" + this.rootFolderName + "/" + FileSystemConstants.TempFolderName, FileSystemConstants.TempFolderName);

            if (!string.IsNullOrWhiteSpace(this.folderID))
            {
                storagePath = Path.Combine(storagePath, this.folderID);

                DirectoryInfo folderDirectoryInfo = new DirectoryInfo(storagePath);

                if (!folderDirectoryInfo.Exists)
                {
                    folderDirectoryInfo.Create();
                }

                this.RootFolder = new DiskVirtualFolder(this, "/" + this.rootFolderName + "/" + this.folderID, FileSystemConstants.StorageName);
            }

            if (this.callback != null)
            {
                this.callback(this);
            }

        }
    }
}
