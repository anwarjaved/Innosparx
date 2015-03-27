namespace Framework.Rest
{
    using System.IO;

    /// <summary>
    /// Container for files to be uploaded with requests
    /// </summary>
    public sealed class FileParameter
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the FileParameter class.
        /// </summary>
        ///
        /// <param name="fileName">
        ///     Name of the file to use when uploading.
        /// </param>
        /// <param name="rawData">
        ///     Information describing the raw.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public FileParameter(string fileName, Stream rawData)
            : this(string.Empty, fileName, MimeMapping.GetMimeMapping(fileName), rawData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public FileParameter(string name, string fileName, Stream rawData)
            : this(name, fileName, MimeMapping.GetMimeMapping(fileName), rawData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public FileParameter(string name, string fileName, string contentType, Stream rawData)
        {
            this.Name = name;
            this.FileName = fileName;
            this.ContentType = contentType;
            this.Data = rawData;
            ContentType = contentType;
            ContentLength = Data.Length;

        }

        /// <summary>
        /// The length of data to be sent
        /// </summary>
        public long ContentLength { get; private set; }
    
        /// <summary>
        /// Provides raw data for file
        /// </summary>
        public Stream Data { get; private set; }

        /// <summary>
        /// Name of the file to use when uploading
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// MIME content type of file
        /// </summary>
        public string ContentType { get; private set; }
        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string Name { get; private set; }
    }
}
