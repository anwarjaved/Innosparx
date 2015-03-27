using System;

namespace Framework.Domain
{
    /// <summary>
    /// Interface implmented by The Aggregate Root objects.
    /// </summary>
    public interface IAggregateRoot<T> : IEntity<T>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the create.
        /// </summary>
        ///
        /// <value>
        ///     The date of the create.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        DateTime CreateDate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last updated.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last updated.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        DateTime? LastUpdatedDate { get; set; }
    }
}
