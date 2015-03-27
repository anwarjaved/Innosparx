namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;

    using Framework;
    using Framework.Collections;

    /// <summary>
    /// Represent an individual item in Paypal Invoice.
    /// </summary>
    public sealed class SingleItemPaymentDetails
    {
        public SingleItemPaymentDetails()
        {
            this.Payer = new PayerDetails();
            this.Quantity = 1;
            this.WeightUnit = WeightUnit.Pound;
            this.Currency = CurrencyCode.USDollar;
        }

        /// <summary>
        /// Gets the payer.
        /// </summary>
        /// <value>The payer.</value>
        [NotifyParentProperty(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PayerDetails Payer { get; private set; }

        /// <summary>
        /// Gets or sets The price or amount of the product, service, or contribution, not including shipping, handling, or tax.
        /// </summary>
        /// <value>The price or amount of the product, service, or contribution, not including shipping, handling, or tax.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(default(double))]
        [Bindable(true)]
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the Description of item.
        /// </summary>
        /// <value>The Description of item.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Number of items.
        /// </summary>
        /// <value>The Number of items.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(1)]
        [Bindable(true)]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets Pass-through variable for you to track product or service
        /// purchased or the contribution made. The value you specify
        /// passed back to you upon payment completion.
        /// </summary>
        /// <value>Pass-through variable for you to track product or service
        /// purchased or the contribution made. The value you specify
        /// passed back to you upon payment completion.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets Passthrough variable never presented to the payer.
        /// </summary>
        /// <value>Passthrough variable never presented to the payer.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string Custom { get; set; }

        /// <summary>
        /// Gets or sets the Passthrough variable you can use to identify your invoice number for this purchase.
        /// </summary>
        /// <value>Passthrough variable you can use to identify your invoice number for this purchase.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the cost of shipping this item.
        /// </summary>
        /// <value>The cost of shipping this item.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(default(double))]
        [Bindable(true)]
        public double ShippingCost { get; set; }

        /// <summary>
        /// Gets or sets the Handling charges.
        /// </summary>
        /// <value>The handling charges.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(default(double))]
        [Bindable(true)]
        public double HandlingCost { get; set; }

        /// <summary>
        /// Gets or sets Transaction-based tax.
        /// </summary>
        /// <value>The Transaction-based tax.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(default(double))]
        [Bindable(true)]
        public double Tax { get; set; }

        /// <summary>
        /// Gets or sets the Weight of items.
        /// </summary>
        /// <value>The Weight of items.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(default(double))]
        [Bindable(true)]
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the unit of measure if weight is specified.
        /// </summary>
        /// <value>The unit of measure if weight is specified.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(WeightUnit), "Pound")]
        [Bindable(true)]
        public WeightUnit WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets The currency of the payment. The default is USD.
        /// </summary>
        /// <value>The currency of the payment. The default is USD.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(CurrencyCode), "USDollar")]
        [Bindable(true)]
        public CurrencyCode Currency { get; set; }

        internal IDictionary<string, string> GetValues()
        {
            OrderedDictionary<string, string> dictionary = new OrderedDictionary<string, string>(this.Payer.GetValues());

            if (this.Name.IsEmpty())
            {
                throw new InvalidOperationException("{0} - Item Name is required".FormatString(this.ID));
            }

            if (!this.ID.IsEmpty())
            {
                dictionary.Add("item_number", this.ID);
            }

            dictionary.Add("amount", string.Format(CultureInfo.InvariantCulture, "{0:0.00}", this.Amount));
            dictionary.Add("item_name", this.Name);

            dictionary.Add("quantity", this.Quantity.ToString(CultureInfo.InvariantCulture));

            if (this.ShippingCost > 0)
            {
                dictionary.Add("shipping", string.Format(CultureInfo.InvariantCulture, "{0:0.00}", this.ShippingCost));
            }

            if (this.Tax > 0)
            {
                dictionary.Add("tax", string.Format(CultureInfo.InvariantCulture, "{0:0.00}", this.Tax));
            }

            if (this.Weight > 0)
            {
                dictionary.Add("weight", string.Format(CultureInfo.InvariantCulture, "{0:0.00}", this.Weight));
                dictionary.Add("weight_unit", this.WeightUnit.ToDescription());
            }

            dictionary.Add("address_override", "1");
            dictionary.Add("currency_code", this.Currency.ToDescription());

            if (!this.Custom.IsEmpty())
            {
                dictionary.Add("custom", this.Custom);
            }

            if (this.HandlingCost > 0)
            {
                dictionary.Add("handling", string.Format(CultureInfo.InvariantCulture, "{0:0.00}", this.HandlingCost));
            }

            if (!this.InvoiceNumber.IsEmpty())
            {
                dictionary.Add("invoice", this.InvoiceNumber);
            }

            return dictionary;
        }
    }
}