namespace Framework.Domain.Mapping
{
    using System.Security;

    using Framework.Domain;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Base entity mapping.
    /// </summary>
    ///
    /// <typeparam name="T">
    ///     Generic type parameter.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public abstract class BaseEntityMapping<T> : EmptyEntityMapping<T>
        where T : BaseEntity
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the BaseEntityMapping class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        protected BaseEntityMapping()
        {
            this.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}
