namespace Framework.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.Security;

    using Framework.Domain;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Value type mapping.
    /// </summary>
    ///
    /// <typeparam name="T">
    ///     Generic type parameter.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    
    public abstract class ValueTypeMapping<T> : ComplexTypeConfiguration<T> where T : class, IValueObject
    {
    }
}
