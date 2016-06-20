namespace Framework.DataAccess
{
    using System.ComponentModel;
    using System.Security;
    using System.Threading.Tasks;

    using Framework.Domain;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for unit of work.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public interface IUnitOfWork
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the context.
        /// </summary>
        ///
        /// <value>
        ///     The context.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEntityContext Context
        {

            get;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Commits this object.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        int Commit();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Async commits this object.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        Task<int> CommitAsync();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///      Rolls backs any pending changes.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        void Rollback();

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
        EntityCollection<T> CreateCollection<T>() where T : class, IBaseEntity, new();

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
        IRepository<TEntity> Get<TEntity>() where TEntity : class, IBaseEntity;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        void Dispose();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get a new <see cref="IUnitOfWork"/> with new connextionString.
        /// </summary>
        ///
        /// <param name="nameOrConnectionString">
        ///     The name or connection string.
        /// </param>
        ///
        /// <returns>
        ///     An instance of <see cref="IUnitOfWork"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IUnitOfWork With(string nameOrConnectionString);
    }
}