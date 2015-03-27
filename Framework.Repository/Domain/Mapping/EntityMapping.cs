namespace Framework.Domain.Mapping
{
    using System.Security;

    using Framework.Domain;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Entity mapping.
    /// </summary>
    ///
    /// <typeparam name="T">
    ///     Generic type parameter.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public abstract class EntityMapping<T, V> : BaseEntityMapping<T>
           where T : Entity<V>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the EntityMapping class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        protected EntityMapping()
        {
            this.HasKey(c => c.ID);
        }
    }
}
