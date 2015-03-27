namespace Framework.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// REprsent a Entity with Guid as Primary Key.
    /// </summary>
    public abstract class Entity : Entity<Guid>
    {
    }
}
