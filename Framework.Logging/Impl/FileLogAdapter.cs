namespace Framework.Logging.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Security;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Framework.Ioc;

    [InjectBind(typeof(ILogAdapter), "FileLog", LifetimeType.Singleton)]
    public class FileLogAdapter : DisposableObject, ILogAdapter
    {
        private readonly ConcurrentQueue<ILogEntry> entries = new ConcurrentQueue<ILogEntry>();

        private readonly string logFolderName;

        private readonly ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        private readonly object syncLock = new object();

        private volatile bool forceStop;

        private volatile bool hasFinished;

        private string logFileName;

        private volatile bool shuttingDown;

        private TextWriter wrapper;

        private StreamWriter writer;

        [Inject]
        [SecuritySafeCritical]
        public FileLogAdapter()
            : this(LoggingConstants.LogFolder)
        {
        }

        [SecuritySafeCritical]
        public FileLogAdapter(string logFolderName)
        {
            this.logFolderName = logFolderName;
            this.InitLogFolder();

            this.StartAppendTask();
        }

        public void InitLogFolder()
        {
            string dataFolderPath = HostingEnvironment.IsHosted
                                        ? HostingEnvironment.MapPath(LoggingConstants.DataFolderPath)
                                        : Path.Combine(
                                            Environment.CurrentDirectory,
                                            LoggingConstants.DataFolderPath.Remove(0, 2));

            if (!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);
            }

            string logFolderPath = Path.Combine(dataFolderPath, this.logFolderName);

            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }
        }

        [SecurityCritical]
        public void Write(ILogEntry entry)
        {
            this.entries.Enqueue(entry);
        }

        protected override void DisposeResources()
        {
            this.shuttingDown = true;
            this.manualResetEvent.WaitOne(TimeSpan.FromSeconds(5));

            if (!this.hasFinished)
            {
                this.forceStop = true;
            }

            this.wrapper.Dispose();
            this.writer.Dispose();
            base.DisposeResources();
        }

        private TextWriter GetLogWriter()
        {
            string newLogFileName = DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log";

            if (this.writer == null || !newLogFileName.Equals(this.logFileName))
            {
                lock (this.syncLock)
                {
                    if (!newLogFileName.Equals(this.logFileName))
                    {
                        if (this.writer != null)
                        {
                            this.wrapper.Dispose();
                            this.writer.Dispose();
                        }
                    }

                    string dataFolderPath = HostingEnvironment.IsHosted
                                                ? HostingEnvironment.MapPath(LoggingConstants.DataFolderPath)
                                                : Path.Combine(
                                                    Environment.CurrentDirectory,
                                                    LoggingConstants.DataFolderPath.Remove(0, 2));
                    string logFilePath = Path.Combine(Path.Combine(dataFolderPath, this.logFolderName), newLogFileName);

                    this.writer =
                        new StreamWriter(
                            new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read),
                            Encoding.UTF8);
                    this.writer.AutoFlush = true;
                    this.wrapper = TextWriter.Synchronized(this.writer);
                    this.logFileName = newLogFileName;
                }
            }

            return this.wrapper;
        }

        [SecuritySafeCritical]
        private void StartAppendTask()
        {
            if (!this.shuttingDown)
            {
                var appendTask = new Task(this.WriteInternal, TaskCreationOptions.LongRunning);
                appendTask.ContinueWith(x => this.WriteInternal());
                appendTask.Start();
            }
        }

        [SecuritySafeCritical]
        private void WriteInternal()
        {
            ILogEntry entry;
            while (!this.shuttingDown)
            {
                while (!this.entries.TryDequeue(out entry))
                {
                    Thread.Sleep(10);
                    if (this.shuttingDown)
                    {
                        break;
                    }
                }

                try
                {
                    this.GetLogWriter().WriteLine(Logger.CompiledTextTemplate.Render(entry));
                }
                catch
                {
                }
            }

            while (this.entries.TryDequeue(out entry) && !this.forceStop)
            {
                try
                {
                    this.GetLogWriter().WriteLine(Logger.CompiledTextTemplate.Render(entry));
                }
                catch
                {
                }
            }

            this.hasFinished = true;
            this.manualResetEvent.Set();
        }
    }
}