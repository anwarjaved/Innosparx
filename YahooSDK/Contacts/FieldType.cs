using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahooSDK.Contacts
{
    using Framework;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent FieldType.
    /// </summary>
    ///
    /// <remarks>
    ///     LM ANWAR, 1/5/2013.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public enum FieldType : byte
    {
        /// <summary>
        ///     Contact's GUID..
        /// </summary>
        [Description("guid")]
        Guid,

        /// <summary>
        ///     The contact's nickname.
        /// </summary>
        [Description("nickname")]
        NickName,

        /// <summary>
        ///    The contact's email.
        /// </summary>
        [Description("email")]
        Email,

        /// <summary>
        ///     Yahoo! ID.
        /// </summary>
        [Description("yahooid")]
        YahooID,

        /// <summary>
        ///     Other identifiers, such as ICQ, etc., distinguished by flags.
        /// </summary>
        [Description("otherid")]
        OtherID,

        /// <summary>
        ///     Phone number; use flags for type.
        /// </summary>
        [Description("phone")]
        Phone,

        /// <summary>
        ///    The contact's job title.
        /// </summary>
        [Description("jobtitle")]
        JobTitle,

        /// <summary>
        ///     Company name.
        /// </summary>
        [Description("company")]
        Company,

        /// <summary>
        ///     Comments about the Contact.
        /// </summary>
        [Description("notes")]
        Notes,

        /// <summary>
        ///     A URL.
        /// </summary>
        [Description("link")]
        Link,

        /// <summary>
        ///     A custom field created with Yahoo Address Book, Add a Custom Field dialog.
        /// </summary>
        [Description("custom")]
        Custom,

        /// <summary>
        ///     A name.
        /// </summary>
        [Description("name")]
        Name,

        /// <summary>
        ///     An enum constant representing the image option.
        /// </summary>
        [Description("image")]
        Image,

        /// <summary>
        ///     A postal address.
        /// </summary>
        [Description("address")]
        Address,

        /// <summary>
        ///     A birthday.
        /// </summary>
        [Description("birthday")]
        Birthday,

        /// <summary>
        ///     An anniversary.
        /// </summary>
        [Description("anniversary")]
        Anniversary,
    }
}
