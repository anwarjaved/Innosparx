namespace MaxmindSDK.Domain.Mapping
{
    using System.Security;

    using Framework.Domain.Mapping;

    using MaxmindSDK.Domain;

    
    public class GeoIPCityMapping : EntityMapping<GeoIPCity>
    {
        public GeoIPCityMapping()
        {
            this.ToTable("GeoIPCities");
            this.Property(c => c.LocationID).IsRequired();
            this.Property(c => c.Country).HasMaxLength(3);
            this.Property(c => c.Region).HasMaxLength(5); ;
            this.Property(c => c.City).HasMaxLength(500); ;
            this.Property(c => c.PostalCode);
            this.Property(c => c.Location);
            this.Property(c => c.MetroCode);
            this.Property(c => c.AreaCode);
        }
    }
}
