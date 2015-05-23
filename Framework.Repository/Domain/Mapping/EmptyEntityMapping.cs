namespace Framework.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.Security;

    using Framework.Domain;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Empty entity mapping.
    /// </summary>
    ///
    /// <typeparam name="T">
    ///     Generic type parameter.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    
    public abstract class EmptyEntityMapping<T> : EntityTypeConfiguration<T>
        where T : BaseEntity
    {
    }
}
