using System.Text;

namespace LinkedInSDK
{
    using System;
    using System.Linq;

    using Framework;
    using Framework.Rest;
    using Framework.Rest.OAuth;

    public sealed class LinkedInClient : OAuth2Client
    {
        public LinkedInClient(string appID, string appSecret)
            : base(appID, appSecret, OAuth2TokenAccessType.Querystring)
        {
        }

        public LinkedInClient(string appID, string appSecret, OAuth2TokenCredential credential)
            : base(appID, appSecret, credential, OAuth2TokenAccessType.Querystring)
        {
        }

        public LinkedInClient(string appID, string appSecret, string token)
            : base(appID, appSecret, token, OAuth2TokenAccessType.Querystring)
        {
        }

        public string BuildAuthorizationUrl(
            string redirectUrl,
            string state = "",
            params string[] scopes)
        {
            return this.BuildAuthorizationUrl(LinkedInConstants.AuthorizeUrl, redirectUrl,
                scopes.ToConcatenatedString(x => x, " "), OAuth2ResponseType.Code, state);
        }

        public string BuildAuthorizationUrl(
            string redirectUrl,
            string state = "",
            string permissions = "")
        {
            return this.BuildAuthorizationUrl(LinkedInConstants.AuthorizeUrl, redirectUrl, permissions, OAuth2ResponseType.Code, state);
        }

        public OAuth2TokenCredential GetAccessToken(string code, string redirectUrl, bool throwException = false)
        {
            var response = this.GetAccessToken(LinkedInConstants.TokenUrl, code, redirectUrl);

            OAuth2TokenCredential credential = response.ContentObject;
            if (!response.Completed || credential == null || !credential.Success)
            {
                if (throwException)
                {
                    credential.ThrowException();
                }
            }

            if (credential != null)
            {
                this.Credential = credential;
                return credential;
            }

            return null;
        }

        protected override string OAuth2AccessTokenParameter
        {
            get
            {
                return LinkedInConstants.OAuth2AccessTokenParam;
            }
        }

        protected override void OnInitRequest(RestRequest request)
        {
            base.OnInitRequest(request);
            request.Headers.Add("x-li-format", "json");
        }

        public RestResponse<Profile> GetProfile(params ProfileField[] fields)
        {
            StringBuilder sb = new StringBuilder(LinkedInConstants.ProfileUrl);

            if (fields != null && fields.Length > 0)
            {
                sb.Append(":(");
                sb.Append(string.Join(",", fields.AsEnumerable().Select(x => x.ToDescription())));
                sb.Append(")");
            }

            RestRequest request = new RestRequest(sb.ToString(), AcceptMode.Json);

            return this.Get<Profile>(request);
        }

        public RestResponse<PostResponse> Publish(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException("post");
            }

            RestRequest request = new RestRequest(LinkedInConstants.PostsUrl, RequestMode.Json, AcceptMode.Json);

            request.AddBody(new
                            {
                                content = new PostContent()
                                          {
                                              Description = post.Description,
                                              ImageUrl = post.ImageUrl,
                                              Title = post.Title,
                                              Url = post.Url
                                          },
                                comment = post.Comment,
                                visibility = new
                                             {
                                                 code = post.Visibility.ToDescription()
                                             }
                            });

            return this.Post<PostResponse>(request);
        }
    }
}

