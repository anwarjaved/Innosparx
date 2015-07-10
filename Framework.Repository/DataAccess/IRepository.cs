using System;
using System.Linq;

namespace Framework.DataAccess
{
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Security;
    using System.Threading.Tasks;

    using Framework.Domain;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for repository.
    /// </summary>
    ///
    /// <typeparam name="TEntity">
    ///     Type of the entity.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    public interface IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds entity.
        /// </summary>
        ///
        /// <param name="instance">
        ///     The instance to add.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void Add(TEntity instance);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves the given entity.
        /// </summary>
        ///
        /// <param name="instance">
        ///     The instance to add.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void Save(TEntity instance);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Removes the given inentitystance.
        /// </summary>
        ///
        /// <param name="instance">
        ///     The instance to add.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void Remove(TEntity instance);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Removes the entity by given predicate.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate used to remove entity.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void Remove(Expression<Func<TEntity, bool>> predicate);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get First entity by specified expression.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate to remove.
        /// </param>
        /// <param name="includes">
        ///     A variable-length parameters list containing includes.
        /// </param>
        ///
        /// <returns>
        ///     First entity or null.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        TEntity One(
            Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);

/*        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get all entity converted to selected result.
        /// </summary>
        ///
        /// <typeparam name="TResult">
        ///     Type of the result.
        /// </typeparam>
        /// <param name="select">
        ///     The select.
        /// </param>
        /// <param name="predicate">
        ///     The predicate to select.
        /// </param>
        /// <param name="includes">
        ///     A variable-length parameters list containing includes.
        /// </param>
        ///
        /// <returns>
        ///     A List of Result specified by predicate.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IQueryable<TResult> SelectAll<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes);*/

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get all entities specified by predicate.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate to select.
        /// </param>
        /// <param name="includes">
        ///     A variable-length parameters list containing includes.
        /// </param>
        ///
        /// <returns>
        ///     A List of entities specified by predicate.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);
        

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the query.
        /// </summary>
        ///
        /// <value>
        ///     The query.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        IQueryable<TEntity> Query
        {
            
            get;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Exists the given predicate.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate to remove.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool Exists(Expression<Func<TEntity, bool>> predicate = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Counts the records by given predicate.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate to remove.
        /// </param>
        ///
        /// <returns>
        ///     Records Count.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        int Count(Expression<Func<TEntity, bool>> predicate = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a context for the scope.
        /// </summary>
        ///
        /// <value>
        ///     The scope context.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        IUnitOfWork UnitOfWork { get; }

/*        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get First entity by specified expression.
        /// </summary>
        ///
        /// <typeparam name="TResult">
        ///     Type of the result.
        /// </typeparam>
        /// <param name="select">
        ///     The select.
        /// </param>
        /// <param name="predicate">
        ///     The predicate to remove.
        /// </param>
        /// <param name="includes">
        ///     A variable-length parameters list containing includes.
        /// </param>
        ///
        /// <returns>
        ///     Converted Entity to specified Type or null.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        TResult SelectOne<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes);*/

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Get First entity by specified expression async.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate to remove.
        /// </param>
        /// <param name="includes">
        ///     A variable-length parameters list containing includes.
        /// </param>
        ///
        /// <returns>
        ///     First entity or null.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        Task<TEntity> OneAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Counts the records by given predicate async.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate to remove.
        /// </param>
        ///
        /// <returns>
        ///     Records Count.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);

        
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null);
    }
}
