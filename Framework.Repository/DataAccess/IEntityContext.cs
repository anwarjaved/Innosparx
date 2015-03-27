using System;

namespace Framework.DataAccess
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Security;
    using System.Threading.Tasks;

    using Framework.Domain;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for data context.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [SecuritySafeCritical]
    public interface IEntityContext
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Mark as modified.
        /// </summary>
        ///
        /// <typeparam name="TEntity">
        ///     Type of the entity.
        /// </typeparam>
        /// <param name="instance">
        ///     The instance.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void MarkAsModified<TEntity>(TEntity instance)
            where TEntity : class, IBaseEntity;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Mark as deleted.
        /// </summary>
        ///
        /// <typeparam name="TEntity">
        ///     Type of the entity.
        /// </typeparam>
        /// <param name="instance">
        ///     The instance.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void MarkAsDeleted<TEntity>(TEntity instance)
            where TEntity : class, IBaseEntity;

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
        void Detach<TEntity>(TEntity entity);

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
        Type GetEntityType<TEntity>(TEntity entity) where TEntity : IBaseEntity;

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
        IDbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves the changes.
        /// </summary>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        int SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        Task<int> SaveChangesAsync();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///      Rolls backs any pending changes.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        void Rollback();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting
        ///     unmanaged resources.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        void Dispose();

        /// <summary>
        /// Provides access to features of the context that deal with change tracking of entities.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// An object used to access features that deal with change tracking.
        /// </value>
        DbChangeTracker ChangeTracker
        {
            get;
        }

        /// <summary>
        /// Gets a <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1"/> object for the given entity providing access to
        ///                 information about the entity and the ability to perform actions on the entity.
        /// 
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam><param name="entity">The entity.</param>
        /// <returns>
        /// An entry for the entity.
        /// </returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Gets a <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry"/> object for the given entity providing access to
        ///                 information about the entity and the ability to perform actions on the entity.
        /// 
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// An entry for the entity.
        /// </returns>
        DbEntityEntry Entry(object entity);

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <remarks>
        /// If there isn't an existing local or ambient transaction a new transaction will be used
        /// to execute the command.
        /// </remarks>
        /// <param name="sql"> The command string. </param>
        /// <param name="parameters"> The parameters to apply to the command string. </param>
        /// <returns> The result returned by the database after executing the command. </returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the log.
        /// </summary>
        ///
        /// <value>
        ///     The log.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        Action<string> Log { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a raw SQL query that will return elements of the given generic type.
        ///                  The type can be any type that has properties that match the names of the
        ///                  columns returned from the query, or can be a simple primitive type.  The
        ///                  type does not have to be an entity type. The results of this query are never
        ///                  tracked by the context even if the type of object returned is an entity
        ///                  type. 
        ///     
        ///                  As with any API that accepts SQL it is important to parameterize any user
        ///                  input to protect against a SQL injection attack. You can include parameter
        ///                  place holders in the SQL query string and then supply parameter values as
        ///                  additional arguments. Any parameter values you supply will automatically be
        ///                  converted to a DbParameter. context.Database.SqlQuery&lt;Post&gt;("SELECT *
        ///                  FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor);
        ///                  Alternatively, you can also construct a DbParameter and supply it to
        ///                  SqlQuery. This allows you to use named parameters in the SQL query string.
        ///                  context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author
        ///                  = @author", new SqlParameter("@author", userSuppliedAuthor));
        /// </summary>
        ///
        /// <typeparam name="TElement">
        ///     The type of object returned by the query.
        /// </typeparam>
        /// <param name="sql">
        ///     The command string.
        /// </param>
        /// <param name="parameters">
        ///     The parameters to apply to the command string.
        /// </param>
        ///
        /// <returns>
        ///     An enumerator that allows foreach to be used to process SQL query in this collection.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        Type GetEntityType(Type type);
    }
}
