// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DisplayDetails.cs" company="iSilver Labs">
//   Copyright (c) iSilverLabs.com All rights reserved.
// </copyright>
// <summary>
//   Defines the DisplayDetailsButton type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace PayPalSDK.WebsiteStandard
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;

    using Framework;
    using Framework.Collections;

    /// <summary>
    /// Represent Variables for Displaying PayPal Checkout Pages.
    /// </summary>
    public sealed class DisplayDetails
    {
        public DisplayDetails()
        {
            this.Language = CountryCode.UnitedStates;
            this.CheckoutBackColor = CheckoutBackColor.White;
            this.HeaderBackColor = Color.Empty;
            this.HeaderBorderColor = Color.Empty;
            this.PayFlowColor = Color.Empty;

        }

        /// <summary>
        /// Gets or sets the language for the information/log-in page only.
        /// </summary>
        /// <value>The language.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(CountryCode), "UnitedStates")]
        [Bindable(true)]
        public CountryCode Language { get; set; }

        /// <summary>
        /// Gets or sets A URL to which the payer’s browser is redirected if payment is cancelled.
        /// </summary>
        /// <value>A URL to which the payer’s browser is redirected if payment is cancelled.</value>
        [DefaultValue("")]
        [System.ComponentModel.Description("The URL of the image to be shown.")]
        [Bindable(true)]
        [Category("PayPal")]
        internal string CancelUrl { get; set; }

        /// <summary>
        /// Gets or sets the success URL.
        /// </summary>
        /// <value>The success URL.</value>
        [DefaultValue("")]
        [System.ComponentModel.Description("The URL of the image to be shown.")]
        [Bindable(true)]
        [Category("PayPal")]
        internal string SuccessUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of the 150x50-pixel image displayed as your logo in the upper left corner of the PayPal checkout pages.
        /// </summary>
        /// <value>The URL of the 150x50-pixel image displayed as your logo in the upper left corner of the PayPal checkout pages.</value>
        [DefaultValue("")]
        [System.ComponentModel.Description("The URL of the image to be shown.")]
        [Bindable(true)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.All)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the header image URL.
        /// </summary>
        /// <value>The header image URL.</value>
        [DefaultValue("")]
        [System.ComponentModel.Description("The URL of the image to be shown.")]
        [Bindable(true)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.All)]
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the color of the header back.
        /// </summary>
        /// <value>The color of the header back.</value>
        [NotifyParentProperty(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "")]
        public Color HeaderBackColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the header border.
        /// </summary>
        /// <value>The color of the header border.</value>
        [NotifyParentProperty(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "")]
        public Color HeaderBorderColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the pay flow.
        /// </summary>
        /// <value>The color of the pay flow.</value>
        [NotifyParentProperty(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "")]
        public Color PayFlowColor { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>The background color.</value>
        [NotifyParentProperty(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(CheckoutBackColor), "White")]
        public CheckoutBackColor CheckoutBackColor { get; set; }

        /// <summary>
        /// Gets or sets the continue text.
        /// </summary>
        /// <value>The continue text.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string ContinueText { get; set; }

        /// <summary>
        /// Gets or sets the note text.
        /// </summary>
        /// <value>The note text.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string NoteText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether display note box.
        /// </summary>
        /// <value>
        /// <see langword="true"/> If display note; otherwise, <see langword="false"/>.
        /// </value>
        [NotifyParentProperty(true)]
        [DefaultValue(false)]
        [Bindable(true)]
        public bool HideNote { get; set; }

        internal IDictionary<string, string> GetValues()
        {
            OrderedDictionary<string, string> dictionary = new OrderedDictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(this.ImageUrl))
            {
                dictionary.Add("image_url", this.ImageUrl);
            }

            if (!this.HeaderImageUrl.IsEmpty())
            {
                dictionary.Add("cpp_header_image", this.HeaderImageUrl);
            }

            if (!this.HeaderBackColor.IsEmpty)
            {
                dictionary.Add("cpp_headerback_color", ColorTranslator.ToHtml(this.HeaderBackColor));
            }

            if (!this.HeaderBorderColor.IsEmpty)
            {
                dictionary.Add("cpp_headerborder_color", ColorTranslator.ToHtml(this.HeaderBorderColor));
            }

            if (!this.PayFlowColor.IsEmpty)
            {
                dictionary.Add("cpp_payflow_color", ColorTranslator.ToHtml(this.PayFlowColor));
            }

            if (this.CheckoutBackColor == CheckoutBackColor.Black)
            {
                dictionary.Add("cs", "1");
            }

            dictionary.Add("lc", this.Language.ToDescription());

            if (!this.HideNote)
            {
                dictionary.Add("cn", this.NoteText);
            }
            else
            {
                dictionary.Add("no_note", "1");
            }

            dictionary.Add("no_ird", "1");
            dictionary.Add("no_shipping", "1");

            if (!this.SuccessUrl.IsEmpty())
            {
                dictionary.Add("return", this.SuccessUrl);

                dictionary.Add("rm", "1");
            }

            if (!this.ContinueText.IsEmpty())
            {
                dictionary.Add("cbt", this.ContinueText);
            }

            if (!this.CancelUrl.IsEmpty())
            {
                dictionary.Add("cancel_return", this.CancelUrl);
            }

            return dictionary;
        }
    }
}