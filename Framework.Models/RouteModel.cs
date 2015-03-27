namespace Framework.Models
{
    using System.Collections.Generic;

    using Framework.Knockout;

    [KnockoutModel]
    public class RouteModel : BaseModel
    {
        public ICollection<string> Methods { get; set; }

        public string Url { get; set; }
    }
}
