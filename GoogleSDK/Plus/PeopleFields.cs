using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Plus
{
    using Framework;

    public enum PeopleFields
    {
        [Description("id")]
        ID,

        [Description("emails")]
        Email,

        [Description("name(familyName,givenName)")]
        Name,

        [Description("displayName")]
        DisplayName
    }
}
