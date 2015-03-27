namespace Framework.Logging
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Security;
    using System.Security.Claims;
    using System.Web;

    using Framework.Configuration;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Log entry.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class LogEntry : ILogEntry
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the LogEntry class.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        public LogEntry(
            LogType type = LogType.Debug,
            string methodName = null,
            string sourceFile = null,
            int lineNumber = 0)
        {
            this.Type = type;

            if (this.Type == LogType.Error)
            {
                this.MethodName = methodName;

                if (!string.IsNullOrWhiteSpace(sourceFile))
                {
                    this.SourceFile = Path.GetFileName(sourceFile);
                }
                this.LineNumber = lineNumber;
            }

            this.Timestamp = DateTime.UtcNow;

            HttpContext context = HttpContext.Current;
            this.MachineName = TryGetMachineName(context);
            this.User = TryGetUserName(context);

            Config config = ConfigManager.GetConfig();
            if (config != null)
            {
                this.ApplicationName = config.Application.Name;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the LogEntry class.
        /// </summary>
        /// <param name="message">
        ///     (optional) the message.
        /// </param>
        /// <param name="type">
        ///     (optional) the type.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public LogEntry(string message = "", LogType type = LogType.Debug)
            : this(type)
        {
            this.Message = message;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the LogEntry class.
        /// </summary>
        /// <param name="message">
        ///     (optional) the message.
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
        public LogEntry(string message = "", string methodName = null, string sourceFile = null, int lineNumber = 0)
            : this(LogType.Error, methodName, sourceFile, lineNumber)
        {
            this.Message = message;
        }

        public LogEntry(Exception exception)
            : this(LogType.Error)
        {
            this.ExceptionType = exception.GetType().FullName;

            var httpException = exception as HttpException;

            Exception stackException;

            if (httpException != null)
            {
                stackException = httpException;
                this.Message = TryGetHtmlErrorMessage(httpException);
            }
            else
            {
                Exception baseException = exception.GetBaseException();

                this.Message = baseException.Message;
                stackException = baseException;
            }

            var st = new StackTrace(stackException, true);
            // Get the top stack frame
            StackFrame frame = st.GetFrame(0);
            if (frame != null)
            {
                this.LineNumber = frame.GetFileLineNumber();
                this.SourceFile = Path.GetFileName(frame.GetFileName());
                this.MethodName = frame.GetMethod().Name;
            }

            MethodBase targetSite = stackException.TargetSite;
            if (targetSite != null)
            {
                this.MethodName = targetSite.Name;
            }
        }

        public string ApplicationName { get; set; }

        public string Component { get; set; }

        public string ExceptionType { get; private set; }

        public int? LineNumber { get; private set; }

        public string MachineName { get; set; }

        public string Message { get; set; }

        public string MethodName { get; private set; }

        public string SourceFile { get; private set; }

        public DateTime Timestamp { get; private set; }

        public LogType Type { get; private set; }

        public string User { get; set; }

        private static string TryGetHtmlErrorMessage(HttpException e)
        {
            try
            {
                string message = e.GetHtmlErrorMessage();
                return string.IsNullOrWhiteSpace(message) ? e.Message : message;
            }
            catch (SecurityException)
            {
            }
            return e.Message;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try get machine name.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        private static string TryGetMachineName(HttpContext context)
        {
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

            return null;
        }

        private static string TryGetUserName(HttpContext context)
        {
            if (context != null)
            {
                ClaimsPrincipal user = ClaimsPrincipal.Current;
                if ((user != null))
                {
                    Claim claim = user.FindFirst(ClaimTypes.Email);

                    if (claim != null)
                    {
                        return claim.Value;
                    }

                    claim = user.FindFirst(ClaimTypes.NameIdentifier);

                    if (claim != null)
                    {
                        return claim.Value;
                    }

                    claim = user.FindFirst(ClaimTypes.GivenName);

                    if (claim != null)
                    {
                        return claim.Value;
                    }
                }
            }

            return null;
        }
    }
}