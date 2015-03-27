namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    internal static class FrameworkConstants
    {
        public const int BufferSize = 16384;

        public static readonly DateTime AssemblyTimeStamp = HostingEnvironment.IsSharedHost ? DateTime.UtcNow : File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location);

        public static readonly string TimeStamp = AssemblyTimeStamp.ToString("yyyyMMddhhmmss");
    }
}
