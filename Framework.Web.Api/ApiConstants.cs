namespace Framework
{
    using System;
    using System.Reflection;

    internal class ApiConstants
    {
        public static readonly Version AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static readonly string FrameworkVersion = "Inno Sparx {0} Beta".FormatString(AssemblyVersion.ToString(2));

        public const string SuppressAuthenticationKey = "MembershipAuthenticationFilterSuppress";

        public const string IoCComponent = "IoC";
    }
}