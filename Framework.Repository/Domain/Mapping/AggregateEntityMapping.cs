namespace Framework.Domain.Mapping
{
    using System.Security;

    using Framework.Domain;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Aggregate entity mapping.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/04/2014 7:12 PM.
    /// </remarks>
    ///
    /// <typeparam name="T">
    ///     Generic type parameter.
    /// </typeparam>
    /// <typeparam name="V">
    ///     Generic type parameter.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    
    public abstract class AggregateEntityMapping<T, V> : EntityMapping<T, V>
        where T : AggregateEntity<V>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the AggregateEntityMapping class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        protected AggregateEntityMapping()
        {
            this.Property(c => c.CreateDate);
            this.Property(c => c.LastUpdatedDate);
        }
    }
}
