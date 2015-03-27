using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ioc
{
    internal class ApiControllerDefination
    {
        public ApiControllerDefination(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }

        public int TypePriority { get; set; }
    }
}
