namespace Framework.Ioc
{
    using System;

    internal interface IBindingRequest
    {
        Type Service { get; set; }

        string Name { get; set; }
    }
}
