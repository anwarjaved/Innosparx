using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Mapping
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Service entity mapping.
    /// </summary>
    ///
    /// <typeparam name="T">
    ///     Generic type parameter.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public abstract class ServiceEntityMapping<T, V> : EntityMapping<T, V>
        where T : ServiceEntity<V>
    {
    }
}
