namespace Framework.Knockout
{
    using System;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for knockout assembly.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class KnockoutAssemblyAttribute : Attribute
    {
    }
}
