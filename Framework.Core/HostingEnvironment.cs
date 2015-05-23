namespace Framework
{
    using System;
    using System.IO;
    using System.Security;
    using System.Security.Permissions;
    using System.Web;

    /// <summary>
    /// Hosting Environment.
    /// </summary>
    public static class HostingEnvironment
    {
        
        static HostingEnvironment()
        {
            try
            {
                var permissionSet = new PermissionSet(PermissionState.None);
                permissionSet.AddPermission(new AspNetHostingPermission(AspNetHostingPermissionLevel.Unrestricted));
                IsSharedHost = !permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
            }
            catch (Exception)
            {
                IsSharedHost = true;
            }

            RootPath = System.Web.Hosting.HostingEnvironment.MapPath("/");
            IsHosted = System.Web.Hosting.HostingEnvironment.IsHosted;
        }

        /// <summary>
        /// Gets the root path.
        /// </summary>
        /// <value>The root path.</value>
        public static string RootPath { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets a value indicating whether the in medium trust.
        /// </summary>
        /// <value>
        /// true if in medium trust, false if not.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public static bool IsSharedHost { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current application domain is being hosted inside web application.
        /// </summary>
        /// <value>true if the application domain is hosted inside web application; otherwise, false.</value>
        public static bool IsHosted { get; private set; }

        /// <summary>
        /// Maps a virtual path to a physical path on the server.
        /// </summary>
        /// <param name="virtualPath">The virtual path (absolute or relative).</param>
        /// <returns>The physical path on the server specified by virtualPath.</returns>
        public static string MapPath(string virtualPath)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets absolute path relative to root.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <param name="path">
        /// Full pathname of the file.
        /// </param>
        /// <returns>
        /// The absolute path.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string GetAbsolutePath(string path)
        {
            string relativePath = path;
            while (relativePath.StartsWith(@"\") || relativePath.StartsWith("/") || relativePath.StartsWith("~"))
            {
                relativePath = relativePath.Substring(1);
            }

            return Path.Combine(HostingEnvironment.MapPath("/"), relativePath.Replace("/", @"\"));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the try get machine name.
        /// </summary>
        /// <returns>
        /// Return Machine Name else empty.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string TryGetMachineName()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                try
                {
                    return context.Server.MachineName;
                }
                catch (HttpException)
                {
                }
                catch (SecurityException)
                {
                }
            }

            try
            {
                return Environment.MachineName;
            }
            catch (SecurityException)
            {
            }

            return string.Empty;
        }
    }
}
