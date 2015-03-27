namespace Framework.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SiteSetting
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public byte[] RowVersion { get; private set; }
    }
}
