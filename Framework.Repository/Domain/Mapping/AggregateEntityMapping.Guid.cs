namespace Framework.Domain.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class AggregateEntityMapping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    
    public abstract class AggregateEntityMapping<T> : AggregateEntityMapping<T, Guid>
        where T : AggregateEntity
    {
    }
}
