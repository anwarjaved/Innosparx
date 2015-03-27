using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookSDK
{
    using Framework;

    public enum UserFields
    {
        /// <summary>
        ///     The user's Facebook ID.
        /// </summary>
        [Description("id")]
        ID,

        /// <summary>
        ///     The user's full name.
        /// </summary>
        [Description("name")]
        Name,

        /// <summary>
        ///     The user's first name.
        /// </summary>
        [Description("first_name")]
        FirstName,

        /// <summary>
        ///     The user's middle name.
        /// </summary>
        [Description("middle_name")]
        MiddleName,

        /// <summary>
        ///     The user's last name.
        /// </summary>
        [Description("last_name")]
        LastName,

        /// <summary>
        ///     The proxied or contact email address granted by the user.
        /// </summary>
        [Description("email")]
        Email

    }
}
