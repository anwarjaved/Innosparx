using System;

namespace Framework.Domain
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Aggregate entity.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public abstract class AggregateEntity<T> : Entity<T>, IAggregateRoot<T>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the AggregateEntity class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        protected AggregateEntity()
        {
            this.CreateDate = DateTime.UtcNow;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the create.
        /// </summary>
        ///
        /// <value>
        ///     The date of the create.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime CreateDate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last updated.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last updated.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? LastUpdatedDate { get; set; }
    }
}
