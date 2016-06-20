namespace Framework.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;


    /// <summary>
    /// Rest Client Service.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/19/2011 10:15 PM</datetime>
    public partial class RestClient : IRestClient
    {
        /// <summary>
        /// Occurs when an asynchronous upload operation successfully transfers some or all of the data.
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> UploadProgressChanged;

        /// <summary>
        /// Occurs when an asynchronous download operation successfully transfers some or all of the data.
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> DownloadProgressChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 2:20 PM</datetime>
        public RestClient()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        private void InvokeUploadProgressChanged(ProgressChangedEventArgs e)
        {
            EventHandler<ProgressChangedEventArgs> handler = this.UploadProgressChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void InvokeDownloadProgressChanged(ProgressChangedEventArgs e)
        {
            EventHandler<ProgressChangedEventArgs> handler = this.DownloadProgressChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth.
        /// </summary>
        /// <param name="value">The value to Url encode.</param>
        /// <returns>Returns a Url encoded string.</returns>
        protected virtual string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        internal static ResponseMode GetResponseMode(string contentType)
        {
            if (!string.IsNullOrEmpty(contentType))
            {
                int semicolonIndex = contentType.IndexOf(';');
                if (semicolonIndex > -1)
                {
                    contentType = contentType.Substring(0, semicolonIndex);
                }

                if (contentType.EqualsIgnoreCase("application/json") ||
                  contentType.EqualsIgnoreCase("text/json") ||
                  contentType.EqualsIgnoreCase("text/javascript") ||
                  contentType.EqualsIgnoreCase("text/x-json"))
                {
                    return ResponseMode.Json;
                }

                if (contentType.EqualsIgnoreCase("text/xml"))
                {
                    return ResponseMode.Xml;
                }
            }

            return ResponseMode.None;
        }
    }
}
