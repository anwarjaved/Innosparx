namespace Framework.Domain.Mapping
{
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     System log mapping.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    
    public class SystemLogMapping : EntityMapping<SystemLog>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the SystemLogMapping class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public SystemLogMapping()
        {
            this.Property(x => x.Timestamp).IsRequired();
            this.Property(x => x.MachineName).HasMaxLength(250).IsOptional();
            this.Property(x => x.Message).HasMaxLength(4000).IsRequired();
            this.Property(x => x.User).HasMaxLength(250).IsOptional();
            this.Property(x => x.Type).IsRequired();
            this.Property(x => x.ExceptionType).HasMaxLength(250).IsOptional();
            this.Property(x => x.ApplicationName).HasMaxLength(250).IsOptional();
            this.Property(x => x.SourceFile).HasMaxLength(250).IsOptional();
            this.Property(x => x.MethodName).HasMaxLength(250).IsOptional();
            this.Property(x => x.Component).HasMaxLength(250).IsOptional();
            this.Property(x => x.LineNumber).IsOptional();
        }
    }
}
