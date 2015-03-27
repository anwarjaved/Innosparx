namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Represents List of mime mappings.
    /// </summary>
    public static class MimeMapping
    {
        private static readonly Dictionary<string, MappingInfo> ExtensionToMimeMappingTable = LoadMappings();

        private static Dictionary<string, MappingInfo> LoadMappings()
        {
            Dictionary<string, MappingInfo> mappings = new Dictionary<string, MappingInfo>(StringComparer.OrdinalIgnoreCase);

            using (Stream stream = typeof(MimeMapping).Assembly.GetManifestResourceStream("Framework.Resources.mimetypes.xml"))
            {
                XDocument document = XDocument.Load(stream);
                foreach (var element in document.Root.Elements("mimeType"))
                {
                    var description = element.Attribute("description") != null ? element.Attribute("description").Value : string.Empty;
                    mappings.Add(element.Attribute("extension").Value, new MappingInfo(element.Attribute("extension").Value, element.Attribute("type").Value, description));
                }
            }

            return mappings;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets all mappings.
        /// </summary>
        ///
        /// <value>
        ///     all mappings.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public static IReadOnlyList<MappingInfo> All
        {
            get
            {
                return ExtensionToMimeMappingTable.Values.ToList();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets mime mapping.
        /// </summary>
        ///
        /// <param name="fileName">
        ///     Filename of the file.
        /// </param>
        ///
        /// <returns>
        ///     The mime mapping.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string GetMimeMapping(string fileName)
        {
            int startIndex = fileName.LastIndexOf('.');
            if ((0 < startIndex) && (startIndex > fileName.LastIndexOf('\\')))
            {
                if (ExtensionToMimeMappingTable.ContainsKey(fileName.Substring(startIndex)))
                {
                    return ExtensionToMimeMappingTable[fileName.Substring(startIndex)].Text;
                }
            }

            return ExtensionToMimeMappingTable[".*"].Text;
        }
    }


}
