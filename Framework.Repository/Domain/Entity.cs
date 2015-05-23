namespace Framework.Domain
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Entity.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        protected Entity()
        {
            Type type = typeof(T);

            if (type != typeof(string) && type != typeof(Guid) && type != typeof(int) && type != typeof(long))
            {
                throw new InvalidConstraintException("Only String, Integer & Long is supported as Entity ID");
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        ///
        /// <value>
        ///     The identifier.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public T ID { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        ///
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this.ID);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="T:System.Object" />.
        /// </summary>
        ///
        /// <param name="obj">
        ///     The object to compare with the current object.
        /// </param>
        ///
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity<T>)obj);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Tests if this Entity is considered equal to another.
        /// </summary>
        ///
        /// <param name="other">
        ///     The entity to compare to this object.
        /// </param>
        ///
        /// <returns>
        ///     true if the objects are considered equal, false if they are not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public bool Equals(Entity<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || other.ID.Equals(ID);
        }

        private string DebuggerDisplay
        {
            
            get { return string.Format("{0} ID: {1}", System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(this.GetType()), this.ID); }
        }
    }
}
