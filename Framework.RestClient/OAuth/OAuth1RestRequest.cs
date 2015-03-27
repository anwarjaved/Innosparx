using System;
using System.Text;

namespace Framework.Rest.OAuth
{
    using System.Collections.Generic;

    public class OAuth1RestRequest : RestRequest
    {
        private readonly Dictionary<string, string> oAuthValues = new Dictionary<string, string>();

        internal Dictionary<string, string> OAuthValues
        {
            get
            {
                return this.oAuthValues;
            }
        }

        public OAuth1RestRequest(Uri resource, RequestMode requestMode, AcceptMode acceptMode, Encoding encoding)
            : base(resource, requestMode, acceptMode, encoding)
        {
        }

        public OAuth1RestRequest(Uri resource, RequestMode requestMode, Encoding encoding)
            : base(resource, requestMode, encoding)
        {
        }

        public OAuth1RestRequest(Uri resource, RequestMode requestMode = RequestMode.UrlEncoded)
            : base(resource, requestMode)
        {
        }

        public OAuth1RestRequest(Uri resource, RequestMode requestMode, AcceptMode acceptMode)
            : base(resource, requestMode, acceptMode)
        {
        }

        public OAuth1RestRequest(string resource, RequestMode requestMode, Encoding encoding)
            : base(resource, requestMode, encoding)
        {
        }

        public OAuth1RestRequest(string resource, RequestMode requestMode, AcceptMode acceptMode, Encoding encoding)
            : base(resource, requestMode, acceptMode, encoding)
        {
        }

        public OAuth1RestRequest(string resource, RequestMode requestMode)
            : base(resource, requestMode)
        {
        }

        public OAuth1RestRequest(string resource, RequestMode requestMode, AcceptMode acceptMode)
            : base(resource, requestMode, acceptMode)
        {
        }

        public OAuth1RestRequest(string resource, AcceptMode acceptMode)
            : base(resource, acceptMode)
        {
        }

        public OAuth1RestRequest(string resource)
            : base(resource)
        {
        }

        public OAuth1RestRequest(string resource, Encoding encoding)
            : base(resource, encoding)
        {
        }

        public OAuth1RestRequest(string resource, AcceptMode acceptMode, Encoding encoding)
            : base(resource, acceptMode, encoding)
        {
        }

        public OAuth1RestRequest(Uri resource, AcceptMode acceptMode)
            : base(resource, acceptMode)
        {
        }

        public OAuth1RestRequest(Uri resource, Encoding encoding)
            : base(resource, encoding)
        {
        }

        public OAuth1RestRequest(Uri resource, AcceptMode acceptMode, Encoding encoding)
            : base(resource, acceptMode, encoding)
        {
        }

        public override void AddBody(string key, string value)
        {
            base.AddBody(key, value);
            this.OAuthValues.Add(key, value);
        }

        protected override string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (RestConstants.UnreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + string.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }
    }
}
