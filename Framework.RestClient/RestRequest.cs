namespace Framework.Rest
{
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Text.RegularExpressions;

    using Framework.Ioc;
    using Framework.Serialization;
    using Framework.Serialization.Json;
    using Framework.Serialization.Xml;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Web Client Configuration.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public partial class RestRequest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The multipart boundary.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        private const string MultipartBoundary = "----------------------------400f182a9360";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The multipart boundary footer.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        private const string MultipartBoundaryFooter = "\r\n--" + MultipartBoundary + "--\r\n";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The post stream.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        private readonly MemoryStream postStream = new MemoryStream();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The empty JSON request.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        private readonly byte[] EmptyJsonRequest;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The query string regular expression.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        private readonly Regex QueryStringRegex = new Regex("(?<key>[^&?=]+)(?(?==)=(?<value>[^&?=]+))", RegexOptions.Compiled);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Occurs when an asynchronous upload operation successfully transfers some or all of the
        ///     data.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        internal event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="requestMode">
        ///     The request mode.
        /// </param>
        /// <param name="acceptMode">
        ///     Type of the accept.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(Uri resource, RequestMode requestMode, AcceptMode acceptMode, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            this.Cookies = new CookieCollection();
            this.Headers = new WebHeaderCollection();
            this.Timeout = 30;
            this.ClientCertificates = new X509CertificateCollection();
            this.BufferSize = 8192;
            this.UserAgent = RestConstants.FrameworkVersion;
            this.RequestMode = requestMode;
            this.Encoding = encoding;
            this.Resource = resource;

            this.Parameters = new QueryParameterCollection();
            if (!string.IsNullOrEmpty(this.Resource.Query))
            {
                MatchCollection matches = QueryStringRegex.Matches(this.Resource.Query);
                foreach (Match item in matches)
                {
                    string key = item.Groups["key"].Value;
                    string value = item.Groups["value"].Value;
                    this.Parameters.Add(new QueryParameter(HttpUtility.UrlDecode(key), HttpUtility.UrlDecode(value)));
                }

                this.Resource = new Uri(this.Resource.AbsoluteUri.Replace(this.Resource.Query, string.Empty));
            }

            this.EmptyJsonRequest = this.Encoding.GetBytes("{}");
            this.AcceptMode = acceptMode;
            this.AllowWriteStreamBuffering = true;

            switch (this.RequestMode)
            {
                case RequestMode.Json:
                    this.ContentType = "application/json; charset=utf-8";
                    break;

                case RequestMode.UrlEncoded:
                    this.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                    break;

                case RequestMode.MultiPart:
                    this.ContentType = "multipart/form-data; boundary=" + MultipartBoundary;
                    break;

                case RequestMode.Xml:
                    this.ContentType = "text/xml; charset=utf-8";
                    break;

                case RequestMode.Raw:
                    this.ContentType = "application/octet-stream";
                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="requestMode">
        ///     The request mode.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(Uri resource, RequestMode requestMode, Encoding encoding)
            : this(resource, requestMode, AcceptMode.None, encoding)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="requestMode">
        ///     The request mode.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(Uri resource, RequestMode requestMode = RequestMode.UrlEncoded)
            : this(resource, requestMode, Encoding.UTF8)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="requestMode">
        ///     The request mode.
        /// </param>
        /// <param name="acceptMode">
        ///     Type of the accept.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(Uri resource, RequestMode requestMode, AcceptMode acceptMode)
            : this(resource, requestMode, acceptMode, Encoding.UTF8)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="requestMode">
        ///     The request mode.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(string resource,RequestMode requestMode, Encoding encoding)
            : this(new Uri(resource), requestMode, encoding)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="requestMode">
        ///     The request mode.
        /// </param>
        /// <param name="acceptMode">
        ///     Type of the accept.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(string resource, RequestMode requestMode, AcceptMode acceptMode, Encoding encoding)
            : this(new Uri(resource), requestMode, acceptMode, encoding)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="requestMode">
        ///     The request mode.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(string resource, RequestMode requestMode)
            : this(new Uri(resource), requestMode)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="requestMode">
        ///     The request mode.
        /// </param>
        /// <param name="acceptMode">
        ///     Type of the accept.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(string resource, RequestMode requestMode, AcceptMode acceptMode)
            : this(new Uri(resource), requestMode, acceptMode)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="acceptMode">
        ///     Type of the accept.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(string resource, AcceptMode acceptMode)
            : this(new Uri(resource), RequestMode.UrlEncoded, acceptMode)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(string resource)
            : this(new Uri(resource), RequestMode.UrlEncoded, Encoding.ASCII)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(string resource, Encoding encoding)
            : this(new Uri(resource), RequestMode.UrlEncoded, encoding)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="acceptMode">
        ///     Type of the accept.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(string resource, AcceptMode acceptMode, Encoding encoding)
            : this(new Uri(resource), RequestMode.UrlEncoded, acceptMode, encoding)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="acceptMode">
        ///     Type of the accept.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(Uri resource, AcceptMode acceptMode)
            : this(resource, RequestMode.UrlEncoded, acceptMode)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(Uri resource, Encoding encoding)
            : this(resource, RequestMode.UrlEncoded, encoding)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestRequest"/> class.
        /// </summary>
        ///
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <param name="acceptMode">
        ///     Type of the accept.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestRequest(Uri resource, AcceptMode acceptMode, Encoding encoding)
            : this(resource, RequestMode.UrlEncoded, acceptMode, encoding)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the User name used for Authentication. To use the currently logged in user
        ///     when accessing an NTLM resource you can use "AUTOLOGIN".
        /// </summary>
        ///
        /// <value>
        ///     The name of the user.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string UserName
        {
            get;
            set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Password for Authentication.
        /// </summary>
        ///
        /// <value>
        ///     The password.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Password
        {
            get;
            set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the type of the content.
        /// </summary>
        ///
        /// <value>
        ///     The type of the content.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ContentType { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets The Content-MD5 header, which specifies the MD5 digest of the accompanying
        ///     body data, for the purpose of providing an end-to-end message integrity check.
        /// </summary>
        ///
        /// <value>
        ///     The Content-MD5 header, which specifies the MD5 digest of the accompanying body data, for
        ///     the purpose of providing an end-to-end message integrity check.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        internal string ContentMD5 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets The Content-MD5 header, which specifies the MD5 digest of the accompanying
        ///     body data, for the purpose of providing an end-to-end message integrity check.
        /// </summary>
        ///
        /// <value>
        ///     The Content-MD5 header, which specifies the MD5 digest of the accompanying body data, for
        ///     the purpose of providing an end-to-end message integrity check.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool EnableContentMD5 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the content disposition.
        /// </summary>
        ///
        /// <value>
        ///     The content disposition.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ContentDisposition { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the content encoding.
        /// </summary>
        ///
        /// <value>
        ///     The content encoding.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ContentEncoding { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets: Return the object only if it has been modified since the specified time,
        ///     otherwise return a 304 (not modified).
        /// </summary>
        ///
        /// <value>
        ///     if modified since.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? IfModifiedSince { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets: Return the object only if it has not been modified since the specified time,
        ///     otherwise return a 412 (precondition failed).
        /// </summary>
        ///
        /// <value>
        ///     if unmodified since.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? IfUnmodifiedSince { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets: Return the object only if its entity tag (ETag) is the same as the one
        ///     specified, otherwise return a 412 (precondition failed).
        /// </summary>
        ///
        /// <value>
        ///     if match.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? IfMatch { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets: Return the object only if its entity tag (ETag) is the different from the
        ///     one specified, otherwise return a 304 (precondition failed).
        /// </summary>
        ///
        /// <value>
        ///     if none match.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? IfNoneMatch { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets Number of milliseconds before expiration.
        /// </summary>
        ///
        /// <value>
        ///     The Number of milliseconds before expiration.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public TimeSpan? Expires { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the cache control.
        /// </summary>
        ///
        /// <value>
        ///     The cache control.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string CacheControl { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the resource.
        /// </summary>
        ///
        /// <value>
        ///     The resource.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public Uri Resource
        {
            get;
            internal set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether the pre authenticate.
        /// </summary>
        ///
        /// <value>
        ///     true if pre authenticate, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool PreAuthenticate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the encoding.
        /// </summary>
        ///
        /// <value>
        ///     The encoding.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public Encoding Encoding { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Address of the Proxy Server to be used. Use optional DEFAULTPROXY value
        ///     to specify that you want to IE Proxy Settings.
        /// </summary>
        ///
        /// <value>
        ///     The proxy address.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ProxyAddress
        {
            get;
            set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Semicolon separated Address list of the servers the proxy is not used
        ///     for.
        /// </summary>
        ///
        /// <value>
        ///     The proxy bypass address list.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ProxyBypass
        {
            get;
            set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the User Name for a password validating Proxy. Only used if the proxy info
        ///     is set.
        /// </summary>
        ///
        /// <value>
        ///     The name of the proxy user.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ProxyUserName
        {
            get;
            set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value that indicates whether to buffer the data sent to the Internet
        ///     resource.
        /// </summary>
        ///
        /// <value>
        ///     <c>true</c> if [allow write stream buffering]; otherwise, <c>false</c>.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool AllowWriteStreamBuffering { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Password for a password validating Proxy. Only used if the proxy info is
        ///     set.
        /// </summary>
        ///
        /// <value>
        ///     The proxy password.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ProxyPassword
        {
            get;
            set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Timeout for the Web request in seconds. Times out on connection, read
        ///     and send operations. Default is 30 seconds.
        /// </summary>
        ///
        /// <value>
        ///     The timeout.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int Timeout
        {
            get;
            set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets Headers sent to server with the request.
        /// </summary>
        ///
        /// <value>
        ///     The headers.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public WebHeaderCollection Headers
        {
            get;
            private set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets what HTTP method to use for this request. Supported methods: GET, POST, PUT,
        ///     DELETE, HEAD, OPTIONS Default is GET.
        /// </summary>
        ///
        /// <value>
        ///     The method.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        internal MethodType Method
        {
            get; set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the accept type.
        /// </summary>
        ///
        /// <value>
        ///     The accept type.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public AcceptMode AcceptMode
        {
            get;
            private set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets what content type to use for this request.
        /// </summary>
        ///
        /// <value>
        ///     The content type.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public RequestMode RequestMode
        {
            get;
            private set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the internal Cookie collection before or after a request.
        /// </summary>
        ///
        /// <value>
        ///     The cookies.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public CookieCollection Cookies
        {
            get;
            private set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the buffer size used for the Send and Receive operations.
        /// </summary>
        ///
        /// <value>
        ///     The size of the buffer.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int BufferSize
        {
            get;
            set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the User Agent  browser string that is sent to the server.
        /// </summary>
        ///
        /// <value>
        ///     The user agent.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string UserAgent
        {
            get;
            private set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the collection of security certificates that are associated with this request.
        /// </summary>
        ///
        /// <value>
        ///     The client certificates.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public X509CertificateCollection ClientCertificates
        {
            get;
            private set;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the parameters collection for querystring.
        /// </summary>
        ///
        /// <value>
        ///     The parameters.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public QueryParameterCollection Parameters { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the request URL.
        /// </summary>
        ///
        /// <value>
        ///     The request URL.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public Uri RequestUrl
        {
            get
            {
                UrlBuilder requestParametersBuilder = new UrlBuilder(this.Resource.AbsoluteUri);

                foreach (QueryParameter item in this.Parameters)
                {
                    requestParametersBuilder.QueryString.Add(item.Name, string.IsNullOrEmpty(item.Value) ? "" : item.Value);
                }

                return new Uri(requestParametersBuilder.ToString(this.UrlEncode));
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds POST form variables to the request buffer.
        ///     <see cref="RequestMode"/> determines how parms are handled.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="key">
        ///     Key value or raw buffer depending on post type.
        /// </param>
        /// <param name="value">
        ///     Value to store. Used only in key/value pair modes.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public virtual void AddBody(string key, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            switch (this.RequestMode)
            {
                case RequestMode.UrlEncoded:
                    if (string.IsNullOrEmpty(key))
                    {
                        throw new ArgumentNullException("key");
                    }

                    this.Write(this.Encoding.GetBytes(this.UrlEncode(key) + "=" + this.UrlEncode(value) + "&"));
                    break;

                case RequestMode.MultiPart:
                    if (string.IsNullOrEmpty(key))
                    {
                        throw new ArgumentNullException("key");
                    }

                    this.Write(this.Encoding.GetBytes("--" + MultipartBoundary + "\r\n" +
                                           "Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n"));

                    this.Write(this.Encoding.GetBytes(value));

                    this.Write(this.Encoding.GetBytes("\r\n\r\n"));
                    break;
                default: // Raw or Xml modes
                    this.Write(value);
                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds POST form variables to the request buffer.
        ///     <see cref="RequestMode"/> determines how parms are handled.
        /// </summary>
        ///
        /// <param name="values">
        ///     Value to store. Used only in key/value pair modes.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void AddBody(byte[] values)
        {
            this.Write(values);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds the file.
        /// </summary>
        ///
        /// <param name="fileParameter">
        ///     The file parameter.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void AddFile(FileParameter fileParameter)
        {
            if (this.Method != MethodType.Get && this.Method != MethodType.Head)
            {
                byte[] buffer = new Byte[RestConstants.BufferSize];
                if (this.RequestMode == RequestMode.MultiPart)
                {
                    if (fileParameter != null && fileParameter.ContentLength > 0)
                    {
                        this.Write(
                            this.Encoding.GetBytes(
                                "--" + MultipartBoundary + "\r\n" + "Content-Disposition: file; name=\""
                                + fileParameter.Name + "\"; filename=\"" + fileParameter.FileName + "\"\r\n"));

                        this.Write(
                            this.Encoding.GetBytes(
                                HttpHeaders.ContentType + ": " + fileParameter.ContentType + "\r\n\r\n"));

                        using (BinaryReader br = new BinaryReader(fileParameter.Data))
                        {
                            int bytesRead;
                            while ((bytesRead = br.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                this.Write(buffer, 0, bytesRead);
                            }
                        }

                        this.Write(this.Encoding.GetBytes("\r\n"));
                    }
                }
                else
                {
                    //Raw File
                    using (BinaryReader br = new BinaryReader(fileParameter.Data))
                    {
                        int bytesRead;
                        while ((bytesRead = br.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            this.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }

      /*  internal void GenerateContentMD5Hash()
        {
            if (this.Method != MethodType.Get && this.Method != MethodType.Head)
            {
                byte[] data = this.postStream.ToArray();
                long length = data.Length;
                switch (this.RequestMode)
                {
                    case RequestMode.Json:
                        if (length == 0)
                        {
                            data = this.EmptyJsonRequest;
                            length = data.Length;
                        }

                        break;

                    case RequestMode.UrlEncoded:
                        if (length > 0)
                        {
                            length = length - 1;

                            byte[] data2 = new byte[length];
                            Buffer.BlockCopy(data, 0, data2, 0, (int)length);
                            data = data2;
                        }

                        break;

                    case RequestMode.MultiPart:
                        byte[] footer = this.Encoding.GetBytes(MultipartBoundaryFooter);

                        byte[] data3 = new byte[length + footer.Length];
                        Buffer.BlockCopy(data, 0, data3, 0, (int)length);
                        Buffer.BlockCopy(footer, 0, data3, (int)length, footer.Length);

                        data = data3;
                        length = length + footer.Length;

                        break;
                }

                if (length > 0)
                {
                    this.ContentMD5 = Convert.ToBase64String(Cryptography.CreateHash(HashMode.MD5, data));
                }
            }
        }*/

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds the body to request.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Type of object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to add.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void AddBody<T>(T obj) where T : class
        {
            if (typeof(ISerializable).IsAssignableFrom(typeof(T)))
            {
                string value = ((ISerializable)obj).Serialize();
                if (!string.IsNullOrEmpty(value))
                {
                    this.Write(value);
                }
            }
            else
            {
                if (this.RequestMode == RequestMode.Json && this.postStream.Length == 0)
                {
                    IJsonSerializer jsonSerializer = Container.Get<IJsonSerializer>();
                    string value = jsonSerializer.Serialize(obj);
                    if (!string.IsNullOrEmpty(value))
                    {
                        this.Write(value);
                    }
                }

                if (this.RequestMode == RequestMode.Xml && this.postStream.Length == 0)
                {
                    IXmlSerializer xmlSerializer = Container.Get<IXmlSerializer>();

                    string value = xmlSerializer.Serialize(obj);

                    if (!string.IsNullOrEmpty(value))
                    {
                        this.Write(value);
                    }
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds POST form variables to the request buffer.
        ///     <see cref="RequestMode"/> determines how parms are handled.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="key">
        ///     Key value or raw buffer depending on post type.
        /// </param>
        /// <param name="values">
        ///     Value to store. Used only in key/value pair modes.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void AddBody(string key, byte[] values)
        {
            if (values == null)
            {
                values = new byte[0];
            }

            switch (this.RequestMode)
            {
                case RequestMode.UrlEncoded:
                    if (string.IsNullOrEmpty(key))
                    {
                        throw new ArgumentNullException("key");
                    }

                    this.Write(this.Encoding.GetBytes(this.UrlEncode(key) + "="));
                    this.Write(values);
                    this.Write("&");

                    break;
                case RequestMode.MultiPart:
                    if (string.IsNullOrEmpty(key))
                    {
                        throw new ArgumentNullException("key");
                    }

                    this.Write(this.Encoding.GetBytes("--" + MultipartBoundary + "\r\n" +
                                           "Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n"));

                    this.Write(values);

                    this.Write(this.Encoding.GetBytes("\r\n"));
                    break;

                default: // Raw or Xml modes
                    this.Write(values);
                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the progress changed on a different thread, and waits for the result.
        /// </summary>
        ///
        /// <param name="e">
        ///     Progress changed event information.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void InvokeProgressChanged(ProgressChangedEventArgs e)
        {
            EventHandler<ProgressChangedEventArgs> handler = this.ProgressChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the given raw bytes.
        /// </summary>
        ///
        /// <param name="rawBytes">
        ///     The raw bytes to write.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void Write(byte[] rawBytes)
        {
            if (rawBytes != null && rawBytes.Length > 0)
            {
                this.postStream.Write(rawBytes, 0, rawBytes.Length);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the given raw bytes.
        /// </summary>
        ///
        /// <param name="rawBytes">
        ///     The raw bytes to write.
        /// </param>
        /// <param name="offset">
        ///     The offset.
        /// </param>
        /// <param name="bytesRead">
        ///     The bytes read.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void Write(byte[] rawBytes, int offset, int bytesRead)
        {
            if (rawBytes != null && rawBytes.Length > 0 && bytesRead > 0)
            {
                this.postStream.Write(rawBytes, offset, bytesRead);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the given raw bytes.
        /// </summary>
        ///
        /// <param name="data">
        ///     The data to write.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void Write(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                byte[] rawBytes = this.Encoding.GetBytes(data);
                this.postStream.Write(rawBytes, 0, rawBytes.Length);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Handles the accept type described by webRequest.
        /// </summary>
        ///
        /// <param name="webRequest">
        ///     The web request.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void HandleAcceptType(HttpWebRequest webRequest)
        {
            switch (this.AcceptMode)
            {
                case AcceptMode.Json:
                    webRequest.Accept = "application/json; charset=utf-8";
                    break;
                case AcceptMode.Xml:
                    webRequest.Accept = "text/xml; charset=utf-8";
                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds the headers.
        /// </summary>
        ///
        /// <exception cref="NotSupportedException">
        ///     Thrown when the requested operation is not supported.
        /// </exception>
        ///
        /// <param name="request">
        ///     The request.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void AddHeaders(HttpWebRequest request)
        {
            if (!string.IsNullOrEmpty(this.ContentMD5))
            {
                this.Headers[HttpHeaders.ContentMd5] = this.ContentMD5;
            }

            if (!string.IsNullOrEmpty(this.CacheControl))
            {
                this.Headers[HttpHeaders.CacheControl] = this.CacheControl;
            }

            if (!string.IsNullOrEmpty(this.ContentDisposition))
            {
                this.Headers[HttpHeaders.ContentDisposition] = this.ContentDisposition;
            }

            if (!string.IsNullOrEmpty(this.ContentEncoding))
            {
                this.Headers[HttpHeaders.ContentEncoding] = this.ContentEncoding;
            }

            if (this.Expires.HasValue)
            {
                this.Headers[HttpHeaders.Expires] = W3CDateTime.UtcNow.ToDate().Add(this.Expires.Value).ToString("X");
            }

            if (this.IfMatch.HasValue)
            {
                this.Headers[HttpHeaders.IfMatch] = new W3CDateTime(this.IfMatch.Value).ToString("X");
            }

            if (this.IfNoneMatch.HasValue)
            {
                this.Headers[HttpHeaders.IfNoneMatch] = new W3CDateTime(this.IfNoneMatch.Value).ToString("X");
            }

            if (this.IfModifiedSince.HasValue)
            {
                this.Headers[HttpHeaders.IfModifiedSince] = new W3CDateTime(this.IfModifiedSince.Value).ToString("X");
            }

            if (this.IfUnmodifiedSince.HasValue)
            {
                this.Headers[HttpHeaders.IfUnmodifiedSince] = new W3CDateTime(this.IfUnmodifiedSince.Value).ToString("X");
            }

            foreach (string header in this.Headers)
            {
                string headerValue = this.Headers[header];
                if (WebHeaderCollection.IsRestricted(header))
                {
                    if (string.Equals(header, HttpHeaders.Accept, StringComparison.OrdinalIgnoreCase))
                        request.Accept = headerValue;
                    else if (string.Equals(header, HttpHeaders.Connection, StringComparison.OrdinalIgnoreCase))
                        request.Connection = headerValue;
                    else if (string.Equals(header, HttpHeaders.ContentType, StringComparison.OrdinalIgnoreCase))
                        request.ContentType = headerValue;
                    else if (string.Equals(header, HttpHeaders.Expect, StringComparison.OrdinalIgnoreCase))
                        request.Expect = headerValue;
                    else if (string.Equals(header, HttpHeaders.UserAgent, StringComparison.OrdinalIgnoreCase))
                        request.UserAgent = headerValue;
                    // Date accessor is only present in .NET 4.0, so using reflection
                    else if (string.Equals(header, HttpHeaders.Date, StringComparison.OrdinalIgnoreCase))
                        request.Date = DateTime.Parse(headerValue);
                    // Host accessor is only present in .NET 4.0, so using reflection
                    else if (string.Equals(header, HttpHeaders.Host, StringComparison.OrdinalIgnoreCase))
                        request.Host = headerValue;
                    else
                        throw new NotSupportedException("Header with name " + header + " is not supported");

                    /*
                    // Content-Length is not supported because it is one of the headers known AFTER signing
                    else if (string.Equals(kvp.Key, "Content-Length", StringComparison.OrdinalIgnoreCase))
                        throw new NotSupportedException();
                    // If-Modified-Since is not supported because the required parsing methods are internal
                    else if (string.Equals(kvp.Key, "If-Modified-Since", StringComparison.OrdinalIgnoreCase))
                        throw new NotSupportedException();
                    // Range is not supported for SDK requests
                    else if (string.Equals(kvp.Key, "Range", StringComparison.OrdinalIgnoreCase))
                        throw new NotSupportedException();
                    // Referer is not supported for SDK requests
                    else if (string.Equals(kvp.Key, "Referer", StringComparison.OrdinalIgnoreCase))
                        throw new NotSupportedException();
                    // Transfer-Encoding is not supported for SDK requests
                    else if (string.Equals(kvp.Key, "Transfer-Encoding", StringComparison.OrdinalIgnoreCase))
                        throw new NotSupportedException();
                    // Proxy-Connection is not supported, proxy must be set using config object
                    else if (string.Equals(kvp.Key, "Proxy-Connection", StringComparison.OrdinalIgnoreCase))
                        throw new NotSupportedException();
                     */
                }
                else
                {
                    request.Headers[header] = headerValue;
                }

            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Handles the request type described by webRequest.
        /// </summary>
        ///
        /// <param name="webRequest">
        ///     The web request.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void HandleRequestType(WebRequest webRequest)
        {
            if (this.Method != MethodType.Get && this.Method != MethodType.Head)
            {
                if (string.IsNullOrEmpty(this.ContentType))
                {
                    switch (this.RequestMode)
                    {
                        case RequestMode.Json:
                            this.ContentType = "application/json; charset=utf-8";
                            break;

                        case RequestMode.UrlEncoded:
                            this.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                            break;

                        case RequestMode.MultiPart:
                            this.ContentType = "multipart/form-data; boundary=" + MultipartBoundary;
                            //this.Write(this.Encoding.GetBytes("--" + MultipartBoundary + "\r\n"));
                            break;

                        case RequestMode.Xml:
                            this.ContentType = "text/xml; charset=utf-8";
                            break;

                        case RequestMode.Raw:
                            this.ContentType = "application/octet-stream";
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(this.ContentType))
                {
                    webRequest.ContentType = this.ContentType;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Handles the method type described by webRequest.
        /// </summary>
        ///
        /// <param name="webRequest">
        ///     The web request.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void HandleMethodType(HttpWebRequest webRequest)
        {
            switch (this.Method)
            {
                case MethodType.Get:
                    webRequest.Method = "GET";
                    break;
                case MethodType.Post:
                    webRequest.Method = "POST";
                    break;
                case MethodType.Put:
                    webRequest.Method = "PUT";
                    break;
                case MethodType.Delete:
                    webRequest.Method = "DELETE";
                    break;
                case MethodType.Head:
                    webRequest.Method = "HEAD";
                    break;
            }
        }


        private void BuildRequestLog(WebRequest webRequest, string data, ref StringBuilder sb)
        {
            UrlBuilder builder = new UrlBuilder(webRequest.RequestUri);

            sb.AppendLine("{0} {1}".FormatString(webRequest.Method, builder.ToString(this.UrlEncode)));
            sb.AppendLine("HTTP/1.1");
            foreach (var header in webRequest.Headers.AllKeys)
            {
                sb.AppendLine("{0} : {1}".FormatString(header, webRequest.Headers[header]));
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                sb.AppendLine(data);
            }

            sb.AppendLine();
        }
    }
}
