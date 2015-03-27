namespace Framework.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Security;

    using Framework.Ioc;
    using Framework.Templates;

    /// <summary>
    ///     Provides support for reading or writing Logs.
    /// </summary>
    [SecuritySafeCritical]
    public static class Logger
    {
        private static readonly object SyncLock = new object();

        private static ICompiledTemplate compiledTextTemplate;

        private static string textTemplate;

        static Logger()
        {
            TextTemplate =
                "{{MachineName}}, {{Type}}, {{TimeStamp}}, {{Message}}, {{SourceFile}}, {{MethodName}}, {{LineNumber}}";
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the text template.
        /// </summary>
        /// <value>
        ///     The text template.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public static string TextTemplate
        {
            get
            {
                return textTemplate;
            }
            set
            {
                lock (SyncLock)
                {
                    textTemplate = value;

                    var templateEngine = Container.Get<ITemplateEngine>();
                    compiledTextTemplate = templateEngine.Compile(textTemplate);
                }
            }
        }

        internal static ICompiledTemplate CompiledTextTemplate
        {
            get
            {
                return compiledTextTemplate;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Completeds.
        /// </summary>
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 2:24 PM.
        /// </remarks>
        /// <param name="duration">
        ///     (optional) the duration.
        /// </param>
        /// <param name="result">
        ///     (optional) the result.
        /// </param>
        /// <param name="message">
        ///     A <see cref="String" /> to be written.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string Completed(TimeSpan? duration = null, bool result = true, string message = null)
        {
            return
                "-- Completed {0} {1} result{2}".FormatString(
                    duration.HasValue ? "in " + duration.Value.Humanize() : "",
                    result ? "with" : "without",
                    string.IsNullOrWhiteSpace(message) ? "" : ": " + message);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the diagnostic message at the <c>Debug</c> level.
        /// </summary>
        /// <param name="message">
        ///     A <see cref="string" /> to be written.
        /// </param>
        /// <param name="component">
        ///     (optional) the component.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void Debug(string message, string component = LoggingConstants.AppComponent)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                LogInternal(new LogEntry(message, LogType.Debug) { Component = component });
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the diagnostic message at the <c>Error</c> level.
        /// </summary>
        /// <param name="message">
        ///     A <see cref="string" /> to be written.
        /// </param>
        /// <param name="component">
        ///     (optional) the component.
        /// </param>
        /// <param name="methodName">
        ///     (optional) name of the method.
        /// </param>
        /// <param name="sourceFile">
        ///     (optional) source file.
        /// </param>
        /// <param name="lineNumber">
        ///     (optional) the line number.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void Error(
            string message,
            string component = LoggingConstants.AppComponent,
            [CallerMemberName] string methodName = null,
            [CallerFilePath] string sourceFile = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                LogInternal(new LogEntry(message, methodName, sourceFile, lineNumber) { Component = component });
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the diagnostic message at the <c>Error</c> level.
        /// </summary>
        /// <param name="exception">
        ///     The exception.
        /// </param>
        /// <param name="component">
        ///     (optional) the component.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void Error(Exception exception, string component = LoggingConstants.AppComponent)
        {
            if (exception != null)
            {
                LogInternal(new LogEntry(exception) { Component = component });
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executings the given message.
        /// </summary>
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 2:24 PM.
        /// </remarks>
        /// <param name="message">
        ///     A <see cref="String" /> to be written.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string Executing(string message = null)
        {
            return "-- Executing at {0}{1}".FormatString(
                DateTimeOffset.Now,
                string.IsNullOrWhiteSpace(message) ? "" : ": " + message);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executing asynchronous.
        /// </summary>
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 2:24 PM.
        /// </remarks>
        /// <param name="message">
        ///     A <see cref="String" /> to be written.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string ExecutingAsync(string message = null)
        {
            return "-- Executing asynchronously at {0}".FormatString(
                DateTimeOffset.Now,
                string.IsNullOrWhiteSpace(message) ? "" : ": " + message);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Faileds.
        /// </summary>
        /// <remarks>
        ///     Anwar Javed, 09/13/2013 2:24 PM.
        /// </remarks>
        /// <param name="duration">
        ///     (optional) the duration.
        /// </param>
        /// <param name="message">
        ///     A <see cref="String" /> to be written.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string Failed(TimeSpan? duration = null, string message = null)
        {
            return "-- Failed {0} with error{1}".FormatString(
                duration.HasValue ? "in " + duration.Value.Humanize() : "",
                string.IsNullOrWhiteSpace(message) ? "" : ": " + message);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the diagnostic message at the <c>Fatal</c> level.
        /// </summary>
        /// <param name="message">
        ///     A <see cref="string" /> to be written.
        /// </param>
        /// <param name="component">
        ///     (optional) the component.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void Fatal(string message, string component = LoggingConstants.AppComponent)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                LogInternal(new LogEntry(message, LogType.Fatal) { Component = component });
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the diagnostic message at the <c>Info</c> level.
        /// </summary>
        /// <param name="message">
        ///     A <see cref="string" /> to be written.
        /// </param>
        /// <param name="component">
        ///     (optional) the component.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void Info(string message, string component = LoggingConstants.AppComponent)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                LogInternal(new LogEntry(message, LogType.Info) { Component = component });
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Logs the given entry.
        /// </summary>
        /// <param name="entry">
        ///     The entry.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void Log(ILogEntry entry)
        {
            if (entry != null)
            {
                LogInternal(entry);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try catch log.
        /// </summary>
        /// <exception cref="Exception">
        ///     Thrown when an exception error condition occurs.
        /// </exception>
        /// <typeparam name="TResult">
        ///     Type of the result.
        /// </typeparam>
        /// <param name="f">
        ///     The Func&lt;TResult&gt; to process.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static TResult TryCatchLog<TResult>(Func<TResult> f)
        {
            try
            {
                return f();
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the diagnostic message at the <c>Warn</c> level.
        /// </summary>
        /// <param name="message">
        ///     A <see cref="String" /> to be written.
        /// </param>
        /// <param name="component">
        ///     (optional) the component.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void Warn(string message, string component = LoggingConstants.AppComponent)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                LogInternal(new LogEntry(message, LogType.Warn) { Component = component });
            }
        }

        [SecuritySafeCritical]
        private static void LogInternal(ILogEntry entry)
        {
            if (!string.IsNullOrWhiteSpace(entry.Message))
            {
                IReadOnlyList<ILogAdapter> adapters = Container.TryGetAll<ILogAdapter>();

                foreach (ILogAdapter adapter in adapters)
                {
                    adapter.Write(entry);
                }
            }
        }
    }
}