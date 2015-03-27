namespace MaxmindSDK.Domain
{
    using System.Data.Entity.Spatial;
    using System.Security;

    using Framework.Domain;

    public class GeoIPCity : Entity
    {
        public int LocationID { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public int? PostalCode { get; set; }

        public DbGeography Location { [SecuritySafeCritical]get; [SecuritySafeCritical]set; }

        public int? MetroCode { get; set; }

        public int? AreaCode { get; set; }

    }
}
