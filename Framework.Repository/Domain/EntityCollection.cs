using System;
using System.Collections.Generic;

namespace Framework.Domain
{
    using System.Collections;
    using System.Runtime.Serialization;
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Collection of entities.
    /// </summary>
    ///
    /// <typeparam name="TEntity">
    ///     Type of the entity.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    public class EntityCollection<TEntity> : ISerializable, ISet<TEntity> where TEntity : class, IBaseEntity, new()
    {
        private readonly HashSet<TEntity> entities = new HashSet<TEntity>(new EntityEqualityComparer<TEntity>());
        private bool IsEntity { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the EntityCollection class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public EntityCollection()
        {
            IsEntity = typeof(TEntity).IsAssignableToGenericType(typeof(IEntity<>));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds instance.
        /// </summary>
        ///
        /// <param name="instance">
        ///     The instance.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public bool Add(TEntity instance)
        {
            if (IsEntity)
            {
                IEntity<Guid> entity = instance as IEntity<Guid>;

                if (entity != null)
                {
                    if (entity.ID == Guid.Empty)
                    {
                        entity.ID = Guid.NewGuid().ToCombGuid();
                    }
                }
            }

            return this.entities.Add(instance);
        }

        public void UnionWith(IEnumerable<TEntity> other)
        {
            this.entities.UnionWith(other);
        }

        public void IntersectWith(IEnumerable<TEntity> other)
        {
            this.entities.IntersectWith(other);
        }

        public void ExceptWith(IEnumerable<TEntity> other)
        {
            this.entities.ExceptWith(other);
        }

        public void SymmetricExceptWith(IEnumerable<TEntity> other)
        {
            this.entities.SymmetricExceptWith(other);
        }

        public bool IsSubsetOf(IEnumerable<TEntity> other)
        {
            return this.entities.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<TEntity> other)
        {
            return this.entities.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<TEntity> other)
        {
            return this.entities.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<TEntity> other)
        {
            return this.entities.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<TEntity> other)
        {
            return this.entities.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<TEntity> other)
        {
            return this.entities.SetEquals(other);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds instance.
        /// </summary>
        ///
        /// <param name="instance">
        ///     The instance.
        /// </param>
        /// <param name="markAsChange">
        ///     true to mark as change.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public bool Add(TEntity instance, bool markAsChange)
        {
            if (markAsChange)
            {
                instance.Modified = true;
            }

            return this.entities.Add(instance);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Removes this object.
        /// </summary>
        ///
        /// <param name="instance">
        ///     The instance.
        /// </param>
        /// <param name="markAsDelete">
        ///     true to mark as delete.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public bool Remove(TEntity instance, bool markAsDelete)
        {
            if (markAsDelete)
            {
                if (entities.Remove(instance))
                {
                    instance.Deleted = true;
                    return true;
                }

                return false;
            }

            instance.Modified = true;
            return this.entities.Remove(instance);
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Clears this object to its blank/initial state.
        /// </summary>
        ///
        /// <param name="markAsDelete">
        ///     true to mark as delete.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void Clear(bool markAsDelete)
        {
            if (markAsDelete)
            {
                foreach (var instance in this)
                {
                    instance.Deleted = true;
                }

                entities.Clear();
            }
            else
            {
                entities.Clear();
            }
        }
   

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        void ICollection<TEntity>.Add(TEntity item)
        {
            this.Add(item);
        }

        void ICollection<TEntity>.Clear()
        {
            this.Clear(false);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Query if this EntityCollection contains the given item.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 10:54 AM.
        /// </remarks>
        ///
        /// <param name="item">
        ///     The item.
        /// </param>
        ///
        /// <returns>
        ///     true if the object is in this collection, false if not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public bool Contains(TEntity item)
        {
            return this.entities.Contains(item);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Copies to.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 10:54 AM.
        /// </remarks>
        ///
        /// <param name="array">
        ///     The array.
        /// </param>
        /// <param name="arrayIndex">
        ///     Zero-based index of the array.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            this.entities.CopyTo(array, arrayIndex);
        }

        bool ICollection<TEntity>.Remove(TEntity item)
        {
            return this.Remove(item, false);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the number of. 
        /// </summary>
        ///
        /// <value>
        ///     The count.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int Count
        {
            get
            {
                return entities.Count;
            }
        }

        bool ICollection<TEntity>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.entities.GetObjectData(info, context);
        }
    }
}
