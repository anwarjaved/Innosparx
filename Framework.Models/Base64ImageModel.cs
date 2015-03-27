namespace Framework.Models
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using Newtonsoft.Json;

    public class Base64ImageModel
    {
        public string Data { get; set; }

        [JsonIgnore]
        public ImageFormat Type
        {
            get
            {
                string[] data = this.Data.Split(new[] { "base64," }, StringSplitOptions.RemoveEmptyEntries);

                switch (data[0])
                {
                    case "data:image/png":
                        return ImageFormat.Png;
                    default:
                        return ImageFormat.Jpeg;
                }
            }
        }

        public string FileName { get; set; }
        public string LargeFileName { get; set; }

        public static implicit operator Image(Base64ImageModel m)
        {
            if (!string.IsNullOrWhiteSpace(m.Data))
            {
                string[] data = m.Data.Split(new[] { ";base64," }, StringSplitOptions.RemoveEmptyEntries);
                string imageBase64 = data[1];
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(imageBase64));
                ms.Seek(0, SeekOrigin.Begin);
                return Image.FromStream(ms);
            }

            return null;
        }

        public static implicit operator Stream(Base64ImageModel m)
        {
            if (!string.IsNullOrWhiteSpace(m.Data))
            {
                string[] data = m.Data.Split(new[] { ";base64," }, StringSplitOptions.RemoveEmptyEntries);
                string imageBase64 = data[1];
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(imageBase64));
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }

            return null;
        }

        public static implicit operator Base64ImageModel(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                Base64ImageModel m = new Base64ImageModel();
                m.Data = value;
                return m;
            }

            return null;
        }
    }
}
