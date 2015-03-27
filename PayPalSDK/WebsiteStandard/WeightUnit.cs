namespace PayPalSDK.WebsiteStandard
{
    using System.ComponentModel;

    /// <summary>
    /// Type of Weight Unit.
    /// </summary>
    public enum WeightUnit
    {
        /// <summary>
        /// Weight in Pounds.
        /// </summary>
        [Description("lbs")]
        Pound,

        /// <summary>
        /// Weight in Kilograms.
        /// </summary>
        [Description("kgs")]
        Kilogram
    }
}