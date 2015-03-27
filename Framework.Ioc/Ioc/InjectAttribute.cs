namespace Framework.Ioc
{
    using System;

    /// <summary>
    /// Mark Constructor, Property or Method for injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
    public class InjectAttribute : Attribute
    {
    }
}
