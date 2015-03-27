using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataAccess
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Additional information for object created events.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class ObjectCreatedEventArgs : EventArgs
    {
        internal ObjectCreatedEventArgs(object entity)
        {
            this.Entity = entity;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the entity.
        /// </summary>
        ///
        /// <value>
        ///     The entity.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public object Entity { get; private set; }
    }
}
