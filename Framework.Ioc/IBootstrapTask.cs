using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.Security;

    /// <summary>
    /// Represents a Bootstrapper Task.
    /// </summary>
    public interface IBootstrapTask
    {
        /// <summary>
        /// Executes this task.
        /// </summary>
        [SecurityCritical]
        void Execute();
    }
}
