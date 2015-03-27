namespace Framework.Models
{
    /// <summary>Represents a multipart file data.</summary>
    public class FileDataModel
    {
        public FileDataModel()
        {
        }

        public FileDataModel(string fileName, string filePath, string webUrl)
        {
            this.FileName = fileName;
            this.FilePath = filePath;
            this.WebUrl = webUrl;
        }

        /// <summary>Gets or sets the name of the local file for the multipart file data.</summary>
        /// <returns>The name of the local file for the multipart file data.</returns>
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string WebUrl { get; set; }

    }
}
