using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ioc
{
    internal class MvcControllerDefination
    {
        public MvcControllerDefination(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }

        public int TypePriority { get; set; }
    }
}
