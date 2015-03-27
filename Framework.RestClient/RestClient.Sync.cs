using System;

namespace Framework.Rest
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;

    using Framework.Logging;
    using Framework.Reflection;
    using Framework.Serialization;
    using Framework.Serialization.Json;
    using Framework.Serialization.Xml;

    using Newtonsoft.Json.Linq;

    using Container = Framework.Ioc.Container;

    partial class RestClient
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the given request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     A <see cref="RestResponse"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse Get(RestRequest request)
        {
            return Execute(request, MethodType.Get);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the given request.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     A <see cref="RestResponse{T}"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse<T> Get<T>(RestRequest request) where T : new()
        {
            return Execute<T>(request, MethodType.Get);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Post this message.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     A <see cref="RestResponse"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse Post(RestRequest request)
        {
            return Execute(request, MethodType.Post);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Post this message.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse<T> Post<T>(RestRequest request) where T : new()
        {
            return Execute<T>(request, MethodType.Post);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the given request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse Delete(RestRequest request)
        {
            return Execute(request, MethodType.Delete);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the given request.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse<T> Delete<T>(RestRequest request) where T : new()
        {
            return Execute<T>(request, MethodType.Delete);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Puts the given request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse Put(RestRequest request)
        {
            return Execute(request, MethodType.Put);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Puts the given request.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse<T> Put<T>(RestRequest request) where T : new()
        {
            return Execute<T>(request, MethodType.Put);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Heads the given request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse Head(RestRequest request)
        {
            return Execute(request, MethodType.Head);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Heads the given request.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to get.
        /// </param>
        ///
        /// <returns>
        ///     Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse<T> Head<T>(RestRequest request) where T : new()
        {
            return Execute<T>(request, MethodType.Head);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to get.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual RestResponse Execute(RestRequest request, MethodType method)
        {
            request.Method = method;
            StringBuilder sb = new StringBuilder();
            RestResponse response = new RestResponse { Request = request };
            try
            {
                response = GetResponse(request, sb);
            }
            catch (WebException exception)
            {
                response.Status = ResponseStatus.Error;
                response.ErrorMessage = exception.Message;
                response.ErrorException = new RestException(exception.Message, response.StatusCode, response.ErrorException);

                sb.AppendLine(exception.Message);

                try
                {
                    //if (exception.Status == WebExceptionStatus.ProtocolError)
                    {
                        if (exception.Response != null)
                        {
                            HttpWebResponse webResponse = (HttpWebResponse)exception.Response;
                            response.ContentEncoding = webResponse.ContentEncoding;
                            response.Server = webResponse.Server;
                            response.ContentType = webResponse.ContentType;
                            response.ContentLength = webResponse.ContentLength;

                            using (Stream responseStream = webResponse.GetResponseStream())
                            {
                                response.ContentArray = responseStream.ToArray();
                            }

                            response.ResponseMode = GetResponseMode(response.ContentType);

                            response.StatusCode = webResponse.StatusCode;

                            response.StatusDescription = webResponse.StatusDescription;
                         
                            sb.AppendLine("HTTP/1.1 {0} {1}".FormatString(((int)webResponse.StatusCode), webResponse.StatusDescription));

                            response.ResponseUri = webResponse.ResponseUri;

                            if (webResponse.Cookies != null)
                            {
                                foreach (Cookie cookie in webResponse.Cookies)
                                {
                                    response.Cookies.Add(cookie);
                                }
                            }

                            foreach (string headerName in webResponse.Headers.AllKeys)
                            {
                                string headerValue = webResponse.Headers[headerName];
                                response.Headers.Add(headerName, headerValue);
                                sb.AppendLine("{0} : {1}".FormatString(headerName, headerValue));
                            }
                            sb.AppendLine();

                            sb.AppendLine(response.Content);

                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Status = ResponseStatus.Error;
                    response.ErrorMessage = ex.Message;
                    response.ErrorException = new RestException(ex.Message, response.StatusCode, response.ErrorException);
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.ErrorMessage = ex.Message;
                response.ErrorException = new RestException(ex.Message, response.StatusCode, response.ErrorException);
            }

            if (LogEnabled)
            {
                Logger.Info(sb.ToString(), RestConstants.RestComponent);
            }

            return response;
        }

        private RestResponse GetResponse(RestRequest request, StringBuilder sb)
        {
            WebRequest webRequest = request.Create(sb);
            RestResponse response = new RestResponse { Request = request };
            request.ProgressChanged += (sender, e) => this.InvokeUploadProgressChanged(e);

            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                response.ContentEncoding = webResponse.ContentEncoding;

                response.Server = webResponse.Server;

                response.ContentType = webResponse.ContentType;

                response.ContentLength = webResponse.ContentLength;

                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    byte[] buffer = new Byte[RestConstants.BufferSize];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int bytesRead;
                        long totalByteRead = 0;
                        while (responseStream != null && (bytesRead = responseStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            ms.Write(buffer, 0, bytesRead);
                            ProgressChangedEventArgs progressChangedEventArgs = new ProgressChangedEventArgs();
                            progressChangedEventArgs.TotalBytesToReceive = response.ContentLength;
                            totalByteRead += bytesRead;
                            progressChangedEventArgs.BytesReceived = totalByteRead;
                            this.InvokeDownloadProgressChanged(progressChangedEventArgs);
                            Thread.Sleep(1);
                        }

                        response.ContentArray = ms.ToArray();
                    }
                }


                response.ResponseMode = GetResponseMode(response.ContentType);
                response.StatusCode = webResponse.StatusCode;

                response.StatusDescription = webResponse.StatusDescription;

                sb.AppendLine("HTTP/1.1 {0} {1}".FormatString(((int)webResponse.StatusCode), webResponse.StatusDescription));
                response.ResponseUri = webResponse.ResponseUri;

                response.Status = ResponseStatus.Completed;
                if (webResponse.Cookies != null)
                {
                    foreach (Cookie cookie in webResponse.Cookies)
                    {
                        response.Cookies.Add(cookie);
                    }
                }

                foreach (string headerName in webResponse.Headers.AllKeys)
                {
                    string headerValue = webResponse.Headers[headerName];
                    response.Headers.Add(headerName, headerValue);
                    sb.AppendLine("{0} : {1}".FormatString(headerName, headerValue));
                }

                sb.AppendLine();
            }

            sb.AppendLine(response.Content);

            return response;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to get.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual RestResponse<T> Execute<T>(RestRequest request, MethodType method) where T : new()
        {
            RestResponse<T> response = new RestResponse<T>(this.Execute(request, method));
            try
            {
                if (!string.IsNullOrWhiteSpace(response.Content))
                {
                    switch (response.ResponseMode)
                    {
                        case ResponseMode.Json:
                            IJsonSerializer jsonSerializer = Container.Get<IJsonSerializer>();

                            response.ContentObject = jsonSerializer.Deserialize<T>(response.Content);
                            break;
                        case ResponseMode.Xml:
                            IXmlSerializer xmlSerializer = Container.Get<IXmlSerializer>();
                            response.ContentObject = xmlSerializer.Deserialize<T>(response.Content);
                            break;
                    }

                    if (typeof(ISerializable).IsAssignableFrom(typeof(T)))
                    {
                        if (response.ContentObject == null)
                        {
                            IReflectionType reflectionType = Reflector.Get<T>();
                            response.ContentObject = (T)reflectionType.CreateInstance();
                            ((ISerializable)response.ContentObject).Deserialize(response.Content);
                        }

                    }
                }

            }
            catch (Exception exception)
            {
                response.Status = ResponseStatus.Error;
                response.ErrorMessage = exception.Message;
                response.ErrorException = new RestException(exception.Message, response.StatusCode, response.ErrorException);
            }

            return response;
        }
    }
}
