// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentStatus.cs" company="iSilver Labs">
//   Copyright (c) iSilverLabs.com All rights reserved.
// </copyright>
// <summary>
//   The status of the payment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// The status of the payment.
    /// </summary>
    [Serializable]
    public enum PaymentStatus
    {
        /// <summary>
        /// No status.
        /// </summary>
        [Description("")]
        None,

        /// <summary>
        /// A reversal has been canceled; for example, when you win
        /// a dispute and the funds for the reversal have been returned to you.
        /// </summary>
        [Description("Canceled_Reversal")]
        CanceledReversal,

        /// <summary>
        /// The payment has been completed, and the funds have been added
        /// successfully to your account balance.
        /// </summary>
        [Description("Completed")]
        Completed,

        /// <summary>
        /// A German ELV payment is made using Express Checkout.
        /// </summary>
        [Description("Created")]
        Created,

        /// <summary>
        /// You denied the payment.
        /// </summary>
        [Description("Denied")]
        Denied,

        /// <summary>
        /// This authorization has expired and cannot be captured.
        /// </summary>
        [Description("Expired")]
        Expired,

        /// <summary>
        /// The payment has failed.
        /// </summary>
        [Description("Failed")]
        Failed,

        /// <summary>
        /// The payment is pending.
        /// </summary>
        [Description("Pending")]
        Pending,

        /// <summary>
        /// You refunded the payment.
        /// </summary>
        [Description("Refunded")]
        Refunded,

        /// <summary>
        /// Payment was reversed due to a chargeback or other type of reversal.
        /// </summary>
        [Description("Reversed")]
        Reversed,

        /// <summary>
        /// A payment has been accepted.
        /// </summary>
        [Description("Processed")]
        Processed,

        /// <summary>
        /// This authorization has been voided.
        /// </summary>
        [Description("Voided")]
        Voided,
    }
}