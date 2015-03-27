using System;

namespace Framework.Rest.OAuth
{
    public static class OAuthExtensions
    {
        public static void ThrowException(this OAuth2BaseResponse response)
        {
            if (response != null && (!string.IsNullOrWhiteSpace(response.ErrorCode) || !string.IsNullOrWhiteSpace(response.ErrorMessage)))
            {
                if (string.IsNullOrWhiteSpace(response.ErrorMessage))
                {
                    throw new ApplicationException(response.ErrorMessage);
                }

                if (string.IsNullOrWhiteSpace(response.ErrorCode))
                {
                    throw new ApplicationException(response.ErrorCode);
                }
            }
        }
    }
}
