namespace Framework.Domain.Mapping
{
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     User mapping.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    
    public class UserMapping : AggregateEntityMapping<User>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the UserMapping class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public UserMapping()
        {
            this.Property(c => c.UniqueID).IsRequired().IsUnicode().HasMaxLength(256);
            this.Property(c => c.FirstName).IsRequired().IsUnicode().HasMaxLength(256);
            this.Property(c => c.LastName).IsUnicode().HasMaxLength(256);
            this.Property(c => c.IsVerified);
            this.Property(c => c.IsSuspended);
            this.Property(c => c.PasswordFailureSinceLastSuccess);
            this.Property(c => c.LastPasswordFailureDate);
            this.Property(c => c.LastActivityDate);
            this.Property(c => c.LastLockoutDate);
            this.Property(c => c.LastLoginDate);
            this.Property(c => c.IsLockedOut);
            this.Property(c => c.LastPasswordChangedDate);
            this.Property(a => a.Phone).IsUnicode().HasMaxLength(20).IsOptional();
            this.Property(a => a.CompanyName).IsUnicode().HasMaxLength(256).IsOptional();

            this.ToTable("Users");
            this.HasMany(u => u.Roles).WithMany(r => r.Users).Map(
                m => m.MapLeftKey("UserID").MapRightKey("RoleID").ToTable("UsersInRoles"));
        }
    }
}
