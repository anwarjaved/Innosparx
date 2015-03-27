namespace Framework.DataAccess.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Threading.Tasks;

    using Framework.Domain;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Data context.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [SecurityCritical]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DbConfigurationType(typeof(EntityConfiguration))]
    public class EntityContext : System.Data.Entity.DbContext, IEntityContext
    {
        private readonly IDictionary<MethodInfo, object> configurations;

        private static readonly ConcurrentDictionary<Type, List<string>> Navigations = new ConcurrentDictionary<Type, List<string>>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the EntityContext class.
        /// </summary>
        /// <param name="nameOrConnectionString">
        ///     The name or connection string.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public EntityContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.configurations = EntityContextFactory.MappingConfigurations[nameOrConnectionString];
            this.Database.Log += this.WriteToLog;

            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 90;
        }

        private void WriteToLog(string message)
        {
            var action = ((IEntityContext)this).Log;
            if (action != null)
            {
                action(message);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Mark as modified.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity.
        /// </typeparam>
        /// <param name="instance">
        ///     The instance.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public virtual void MarkAsModified<TEntity>(TEntity instance)
            where TEntity : class, IBaseEntity
        {
            Entry(instance).State = EntityState.Modified;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Mark as deleted.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity.
        /// </typeparam>
        /// <param name="instance">
        ///     The instance.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public virtual void MarkAsDeleted<TEntity>(TEntity instance)
            where TEntity : class, IBaseEntity
        {
            var list = GetNavigationList(typeof(TEntity));

            foreach (string name in list)
            {
                Entry(instance).Reference(name).CurrentValue = null;
            }

            Entry(instance).State = EntityState.Deleted;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Detaches the given entity.
        /// </summary>
        ///
        /// <typeparam name="TEntity">
        ///     Type of the entity.
        /// </typeparam>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public virtual void Detach<TEntity>(TEntity entity)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a type.
        /// </summary>
        ///
        /// <typeparam name="TEntity">
        ///     Type of the entity.
        /// </typeparam>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        ///
        /// <returns>
        ///     The type&lt; t entity&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public virtual Type GetEntityType<TEntity>(TEntity entity) where TEntity : IBaseEntity
        {
            return System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(entity.GetType());
        }

        [SecuritySafeCritical]
        public virtual Type GetEntityType(Type type)
        {
            return System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(type);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates the set.
        /// </summary>
        ///
        /// <typeparam name="TEntity">
        ///     Type of the entity.
        /// </typeparam>
        ///
        /// <returns>
        ///     The new set&lt; t entity&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public virtual IDbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return Set<TEntity>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Rolls backs any pending changes.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public void Rollback()
        {
            ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        [SecuritySafeCritical]
        DbEntityEntry<TEntity> IEntityContext.Entry<TEntity>(TEntity entity)
        {
            return this.Entry(entity);
        }

        [SecuritySafeCritical]
        DbEntityEntry IEntityContext.Entry(object entity)
        {
            return this.Entry(entity);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the given DDL/DML command against the database.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 10:54 AM.
        /// </remarks>
        ///
        /// <param name="sql">
        ///     The SQL.
        /// </param>
        /// <param name="parameters">
        ///     Options for controlling the operation.
        /// </param>
        ///
        /// <returns>
        ///     The result returned by the database after executing the command.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return this.Database.ExecuteSqlCommand(sql, parameters);
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        [SecuritySafeCritical]
        int IEntityContext.ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return this.ExecuteSqlCommand(sql, parameters);
        }

        [SecuritySafeCritical]
        IEnumerable<TElement> IEntityContext.SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.SqlQuery<TElement>(sql, parameters);
        }

        Action<string> IEntityContext.Log
        {
            [SecuritySafeCritical]
            get;
            [SecuritySafeCritical]
            set;
        }

        [SecuritySafeCritical]
        int IEntityContext.SaveChanges()
        {
            return this.SaveChanges();
        }

        [SecuritySafeCritical]
        Task<int> IEntityContext.SaveChangesAsync()
        {
            return this.SaveChangesAsync();
        }

        DbChangeTracker IEntityContext.ChangeTracker
        {
            [SecuritySafeCritical]
            get
            {
                return this.ChangeTracker;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     This method is called when the model for a derived context has been initialized, but
        ///     before the model has been locked down and used to initialize the context.  The default
        ///     implementation of this method does nothing, but it can be overridden in a derived class
        ///     such that the model can be further configured before it is locked down.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="modelBuilder">
        ///     The builder that defines the model for the context being created.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            foreach (var config in configurations)
            {
                config.Key.Invoke(
                    modelBuilder.Configurations,
                    new[] { config.Value });
            }


            base.OnModelCreating(modelBuilder);
        }

        private static IEnumerable<string> GetNavigationList(Type type)
        {
            return Navigations.GetOrAdd(
                type, t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.PropertyType.IsSubclassOf(typeof(BaseEntity))).Select(x => x.Name).ToList());

        }
    }
}
