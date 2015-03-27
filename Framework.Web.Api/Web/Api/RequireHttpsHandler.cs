namespace Framework.Web.Api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;

    public class RequireHttpsHandler : DelegatingHandler
    {
        [SecuritySafeCritical]
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                var forbiddenResponse =
                    request.CreateResponse(HttpStatusCode.Forbidden);

                forbiddenResponse.ReasonPhrase = "HTTPS Required";
                return Task.FromResult(forbiddenResponse);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
