namespace Framework.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represent a Service Entity.
    /// </summary>
    public abstract class ServiceEntity<T> : Entity<T>, IServiceEntity<T>
    {
    }
}
