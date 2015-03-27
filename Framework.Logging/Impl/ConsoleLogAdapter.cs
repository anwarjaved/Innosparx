namespace Framework.Logging.Impl
{
    using System;
    using System.Security;

    using Framework.Ioc;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Debug log adapter.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [InjectBind(typeof(ILogAdapter), "ConsoleLog", LifetimeType.Singleton)]
    public class ConsoleLogAdapter : ILogAdapter
    {
        [SecurityCritical]
        public void Write(ILogEntry entry)
        {
            Console.WriteLine(Logger.CompiledTextTemplate.Render(entry));
        }
    }
}