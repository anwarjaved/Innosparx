namespace Framework.Domain.Mapping
{
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Location mapping.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public class LocationMapping : ValueTypeMapping<Location>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the LocationMapping class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public LocationMapping()
        {
            this.Property(a => a.Address1).IsUnicode().HasMaxLength(256).IsOptional().HasColumnName("Address1");
            this.Property(a => a.Address2).IsUnicode().HasMaxLength(256).HasColumnName("Address2");
            this.Property(a => a.City).IsUnicode().HasMaxLength(256).IsOptional().HasColumnName("City");
            this.Property(a => a.State).IsUnicode().HasMaxLength(256).IsOptional().HasColumnName("State");
            this.Property(a => a.ZipCode).IsUnicode().HasMaxLength(32).IsOptional().HasColumnName("ZipCode");
            this.Property(a => a.Country).IsUnicode().HasMaxLength(512).IsOptional().HasColumnName("Country");
            //this.Property(a => a.GeoAddress).HasColumnName("GeoAddress").IsOptional();
        }
    }
}
