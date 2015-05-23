using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure;
using Framework.Models;

namespace Framework.Web.Api
{
    using Framework.FileSystem;

    
    public class FileSystemStreamProvider : MultipartStreamProvider
    {
        private readonly IVirtualFileSystem fileSystem;

        private readonly IVirtualFolder folderItem;

        private readonly int bufferSize;

        private readonly Collection<FileDataModel> files = new Collection<FileDataModel>();

        public FileSystemStreamProvider(IVirtualFileSystem fileSystem, IVirtualFolder folderItem, int bufferSize = 4096)
        {
            this.fileSystem = fileSystem;
            this.folderItem = folderItem;
            this.bufferSize = bufferSize;
        }

        public virtual string GetFileName(HttpContentHeaders headers)
        {
            string fileName = headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            return !string.IsNullOrWhiteSpace(fileName)
                       ? Guid.NewGuid().ToStringValue() + Path.GetExtension(fileName)
                       : Guid.NewGuid().ToStringValue() + ".data";
        }

        
        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            string fileName = this.GetFileName(headers);

            var fileItem = fileSystem.CreateFile(this.FolderItem, fileName, Stream.Null);
            this.Files.Add(new FileDataModel(fileItem.Name, fileItem.RelativePath, fileItem.WebUrl));

            return fileSystem.WriteFileAsync(fileItem, this.BufferSize);
        }

        /// <summary>Gets the FolderItem where the content of MIME multipart body parts are written to.</summary>
        /// <returns>The FolderItem where the content of MIME multipart body parts are written to.</returns>
        protected IVirtualFolder FolderItem
        {
            get
            {
                return this.folderItem;
            }
        }

        /// <summary>Gets the fileS.</summary>
        /// <returns>The file data.</returns>
        public ICollection<FileDataModel> Files
        {
            get
            {
                return this.files;
            }
        }





        /// <summary>Gets or sets the number of bytes buffered for writes to the file.</summary>
        /// <returns>The number of bytes buffered for writes to the file.</returns>
        protected int BufferSize
        {
            get
            {
                return this.bufferSize;
            }
        }
    }
}
