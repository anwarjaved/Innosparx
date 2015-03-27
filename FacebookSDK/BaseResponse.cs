namespace FacebookSDK
{
    using Framework.Rest.OAuth;

    public abstract class BaseResponse : OAuth2BaseResponse
    {
        public string ID { get; set; }
    }
}
