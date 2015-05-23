namespace Framework.Domain.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class EntityMapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    
    public abstract class EntityMapping<T> : EntityMapping<T, Guid>
        where T : Entity
    {
    }
}
