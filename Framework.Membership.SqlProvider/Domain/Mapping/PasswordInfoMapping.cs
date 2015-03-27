namespace Framework.Domain.Mapping
{
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Password information mapping.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public class PasswordInfoMapping : ValueTypeMapping<PasswordInfo>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the PasswordInfoMapping class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public PasswordInfoMapping()
        {
            this.Property(c => c.Value).IsRequired().IsUnicode().HasMaxLength(64).HasColumnName("Password");
            this.Property(c => c.Salt).IsRequired().IsUnicode().HasMaxLength(64).HasColumnName("PasswordSalt");
        }
    }
}
