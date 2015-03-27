using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.DataAccess.Impl
{
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    using Framework.Domain;
    using Framework.Logging;

    using Container = Framework.Ioc.Container;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Unit of work.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [SecuritySafeCritical]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class UnitOfWork : DisposableObject, IUnitOfWork
    {
        private IEntityContext context;

        private readonly string connectionString;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the UnitOfWork class.
        /// </summary>
        ///
        /// <param name="nameOrConnectionString">
        ///     The context.
        /// </param>
        /// <param name="logEnabled">
        ///     (optional) the log enabled.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public UnitOfWork(string nameOrConnectionString, bool logEnabled = false)
        {
            if (string.IsNullOrWhiteSpace(nameOrConnectionString))
            {
                var overriddenConnectionString = WebConfigurationManager.AppSettings["Framework.RepositoryContext"];
                if (!string.IsNullOrWhiteSpace(overriddenConnectionString))
                {
                    nameOrConnectionString = overriddenConnectionString;
                }

                if (string.IsNullOrWhiteSpace(nameOrConnectionString))
                {
                    nameOrConnectionString = "AppContext";
                }
            }

            this.connectionString = nameOrConnectionString;
            this.LoggingEnabled = logEnabled;
        }

        private void OnObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            using (var benchmark = Benchmark.Start())
            {
                IBaseEntity entity = e.Entity as IBaseEntity;
                var entityFormatters = Container.TryGetAll<IEntityFormatter>();

                if (entity != null)
                {
                    foreach (var formatter in entityFormatters)
                    {
                        if (formatter.OnLoad(e.Entity.GetType(), entity))
                        {
                            ((ObjectContext)sender).ObjectStateManager.GetObjectStateEntry(e.Entity).AcceptChanges();
                        }
                    }

                    entity.OnLoad(this);
                }

                benchmark.Stop();

                if (LoggingEnabled)
                {
                    Logger.Info(Logger.Completed(benchmark.TotalTime, true, "Entity Loaded: {0}".FormatString(e.Entity.GetType().Name)), RepositoryConstants.RepositoryComponent);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the context.
        /// </summary>
        ///
        /// <value>
        ///     The context.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public IEntityContext Context
        {
            [SecurityCritical]
            get
            {
                if (this.context == null)
                {
                    using (var benchmark = Benchmark.Start())
                    {
                        this.context = EntityContextFactory.CreateContext(this.connectionString);
                        ((IObjectContextAdapter)this.context).ObjectContext.ObjectMaterialized += this.OnObjectMaterialized;

                        if (LoggingEnabled)
                        {
                            this.context.Log += s => Logger.Info(s, RepositoryConstants.RepositoryComponent);
                            Logger.Info(Logger.Completed(benchmark.TotalTime, true, "new EntityContext('{0}')".FormatString(this.connectionString)), RepositoryConstants.RepositoryComponent);
                        }

                    }

                }

                return this.context;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Commits this object.
        /// </summary>
        ///
        /// <exception cref="UnitOfWorkException">
        ///     Thrown when an Unit Of Work error condition occurs.
        /// </exception>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public int Commit()
        {
            try
            {
                if (this.context == null)
                {
                    return -1;
                }
                IEnumerable<DbEntityEntry> entries = Context.ChangeTracker.Entries().ToList();
                IList<DbEntityEntry> notifyEntries = entries.Where(entry => entry.Entity is IBaseEntity).ToList();

                foreach (var entry in notifyEntries)
                {
                    IBaseEntity entity = (IBaseEntity)entry.Entity;

                    if (entity.Modified && !entity.Deleted)
                    {
                        this.Context.MarkAsModified(entity);
                    }

                    if (entity.Deleted)
                    {
                        this.Context.MarkAsDeleted(entity);
                    }

                }

                IList<DbEntityEntry> addedEntries =
                    notifyEntries.Where(
                        entry => IsState(entry, EntityState.Added) && !IsState(entry, EntityState.Deleted)).ToList();

                IList<DbEntityEntry> modifiedyEntries =
                    notifyEntries.Where(
                        entry => IsState(entry, EntityState.Modified) && !IsState(entry, EntityState.Deleted)).ToList();
                IList<DbEntityEntry> deletedEntries =
                    notifyEntries.Where(entry => IsState(entry, EntityState.Deleted)).ToList();

                var formatters = Container.TryGetAll<IEntityFormatter>();
                foreach (var entry in addedEntries)
                {
                    ((IBaseEntity)entry.Entity).OnBeforeSave(this, true);

                    foreach (var enitityFormatter in formatters)
                    {
                        enitityFormatter.OnSave(entry.Entity.GetType(), (IBaseEntity)entry.Entity);
                    }
                }

                foreach (var entry in modifiedyEntries)
                {
                    ((IBaseEntity)entry.Entity).OnBeforeSave(this, false);
                    foreach (var formatter in formatters)
                    {
                        formatter.OnSave(entry.Entity.GetType(), (IBaseEntity)entry.Entity);
                    }
                }

                if (entries.Any(HasChanged))
                {
                    int returnValue = this.Context.SaveChanges();

                    if (returnValue > 0)
                    {
                        foreach (var entry in modifiedyEntries)
                        {
                            ((IBaseEntity)entry.Entity).OnSave(this);
                        }

                        foreach (var entry in deletedEntries)
                        {
                            ((IBaseEntity)entry.Entity).OnRemove(this);
                        }

                        return returnValue;
                    }
                }
            }
            catch (DbEntityValidationException exception)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in exception.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());

                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new UnitOfWorkException(sb.ToString(), exception);

            }
            catch (UnitOfWorkException)
            {
                throw;
            }
            catch (Exception exception)
            {
                Exception innerException = exception;
                while (innerException.InnerException != null)
                    innerException = innerException.InnerException;
                throw new UnitOfWorkException(innerException.Message, exception);
            }

            return 0;
        }

        public Task<int> CommitAsync()
        {
            return Task.Run(() => this.Commit());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Async commits this object.
        /// </summary>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Task<int> IUnitOfWork.CommitAsync()
        {
            return this.CommitAsync();
        }

        public void Rollback()
        {
            if (this.context == null)
            {
                this.Context.Rollback();
 
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Rolls backs any pending changes.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        void IUnitOfWork.Rollback()
        {
            this.Rollback();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates the collection.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        ///
        /// <returns>
        ///     The new collection&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public EntityCollection<T> CreateCollection<T>() where T : class, IBaseEntity, new()
        {
            return new EntityCollection<T>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override This Method To Dispose Managed Resources.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        protected override void DisposeResources()
        {
            if (this.context != null)
            {
                this.Commit();

                ((IObjectContextAdapter)this.context).ObjectContext.ObjectMaterialized -= this.OnObjectMaterialized;
                this.context.Dispose();
                this.context = null;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the Entity Repository.
        /// </summary>
        ///
        /// <typeparam name="TEntity">
        ///     Type of the entity.
        /// </typeparam>
        ///
        /// <returns>
        ///     Entity Repository.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IRepository<TEntity> Get<TEntity>() where TEntity : class, IBaseEntity
        {
            using (var benchmark = Benchmark.Start())
            {
                Repository<TEntity> repository = new Repository<TEntity>(this);

                benchmark.Stop();

                if (LoggingEnabled)
                {
                    Logger.Info(Logger.Completed(benchmark.TotalTime, true, "Get Repository: {0}".FormatString(typeof(TEntity).Name)), RepositoryConstants.RepositoryComponent);
                }
                return repository;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get a new <see cref="IUnitOfWork"/> with new connextionString.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="nameOrConnectionString">
        ///     The context.
        /// </param>
        /// <param name="logEnabled">
        ///     (optional) the log enabled.
        /// </param>
        ///
        /// <returns>
        ///     An instance of <see cref="IUnitOfWork"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public IUnitOfWork With(string nameOrConnectionString, bool logEnabled = false)
        {
            if (string.IsNullOrWhiteSpace(nameOrConnectionString))
            {
                throw new ArgumentNullException(nameOrConnectionString);
            }

            if (!Container.Contains<IUnitOfWork>(nameOrConnectionString))
            {
                Container.Bind<IUnitOfWork>(nameOrConnectionString).ToMethod(() => new UnitOfWork(nameOrConnectionString, logEnabled));
            }

            return Container.Get<IUnitOfWork>(nameOrConnectionString);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether the log is enabled.
        /// </summary>
        ///
        /// <value>
        ///     true if log enabled, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool LoggingEnabled { get; set; }

        private static bool HasChanged(DbEntityEntry entity)
        {
            return IsState(entity, EntityState.Added) ||
                   IsState(entity, EntityState.Deleted) ||
                   IsState(entity, EntityState.Modified);
        }

        private static bool IsState(DbEntityEntry entity, EntityState state)
        {
            return (entity.State & state) == state;
        }

        //private static int Retry(Func<int> func)
        //{
        //    int count = 3;
        //    TimeSpan delay = TimeSpan.FromSeconds(2);
        //    while (true)
        //    {
        //        try
        //        {
        //            return func();
        //        }
        //        catch (SqlException e)
        //        {
        //            --count;
        //            if (count <= 0) throw;

        //            if (e.Number == 1205)
        //            {
        //                //_log.Debug("Deadlock, retrying", e);
        //            }
        //            else if (e.Number == -2)
        //            {
        //                //_log.Debug("Timeout, retrying", e);
        //            }
        //            else
        //            {
        //                throw;
        //            }

        //            Thread.Sleep(delay);
        //        }
        //    }
        //}
    }
}
