namespace Framework
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Url related function.
    /// </summary>
    public static class UrlPath
    {
        private static readonly char[] SlashChars = { '\\', '/' };

        /// <summary>
        /// Appends the literal slash mark (/) to the end of the path,
        ///  if one does not already exist.
        /// </summary>
        /// <returns>
        /// The modified path.
        /// </returns>
        /// <param name="path">
        /// The path to append the slash mark to.
        /// </param>
        public static string AppendTrailingSlash(string path)
        {
            if (path == null)
            {
                return null;
            }

            int length = path.Length;
            if ((length != 0) && (path[length - 1] != '/'))
            {
                path = path + '/';
            }

            return path;
        }

        public static string AppendLeadingSlash(string path)
        {
            if (path == null)
            {
                return null;
            }

            int length = path.Length;
            if ((length != 0) && (path[0] != '/'))
            {
                path = '/' + path;
            }

            return path;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Combines Two url.
        /// </summary>
        ///
        /// <param name="baseUrl">
        ///     URL of the base.
        /// </param>
        /// <param name="resource">
        ///     The resource.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string Combine(string baseUrl, string resource)
        {
            
            UrlBuilder urlBuilder = new UrlBuilder(baseUrl);

            urlBuilder.AppendUrl(resource);

            return urlBuilder.ToString();
        }

        /// <summary>
        /// Removes a trailing slash mark (/) from a path.
        /// </summary>
        /// <returns>
        /// A virtual path without a trailing slash mark, if the path is 
        /// not already the root directory ("/"); otherwise, null.
        /// </returns>
        /// <param name="path">
        /// The path to remove any trailing slash mark from. 
        /// </param>
        public static string RemoveTrailingSlash(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            int length = path.Length;
            if ((length > 1) && (path[length - 1] == '/'))
            {
                return path.Substring(0, length - 1);
            }

            return path;
        }
    }
}