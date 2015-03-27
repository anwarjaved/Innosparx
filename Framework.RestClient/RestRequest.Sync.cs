using System;

namespace Framework.Rest
{
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    partial class RestRequest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new <see cref="HttpWebRequest"/> object.
        /// </summary>
        ///
        /// <returns>
        ///     <see cref="HttpWebRequest"/> object.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        internal HttpWebRequest Create(StringBuilder sb)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(this.RequestUrl);

            if (this.Timeout == int.MaxValue)
            {
                webRequest.Timeout = System.Threading.Timeout.Infinite;
            }
            webRequest.AllowWriteStreamBuffering = this.AllowWriteStreamBuffering;
            //webRequest.AllowReadStreamBuffering = true;
            webRequest.UserAgent = this.UserAgent;

            webRequest.PreAuthenticate = this.PreAuthenticate;

            this.HandleLogin(webRequest);

            this.AddCertificate(webRequest);

            this.HandleProxy(webRequest);

            this.AddCookies(webRequest);

            this.AddHeaders(webRequest);

            this.HandleRequestType(webRequest);

            this.HandleAcceptType(webRequest);

            this.HandleMethodType(webRequest);

            string data = this.AddBody(webRequest);

            BuildRequestLog(webRequest, data, ref sb);

            return webRequest;
        }

        private void HandleProxy(HttpWebRequest webRequest)
        {
            if (!string.IsNullOrEmpty(this.ProxyAddress))
            {
                if (this.ProxyAddress == "DEFAULTPROXY")
                {
                    webRequest.Proxy = new WebProxy();
                }
                else
                {
                    WebProxy webProxy = new WebProxy(this.ProxyAddress, true);

                    if (!string.IsNullOrEmpty(this.ProxyBypass))
                    {
                        webProxy.BypassList = this.ProxyBypass.Split(';');
                    }

                    if (!string.IsNullOrEmpty(this.ProxyUserName))
                    {
                        webProxy.Credentials = new NetworkCredential(this.ProxyUserName, this.ProxyPassword);
                    }

                    webRequest.Proxy = webProxy;
                }
            }
        }


        private string AddBody(HttpWebRequest webRequest)
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
                    webRequest.ContentLength = length;

                    if (this.EnableContentMD5)
                    {
                        this.ContentMD5 = Convert.ToBase64String(Cryptography.CreateHash(HashMode.MD5, data));
                    }

                    ProgressChangedEventArgs progressChangedEventArgs = new ProgressChangedEventArgs() { TotalBytesToSend = length };
                    byte[] buffer = new Byte[RestConstants.BufferSize];

                    using (Stream requestStream = webRequest.GetRequestStream())
                    {
                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            int bytesRead;
                            long totalByteWrite = 0;
                            while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                requestStream.Write(buffer, 0, bytesRead);
                                totalByteWrite += bytesRead;
                                progressChangedEventArgs.BytesSent = totalByteWrite;
                                this.InvokeProgressChanged(progressChangedEventArgs);
                            }
                        }
                    }
                }
                else
                {
                    webRequest.ContentLength = 0;
                }

                return this.Encoding.GetString(data);
            }

            return null;
        }

        private void HandleLogin(HttpWebRequest webRequest)
        {
            if (!string.IsNullOrEmpty(this.UserName))
            {
                webRequest.Credentials = this.UserName == "AUTOLOGIN"
                                           ? CredentialCache.DefaultCredentials
                                           : new NetworkCredential(this.UserName, this.Password);
            }
        }

        private void AddCertificate(HttpWebRequest webRequest)
        {
            foreach (X509Certificate certificate in this.ClientCertificates)
            {
                webRequest.ClientCertificates.Add(certificate);
            }
        }




        private void AddCookies(HttpWebRequest webRequest)
        {
            if (this.Cookies.Count > 0)
            {
                webRequest.CookieContainer = new CookieContainer();
                webRequest.CookieContainer.Add(this.Cookies);
            }
        }

    }
}
