using System;
using System.Threading.Tasks;

namespace Framework.Logging.Impl
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Threading;

    using Framework.DataAccess;
    using Framework.Domain;
    using Framework.Ioc;
    using Framework.Models;
    using Framework.Paging;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Database log adapter.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [InjectBind(typeof(ILogAdapter), "DbLog", LifetimeType.Singleton)]
    public class DbLogAdapter : DisposableObject, ILogAdapter
    {
        private readonly ConcurrentQueue<ILogEntry> entries = new ConcurrentQueue<ILogEntry>();
        private readonly ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private volatile bool shuttingDown;
        private volatile bool hasFinished;
        private volatile bool forceStop;

        public DbLogAdapter()
        {
            StartAppendTask();
        }

        private void StartAppendTask()
        {
            if (!shuttingDown)
            {
                Task appendTask = new Task(WriteInternal, TaskCreationOptions.LongRunning);
                appendTask.ContinueWith(x => WriteInternal());
                appendTask.Start();
            }
        }

        
        public void Write(ILogEntry entry)
        {
            this.entries.Enqueue(entry);
        }

        private void WriteInternal()
        {
            ILogEntry entry;
            while (!shuttingDown)
            {
                while (!this.entries.TryDequeue(out entry))
                {
                    Thread.Sleep(10);
                    if (shuttingDown)
                    {
                        break;
                    }
                }

                try
                {
                    SaveEntry(entry);
                }
                catch
                {
                }
            }

            while (this.entries.TryDequeue(out entry) && !forceStop)
            {
                try
                {
                    SaveEntry(entry);
                }
                catch
                {
                }
            }

            hasFinished = true;
            manualResetEvent.Set();
        }

        private static void SaveEntry(ILogEntry entry)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>().With(DbLogConstants.SystemLogsContext, false);
            var repository = unitOfWork.Get<SystemLog>();

            SystemLog log = new SystemLog();
            log.ApplicationName = entry.ApplicationName;
            log.ExceptionType = entry.ExceptionType;
            log.LineNumber = entry.LineNumber;
            log.MachineName = entry.MachineName;
            log.Message = entry.Message;
            log.MethodName = entry.MethodName;
            log.SourceFile = entry.SourceFile;
            log.Timestamp = entry.Timestamp;
            log.Type = entry.Type;
            log.User = entry.User;
            log.Component = entry.Component;

            repository.Save(log);
            unitOfWork.Commit();
        }

        protected override void DisposeResources()
        {
            shuttingDown = true;
            manualResetEvent.WaitOne(TimeSpan.FromSeconds(5));

            if (!hasFinished)
            {
                forceStop = true;
            }

            base.DisposeResources();
        }
    }
}
