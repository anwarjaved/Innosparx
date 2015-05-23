using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.DataAccess.Impl
{
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq.Expressions;
    using System.Security;
    using System.Threading.Tasks;

    using Framework.Domain;

    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private bool IsEntity { get; set; }

        private bool IsAggregateEntity { get; set; }

        public Repository(IUnitOfWork context)
        {
            this.UnitOfWork = context;
            IsEntity = typeof(TEntity).IsAssignableToGenericType(typeof(IEntity<>));
            IsAggregateEntity = typeof(TEntity).IsAssignableToGenericType(typeof(IAggregateRoot<>));

            if (IsAggregateEntity)
            {
                IsEntity = false;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public IUnitOfWork UnitOfWork { get; private set; }

        
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public virtual void Add(TEntity instance)
        {
            if (IsAggregateEntity || IsEntity)
            {
                //this.Add(instance);
            }
            else
            {
                CreateSet().Add(instance);
            }
       }

        
        public virtual void Save(TEntity instance)
        {
            IDbSet<TEntity> dbSet = CreateSet();
            if (IsAggregateEntity)
            {
                this.SaveAggregateRoot(instance, dbSet);
            }
            else if (IsEntity)
            {
                this.SaveEntity(instance, dbSet);
            }
            else
            {
                if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                {
                    dbSet.Add(instance);
                }
                else
                {
                    this.UnitOfWork.Context.MarkAsModified(instance);
                }
            }
        }

        
        private void SaveEntity(TEntity instance, IDbSet<TEntity> dbSet)
        {
            IEntity<Guid> entityWithGuid = instance as IEntity<Guid>;
            if (entityWithGuid != null)
            {
                if (entityWithGuid.ID == Guid.Empty)
                {
                    entityWithGuid.ID = Guid.NewGuid().ToCombGuid();

                    dbSet.Add(instance);
                }
                else
                {
                    if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                    {
                        //CreateSet().Attach(instance);
                    }

                    this.UnitOfWork.Context.MarkAsModified(entityWithGuid);
                }
                
                return;
            }

            IEntity<int> entityWithInt = instance as IEntity<int>;
            if (entityWithInt != null)
            {
                if (entityWithInt.ID <= 0)
                {
                    dbSet.Add(instance);
                }
                else
                {
                    if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                    {
                        //CreateSet().Attach(instance);
                    }

                    this.UnitOfWork.Context.MarkAsModified(entityWithInt);
                }

                return;
            }

            IEntity<long> entityWithLong = instance as IEntity<long>;
            if (entityWithLong != null)
            {
                if (entityWithLong.ID <= 0)
                {
                    dbSet.Add(instance);
                }
                else
                {
                    if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                    {
                        //CreateSet().Attach(instance);
                    }

                    this.UnitOfWork.Context.MarkAsModified(entityWithLong);
                }

                return;
            }

            IEntity<string> entityWithString = instance as IEntity<string>;
            if (entityWithString != null)
            {
                if (entityWithString.RowVersion == null)
                {
                    dbSet.Add(instance);
                }
                else
                {
                    if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                    {
                        //CreateSet().Attach(instance);
                    }

                    this.UnitOfWork.Context.MarkAsModified(entityWithString);
                }
            }
        }

        
        private void SaveAggregateRoot(TEntity instance, IDbSet<TEntity> dbSet)
        {
            IAggregateRoot<Guid> entityWithGuid = instance as IAggregateRoot<Guid>;

            if (entityWithGuid != null)
            {
                if (entityWithGuid.ID == Guid.Empty)
                {
                    entityWithGuid.ID = Guid.NewGuid().ToCombGuid();

                    dbSet.Add(instance);
                }
                else
                {
                    entityWithGuid.LastUpdatedDate = DateTime.UtcNow;

                    if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                    {
                        //CreateSet().Attach(instance);
                    }

                    this.UnitOfWork.Context.MarkAsModified(instance);
                }

                return;
            }

            IAggregateRoot<int> entityWithInt = instance as IAggregateRoot<int>;

            if (entityWithInt != null)
            {
                if (entityWithInt.ID <= 0)
                {
                    dbSet.Add(instance);
                }
                else
                {
                    entityWithInt.LastUpdatedDate = DateTime.UtcNow;

                    if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                    {
                        //CreateSet().Attach(instance);
                    }

                    this.UnitOfWork.Context.MarkAsModified(instance);
                }

                return;
            }

            IAggregateRoot<long> entityWithlong = instance as IAggregateRoot<long>;

            if (entityWithlong != null)
            {
                if (entityWithlong.ID <= 0)
                {
                    dbSet.Add(instance);
                }
                else
                {
                    entityWithlong.LastUpdatedDate = DateTime.UtcNow;

                    if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                    {
                        //CreateSet().Attach(instance);
                    }

                    this.UnitOfWork.Context.MarkAsModified(entityWithlong);
                }

                return;
            }

            IAggregateRoot<string> entityWithString = instance as IAggregateRoot<string>;

            if (entityWithString != null)
            {
                if (!entityWithString.LastUpdatedDate.HasValue)
                {
                    dbSet.Add(instance);
                }
                else
                {
                    entityWithString.LastUpdatedDate = DateTime.UtcNow;

                    if (this.UnitOfWork.Context.Entry(instance).State == EntityState.Detached)
                    {
                        //CreateSet().Attach(instance);
                    }

                    this.UnitOfWork.Context.MarkAsModified(entityWithString);
                }
            }
        }

        
        public virtual void Remove(TEntity instance)
        {
            if (!IsAggregateEntity)
            {
                this.UnitOfWork.Context.MarkAsDeleted(instance);
            }

            instance.Deleted = true;

            CreateSet().Remove(instance);

        }

        
        public void Remove(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity instance = this.One(predicate);

            if (!IsAggregateEntity)
            {
                this.UnitOfWork.Context.MarkAsDeleted(instance);
            }

            CreateSet().Remove(instance);
        }

        
        public virtual TEntity One(
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateIncludedSet(includes);

            return (predicate == null) ?
                   set.FirstOrDefault() :
                   set.FirstOrDefault(predicate);
        }

        
        public virtual Task<TEntity> OneAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateIncludedSet(includes);
            
            return (predicate == null) ?
                   set.FirstOrDefaultAsync() :
                   set.FirstOrDefaultAsync(predicate);
        }

/*        public IQueryable<TResult> SelectAll<TResult>(Expression<Func<TEntity, TResult>> @select, Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return predicate == null
                       ? this.CreateIncludedSet(includes).Select(@select)
                       : this.CreateIncludedSet(includes).Where(predicate).Select(@select);
        }*/

/*        public virtual TResult SelectOne<TResult>(
                        Expression<Func<TEntity, TResult>> select,
           Expression<Func<TEntity, bool>> predicate = null,
           params Expression<Func<TEntity, object>>[] includes)
        {
            return predicate == null
                       ? this.CreateIncludedSet(includes).Select(@select).FirstOrDefault()
                       : this.CreateIncludedSet(includes).Where(predicate).Select(@select).FirstOrDefault();
        }*/

        
        public virtual IQueryable<TEntity> Where(
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateIncludedSet(includes);

            return (predicate == null) ? set.AsExpandable() : set.Where(predicate).AsExpandable();
        }

        public IQueryable<TEntity> Query
        {
            
            get
            {
                return this.CreateSet().AsExpandable();
            }
        }

        
        public virtual bool Exists(
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var set = CreateSet();

            return (predicate == null) ? set.Any() : set.Any(predicate);
        }

        
        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var set = CreateSet();

            return (predicate == null) ? set.AnyAsync() : set.AnyAsync(predicate);
        }

        
        public virtual int Count(
            Expression<Func<TEntity, bool>> predicate = null)
        {
            var set = CreateSet();

            return (predicate == null) ?
                   set.Count() :
                   set.Count(predicate);
        }

        
        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var set = CreateSet();

            return (predicate == null) ?
                   set.CountAsync() :
                   set.CountAsync(predicate);
        }

        
        private IDbSet<TEntity> CreateIncludedSet(
            IEnumerable<Expression<Func<TEntity, object>>> includes)
        {
            var set = CreateSet();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    set.Include(include);
                }
            }

            return set;
        }

        
        private IDbSet<TEntity> CreateSet()
        {
            return this.UnitOfWork.Context.CreateSet<TEntity>();
        }
    }
}