using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Rest.OAuth
{
    public class OAuthState
    {
        public string State { get; set; }

        public string SuccessUrl { get; set; }

        public string FailureUrl { get; set; }
    }
}
