namespace Framework.Localization
{
    using System.ComponentModel;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent LanguageCode.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public enum LanguageCode : byte
    {
        /// <summary>
        ///     English Language.
        /// </summary>
        [Description("1033,9,3081,10249,4105,6153,8201,5129,7177,11273,2057")]
        English = 0,

        /// <summary>
        ///     Russian Language.
        /// </summary>
        [Description("1049,2073")]
        Russian = 1
    }
}
