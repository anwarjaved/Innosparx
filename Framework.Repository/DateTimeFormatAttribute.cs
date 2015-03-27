using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeFormatAttribute : Attribute
    {
        private readonly bool isLocal;

        public DateTimeFormatAttribute(bool isLocal)
        {
            this.isLocal = isLocal;
        }

        public bool IsLocal
        {
            get
            {
                return this.isLocal;
            }
        }

        public bool IsUtc
        {
            get
            {
                return !this.IsLocal;
            }
        }
    }
}
