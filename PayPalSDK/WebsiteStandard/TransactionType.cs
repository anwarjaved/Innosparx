namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// The type of transaction.
    /// </summary>
    [Serializable]
    public enum TransactionType
    {
        /// <summary>
        /// No Transaction.
        /// </summary>
        [Description("")]
        None,

        /// <summary>
        /// Payment received; source is a Buy Now, Donation, or Auction Smart Logos button.
        /// </summary>
        [Description("web_accept")]
        PaymentReceived,

        /// <summary>
        /// A dispute has been resolved and closed.
        /// </summary>
        [Description("adjustment")]
        Adjustment,

        /// <summary>
        /// Payment received; source is Virtual Terminal.
        /// </summary>
        [Description("virtual_terminal")]
        VirtualTerminal,

        /// <summary>
        /// Subscription started.
        /// </summary>
        [Description("subscr_signup")]
        SubscriptionStarted,

        /// <summary>
        /// Subscription payment received.
        /// </summary>
        [Description("subscr_payment")]
        SubscriptionPaymentReceived,

        /// <summary>
        /// Subscription modified.
        /// </summary>
        [Description("subscr_modify")]
        SubscriptionModified,

        /// <summary>
        /// Subscription signup failed.
        /// </summary>
        [Description("subscr_failed")]
        SubscriptionSignupFailed,

        /// <summary>
        /// Subscription expired.
        /// </summary>
        [Description("subscr_eot")]
        SubscriptionExpired,

        /// <summary>
        /// Subscription canceled.
        /// </summary>
        [Description("subscr_cancel")]
        SubscriptionCanceled,

        /// <summary>
        /// Payment received; source is the Send Money tab on the PayPal website.
        /// </summary>
        [Description("send_money")]
        SendMoney,

        /// <summary>
        /// Recurring payment profile created.
        /// </summary>
        [Description("recurring_payment_profile_created")]
        RecurringPaymentProfileCreated,

        /// <summary>
        /// Recurring payment received.
        /// </summary>
        [Description("recurring_payment")]
        RecurringPayment,

        /// <summary>
        /// A new dispute was filed.
        /// </summary>
        [Description("new_case")]
        NewCase,

        /// <summary>
        /// Monthly subscription paid for Website Payments Pro.
        /// </summary>
        [Description("merch_pmt")]
        MerchantPayment,

        /// <summary>
        /// Payment received for a single item; source is Express Checkout.
        /// </summary>
        [Description("masspay")]
        MassPay,

        /// <summary>
        /// Payment received for multiple items; source is Express Checkout or the PayPal Shopping Cart.
        /// </summary>
        [Description("cart")]
        Cart,

        /// <summary>
        /// Express-checkout.Payment received for a single item; source is Express Checkout.
        /// </summary>
        [Description("express_checkout")]
        ExpressCheckout
    }
}