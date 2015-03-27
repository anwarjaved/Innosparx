using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.ComponentModel;
    using System.Globalization;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ValueTypeExtensions
    {
        public static string ToStringValue(this bool value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToStringValue(this int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToStringValue(this long value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
