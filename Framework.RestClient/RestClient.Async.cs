using System;
using System.Threading.Tasks;

namespace Framework.Rest
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;

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
        ///     Gets the asynchronous.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     The asynchronous response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse> GetAsync(RestRequest request)
        {
            request.Method = MethodType.Get;
            return await ExecuteAsync(request);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     The async response;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse<T>> GetAsync<T>(RestRequest request) where T : new()
        {
            return await ExecuteAsync<T>(request, MethodType.Get);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Posts the asynchronous.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     Async Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse> PostAsync(RestRequest request)
        {
            request.Method = MethodType.Post;
            return await ExecuteAsync(request);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Posts the asynchronous.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     Async Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse<T>> PostAsync<T>(RestRequest request) where T : new()
        {
            return await ExecuteAsync<T>(request, MethodType.Post);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Puts the asynchronous.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     Async Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse> PutAsync(RestRequest request)
        {
            request.Method = MethodType.Put;
            return await ExecuteAsync(request);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Puts the asynchronous.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     Async Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse<T>> PutAsync<T>(RestRequest request) where T : new()
        {
            return await ExecuteAsync<T>(request, MethodType.Put);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the asynchronous described by request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     Async Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse> DeleteAsync(RestRequest request)
        {
            request.Method = MethodType.Delete;
            return await ExecuteAsync(request);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the asynchronous described by request.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     Async Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse<T>> DeleteAsync<T>(RestRequest request) where T : new()
        {
            return await ExecuteAsync<T>(request, MethodType.Delete);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Head asynchronous.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     Async Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse> HeadAsync(RestRequest request)
        {
            request.Method = MethodType.Head;
            return await ExecuteAsync(request);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Head asynchronous.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request.
        /// </param>
        ///
        /// <returns>
        ///     Async Response.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public async Task<RestResponse<T>> HeadAsync<T>(RestRequest request) where T : new()
        {
            return await ExecuteAsync<T>(request, MethodType.Head);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            return await GetResponseAsync(request);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest request, MethodType method) where T : new()
        {
            request.Method = method;
            RestResponse<T> response = new RestResponse<T>(await GetResponseAsync(request));
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

        private async Task<RestResponse> GetResponseAsync(RestRequest request)
        {
            RestResponse response = new RestResponse { Request = request };
            request.ProgressChanged += (sender, e) => this.InvokeUploadProgressChanged(e);
            StringBuilder sb = new StringBuilder();

            try
            {
                var webRequest = await request.CreateAsync(sb);

                HttpWebResponse webResponse = (HttpWebResponse)(await webRequest.GetResponseAsync());

                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    byte[] buffer = new Byte[RestConstants.BufferSize];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int bytesRead;
                        long totalByteRead = 0;
                        while (responseStream != null && (bytesRead = responseStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            await ms.WriteAsync(buffer, 0, bytesRead);
                            ProgressChangedEventArgs progressChangedEventArgs = new ProgressChangedEventArgs();
                            totalByteRead += bytesRead;
                            progressChangedEventArgs.BytesReceived = totalByteRead;
                            this.InvokeDownloadProgressChanged(progressChangedEventArgs);
                        }

                        response.ContentArray = ms.ToArray();
                    }
                }

                response.ResponseMode = GetResponseMode(response.ContentType);
                response.StatusCode = webResponse.StatusCode;
                sb.AppendLine("HTTP/1.1 {0} {1}".FormatString(((int)webResponse.StatusCode), webResponse.StatusDescription));

                response.StatusDescription = webResponse.StatusDescription;
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
                    response.Headers[headerName] = headerValue;
                    sb.AppendLine("{0} : {1}".FormatString(headerName, headerValue));
                }

                response.ContentType = webResponse.ContentType;
                response.ContentLength = webResponse.ContentLength;
                response.StatusCode = webResponse.StatusCode;
                response.ResponseMode = GetResponseMode(response.ContentType);
                response.StatusDescription = webResponse.StatusDescription;
                response.ResponseUri = webResponse.ResponseUri;
                response.Status = ResponseStatus.Completed;
                sb.AppendLine(response.Content);
            }
            catch (WebException exception)
            {
                response.Status = ResponseStatus.Error;
                response.ErrorMessage = exception.Message;
                response.ErrorException = new RestException(exception.Message, response.StatusCode, response.ErrorException);
                sb.AppendLine(exception.Message);

                try
                {
                    if (exception.Response != null)
                    {
                        HttpWebResponse webResponse = (HttpWebResponse)exception.Response;
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
                            response.Headers[headerName] = headerValue;
                            sb.AppendLine("{0} : {1}".FormatString(headerName, headerValue));
                        }

                        sb.AppendLine();

                        sb.AppendLine(response.Content);

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

            return response;
        }
    }
}
