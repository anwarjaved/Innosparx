namespace Framework.Models
{
    public class ImageFileDataModel : FileDataModel
    {
        public ImageFileDataModel(FileDataModel model)
            : this(model.FileName, model.FilePath, model.WebUrl)
        {

        }

        public ImageFileDataModel(string fileName, string filePath, string webUrl)
            : base(fileName, filePath, webUrl)
        {
        }

        public ImageFileDataModel()
        {
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
