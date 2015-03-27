namespace Framework
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Common HTTP headers.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class HttpHeaders
    {
        /// <summary>
        /// The Authorization header, which specifies the credentials that the client presents in order to authenticate itself to the server.
        /// </summary>
        public const string Authorization = "Authorization";

        /// <summary>
        /// The Content-Type header, which specifies the MIME type of the accompanying body data.
        /// </summary>
        public const string ContentType = "Content-Type";

        /// <summary>
        /// The Content-Length header, which specifies the length, in bytes, of the accompanying body data.
        /// </summary>
        public const string ContentLength = "Content-Length";

        /// <summary>
        /// The Content-MD5 header, which specifies the MD5 digest of the accompanying body data, for the purpose of providing an end-to-end message integrity check.
        /// </summary>
        public const string ContentMd5 = "Content-MD5";

        /// <summary>
        /// The User-Agent header, which specifies information about the client agent.
        /// </summary>
        public const string UserAgent = "User-Agent";

        /// <summary>
        /// The Host header, which specifies the host name and port number of the resource being requested.
        /// </summary>
        public const string Host = "Host";


        /// <summary>
        /// The Date header, which specifies the date and time at which the request originated.
        /// </summary>
        public const string Date = "Date";

        /// <summary>
        /// The Accept header, which specifies the MIME types that are acceptable for the response.
        /// </summary>
        public const string Accept = "Accept";

        /// <summary>
        /// The Connection header, which specifies options that are desired for a particular connection.
        /// </summary>
        public const string Connection = "Connection";
        /// <summary>
        /// The Expect header, which specifies particular server behaviors that are required by the client.
        /// </summary>
        public const string Expect = "Expect";

        /// <summary>
        /// The Content-Encoding header, which specifies the encodings that have been applied to the accompanying body data.
        /// </summary>
        public const string ContentEncoding = "Content-Encoding";

        /// <summary>
        /// The Accept-Encoding header, which specifies the content encodings that are acceptable for the response.
        /// </summary>
        public const string AcceptEncoding = "Accept-Encoding";

        /// <summary>
        /// The Cache-Control header, which specifies directives that must be obeyed by all cache control mechanisms along the request/response chain.
        /// </summary>
        public const string CacheControl = "Cache-Control";

        /// <summary>
        /// The If-Modified-Since header, which specifies that the requested operation should be performed only if the requested resource has been modified since the indicated data and time.
        /// </summary>
        public const string IfModifiedSince = "If-Modified-Since";

        /// <summary>
        /// The If-Unmodified-Since header, which specifies that the requested operation should be performed only if the requested resource has not been modified since the indicated date and time.
        /// </summary>
        public const string IfUnmodifiedSince = "If-Unmodified-Since";

        /// <summary>
        /// The If-Match header, which specifies that the requested operation should be performed only if the client's cached copy of the indicated resource is current.
        /// </summary>
        public const string IfMatch = "If-Match";

        /// <summary>
        /// The If-None-Match header, which specifies that the requested operation should be performed only if none of client's cached copies of the indicated resources are current.
        /// </summary>
        public const string IfNoneMatch = "If-None-Match";

        /// <summary>
        /// The Etag header, which specifies the current value for the requested variant.
        /// </summary>
        public const string ETag = "ETag";

        /// <summary>
        /// The Location header, which specifies a URI to which the client is redirected to obtain the requested resource.
        /// </summary>
        public const string Location = "Location";

        /// <summary>
        /// Header field has been proposed as a means for the origin server to suggest a default filename if the user requests that the content is saved to a file. 
        /// </summary>
        public const string ContentDisposition = "Content-Disposition";

        /// <summary>
        /// The Expires header, which specifies the date and time after which the accompanying body data should be considered stale.
        /// </summary>
        public const string Expires = "Expires";

        /// <summary>
        /// The Last-Modified header, which specifies the date and time at which the accompanying body data was last modified.
        /// </summary>
        public const string LastModified = "Last-Modified";

        /// <summary>
        /// The Accept-Ranges header, which specifies the range that is accepted by the server.
        /// </summary>
        public const string AcceptRanges = "Accept-Ranges";

    }
}
