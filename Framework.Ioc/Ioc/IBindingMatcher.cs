namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal interface IBindingMatcher
    {
        bool Matches(IBindingRequest request);
    }
}
