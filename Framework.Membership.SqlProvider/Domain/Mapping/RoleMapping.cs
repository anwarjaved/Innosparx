namespace Framework.Domain.Mapping
{
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Role mapping.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public class RoleMapping : AggregateEntityMapping<Role>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the RoleMapping class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public RoleMapping()
        {
            this.Property(c => c.Name).IsRequired().IsUnicode().HasMaxLength(64);
            this.Property(c => c.Description).IsUnicode();
            this.ToTable("Roles");
        }
    }
}
