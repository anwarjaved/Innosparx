using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataAccess.Impl
{
    using System.Data.Entity;
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Entity configuration.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public class EntityConfiguration : DbConfiguration
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the EntityConfiguration class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public EntityConfiguration()
        {
            SetDatabaseLogFormatter(
            (context, writeAction) => new EntityLogFormatter(context, writeAction));
        }
    }
}
