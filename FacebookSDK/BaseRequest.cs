namespace FacebookSDK
{
    using Framework;
    using Framework.Rest;

    public abstract class BaseRequest
    {
        protected internal abstract void AddFields(RestRequest request);

        protected void AddFieldIfNotEmpty(RestRequest request, string fieldName, string[] array)
        {
            if (array != null && array.Length > 0)
            {
                request.AddBody(fieldName, array.ToConcatenatedString(","));
            }
        }

        protected void AddFieldIfNotEmpty(RestRequest request, string fieldName, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                request.AddBody(fieldName, value);
            }
        }
    }
}
