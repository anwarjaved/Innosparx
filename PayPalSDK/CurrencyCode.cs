namespace PayPalSDK
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// ISO 4217 standard 3-letter currency code. 
    /// </summary>
    [Serializable]
    public enum CurrencyCode
    {
        /// <summary>
        /// Australian Dollar.
        /// </summary>
        [Description("AUD")]
        AustralianDollar,

        /// <summary>
        /// Canadian Dollar.
        /// </summary>
        [Description("CAD")]
        CanadianDollar,

        /// <summary>
        /// Czech Koruna.
        /// </summary>
        [Description("CZK")]
        CzechKoruna,

        /// <summary>
        /// Danish Krone.
        /// </summary>
        [Description("DKK")]
        DanishKrone,

        /// <summary>
        /// Euro Currency.
        /// </summary>
        [Description("EUR")]
        Euro,

        /// <summary>
        /// HongKong Dollar.
        /// </summary>
        [Description("HKD")]
        HongKongDollar,

        /// <summary>
        /// Hungarian Forint.
        /// </summary>
        [Description("HUF")]
        HungarianForint,

        /// <summary>
        /// Japanese Yen.
        /// </summary>
        [Description("JPY")]
        JapaneseYen,

        /// <summary>
        /// NewZealand Dollar.
        /// </summary>
        [Description("NZD")]
        NewZealandDollar,

        /// <summary>
        /// Norwegian Krone.
        /// </summary>
        [Description("NOK")]
        NorwegianKrone,

        /// <summary>
        /// Polish Zloty.
        /// </summary>
        [Description("PLN")]
        PolishZloty,

        /// <summary>
        /// Pound Sterling.
        /// </summary>
        [Description("GBP")]
        PoundSterling,

        /// <summary>
        /// Singapore Dollar.
        /// </summary>
        [Description("SGD")]
        SingaporeDollar,

        /// <summary>
        /// Swedish Krona.
        /// </summary>
        [Description("SEK")]
        SwedishKrona,

        /// <summary>
        /// Swiss Franc.
        /// </summary>
        [Description("CHF")]
        SwissFranc,

        /// <summary>
        /// US Dollar.
        /// </summary>
        [Description("USD")]
        USDollar,

        [Description("RUB")]
        RussianRuble,

        [Description("PHP")]
        PhillipinePeso,

        [Description("MXN")]
        MexicanPeso,

        [Description("ILS")]
        IsraeliNewSheqel,

        [Description("THB")]
        ThaiBaht
    }
}