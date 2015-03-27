using System;

namespace Framework.Rest
{
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    partial class RestRequest
    {
        internal async Task<HttpWebRequest> CreateAsync(StringBuilder sb)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(this.RequestUrl);
            webRequest.UserAgent = this.UserAgent;

            webRequest.AllowWriteStreamBuffering = this.AllowWriteStreamBuffering;
            webRequest.AllowReadStreamBuffering = true;

            webRequest.PreAuthenticate = this.PreAuthenticate;
            this.HandleLogin(webRequest);

            this.AddCertificate(webRequest);

            this.HandleProxy(webRequest);

            this.AddCookies(webRequest);

            this.AddHeaders(webRequest);

            this.HandleRequestType(webRequest);

            this.HandleAcceptType(webRequest);

            this.HandleMethodType(webRequest);

            string data = await AddBodyAsync(webRequest);

            BuildRequestLog(webRequest, data, ref sb);

            return webRequest;
        }

        private async Task<string> AddBodyAsync(HttpWebRequest webRequest)
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

                    using (var requestStream = await Task<Stream>.Factory.FromAsync(webRequest.BeginGetRequestStream, webRequest.EndGetRequestStream, webRequest))
                    {
                        byte[] buffer = new Byte[RestConstants.BufferSize];

                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            int bytesRead;
                            long totalByteWrite = 0;
                            while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                await requestStream.WriteAsync(buffer, 0, bytesRead);
                                totalByteWrite += bytesRead;
                                progressChangedEventArgs.BytesSent = totalByteWrite;
                                this.InvokeProgressChanged(progressChangedEventArgs);
                            }
                        }

                    }
                }

                return this.Encoding.GetString(data);
            }

            return null;
        }
    }
}
