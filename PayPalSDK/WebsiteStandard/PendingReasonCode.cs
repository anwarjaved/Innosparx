namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// PendingReasonCode is returned in the response only if <see cref="PaymentStatus"/> is Pending.
    /// </summary>
    [Serializable]
    public enum PendingReasonCode
    {
        /// <summary>
        /// No reason code.
        /// </summary>
        [Description("")]
        None,

        /// <summary>
        /// The payment is pending because your customer did not include a confirmed
        /// shipping address and your Payment Receiving Preferences is  set such that you
        /// want to manually accept or deny each of these payments.  To change your
        /// preference, go to the Preferences section of your Profile.
        /// </summary>
        [Description("address")]
        Address,

        /// <summary>
        /// The payment is pending because it has been authorized but not
        /// settled. You must capture the funds first.
        /// </summary>
        [Description("authorization")]
        Authorization,

        /// <summary>
        /// The payment is pending because it was made by an eCheck that has not yet cleared.
        /// </summary>
        [Description("echeck")]
        ECheck,

        /// <summary>
        /// The payment is pending because you hold a non-U.S. account and do not
        /// have a withdrawal mechanism. You must manually accept or deny this payment
        /// from your Account Overview.
        /// </summary>
        [Description("intl")]
        International,

        /// <summary>
        /// You do not have a balance in the currency sent, and you do
        /// not have your Payment Receiving Preferences set to automatically
        /// convert and accept this payment. You must manually accept or deny this payment.
        /// </summary>
        [Description("multi-currency")]
        MultiCurrency,

        /// <summary>
        /// The payment is pending because it is part of an order that has been authorized but not settled.
        /// </summary>
        [Description("order")]
        Order,

        /// <summary>
        /// The payment is pending while it is being reviewed by PayPal for risk.
        /// </summary>
        [Description("paymentreview")]
        PaymentReview,

        /// <summary>
        /// The payment is pending because it was made to an email address
        /// that is not yet registered or confirmed.
        /// </summary>
        [Description("unilateral")]
        Unilateral,

        /// <summary>
        /// The payment is pending because it was made via credit card
        /// and you must upgrade your account to Business or Premier status in order
        /// to receive the funds. upgrade can also mean that you have reached the
        /// monthly limit for transactions on your account.
        /// </summary>
        [Description("upgrade")]
        Upgrade,

        /// <summary>
        /// The payment is pending because you are not yet verified. You must
        /// verify your account before you can accept this payment.
        /// </summary>
        [Description("verify")]
        Verify,

        /// <summary>
        /// The payment is pending for a reason other than those listed above. For
        /// more information, contact PayPal customer service.
        /// </summary>
        [Description("other")]
        Other,
    }
}