namespace Framework.Membership
{
    using System;
    using System.Reflection;

    internal class MembershipConstants
    {
        public const string SuppressAuthenticationKey = "MembershipAuthenticationFilterSuppress";

        public static readonly Version AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static readonly string FrameworkVersion = "Inno Sparx {0} Beta".FormatString(AssemblyVersion.ToString(2));

        public static readonly string AuthenticationType = FrameworkVersion + " Membership";
    }
}