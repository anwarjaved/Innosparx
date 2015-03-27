using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Models
{
    using Framework.Knockout;

    [KnockoutModel]
    public enum PagingStyle
    {
        None = 0,

        Social,

        Bootstrap,

        More
    }
}
