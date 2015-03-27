namespace Framework.Domain.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class ServiceEntityMapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [SecurityCritical]
    public abstract class ServiceEntityMapping<T> : ServiceEntityMapping<T, Guid>
        where T : ServiceEntity
    {
    }
}
