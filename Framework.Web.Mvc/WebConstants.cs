namespace Framework
{
    using System;
    using System.Reflection;

    internal class WebConstants
    {
        public static readonly Version AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static readonly string FrameworkVersion = "Inno Sparx {0} Beta".FormatString(AssemblyVersion.ToString(2));

        public const string FilterComponent = "Action Filter";

        public const string SuppressAuthenticationKey = "MembershipAuthenticationFilterSuppress";

        public const string IoCComponent = "IoC";

        public const string TemplatePreviewMode = "TemplatePreviewMode";

        public const string AssetsFolderPath = "~/assets";
    }
}