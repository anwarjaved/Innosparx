using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalSDK.WebsiteStandard
{
    /// <summary>
    /// Interface IIPNProcessor
    /// </summary>
    public interface IIPNProcessor
    {
        void Process(IPNResponse response);
    }
}
