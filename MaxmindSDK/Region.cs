using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxmindSDK
{
    public class Region
    {
        public Region()
        {
        }

        public Region(string countryCode, string countryName, string name)
        {
            this.CountryCode = countryCode;
            this.CountryName = countryName;
            this.Name = name;
        }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string Name { get; set; }
    }
}
