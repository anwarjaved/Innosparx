namespace Framework.Logging
{
    using System;

    public interface ILogEntry
    {
        string ApplicationName { get; set; }

        string Component { get; set; }

        string ExceptionType { get; }

        int? LineNumber { get; }

        string MachineName { get; set; }

        string Message { get; set; }

        string MethodName { get; }

        string SourceFile { get; }

        DateTime Timestamp { get; }

        LogType Type { get; }

        string User { get; set; }
    }
}