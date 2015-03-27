namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class BindingRequest : IBindingRequest
    {
        public Type Service { get; set; }

        public string Name { get; set; }
    }
}
