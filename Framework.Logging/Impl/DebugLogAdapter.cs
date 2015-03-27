namespace Framework.Logging.Impl
{
    using System.Diagnostics;
    using System.Security;

    using Framework.Ioc;

    [InjectBind(typeof(ILogAdapter), LifetimeType.Singleton)]
    [InjectBind(typeof(ILogAdapter), "DebugLog", LifetimeType.Singleton)]
    public class DebugLogAdapter : ILogAdapter
    {
        [SecurityCritical]
        public void Write(ILogEntry entry)
        {
            Debug.WriteLine(Logger.CompiledTextTemplate.Render(entry));
        }
    }
}