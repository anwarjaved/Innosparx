using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Information about the password.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class PasswordInfo : IValueObject
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        ///
        /// <value>
        ///     The password.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Value { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the password salt.
        /// </summary>
        ///
        /// <value>
        ///     The password salt.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Salt { get; set; }
    }
}
