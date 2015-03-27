namespace GoogleSDK.Places
{
    using Framework;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///      The status of the request.
    /// </summary>
    ///
    /// <remarks>
    ///     LM ANWAR, 2/15/2013.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public enum StatusCode
    {
        [Description("OK")]
        Ok,

        [Description("ZERO_RESULTS")]
        NoResult,

        [Description("OVER_QUERY_LIMIT")]
        QuotaLimit,

        [Description("REQUEST_DENIED")]
        RequestDenied,

        [Description("INVALID_REQUEST")]
        InvalidRequest,

    }
}
