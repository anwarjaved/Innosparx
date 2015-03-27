using System.Collections.Generic;

namespace Framework.Domain
{
    using System.Runtime.CompilerServices;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Entity equality comparer.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 10/21/2013 10:16 AM.
    /// </remarks>
    ///
    /// <typeparam name="TEntity">
    ///     Type of the entity.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    internal sealed class EntityEqualityComparer<TEntity> : IEqualityComparer<TEntity> where TEntity : IBaseEntity
    {
        public bool Equals(TEntity x, TEntity y)
        {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(TEntity obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
