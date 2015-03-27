using System;

namespace Framework.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Entity model.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class EntityModel : BaseModel
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        ///
        /// <value>
        ///     The identifier.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public Guid? ID { get; set; }

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
            return this.ID.HasValue ? ID.GetHashCode() : 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Tests if this EntityModel is considered equal to another.
        /// </summary>
        ///
        /// <param name="other">
        ///     The entity model to compare to this object.
        /// </param>
        ///
        /// <returns>
        ///     true if the objects are considered equal, false if they are not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public bool Equals(EntityModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || other.ID.Equals(ID);
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
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(EntityModel) && Equals((EntityModel)obj);
        }
    }
}
