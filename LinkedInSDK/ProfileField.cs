using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using System.ComponentModel;

    public enum ProfileField
    {
        /// <summary>
        /// A unique identifier token for a member
        /// </summary>
        [Description("id")]
        ID,

        /// <summary>
        /// The first name of a member.
        /// </summary>
        [Description("first-name")]
        FirstName,

        /// <summary>
        /// The last name of a member.
        /// </summary>
        [Description("last-name")]
        LastName,

        /// <summary>
        /// the member's maiden name
        /// </summary>
        [Description("maiden-name")]
        MaidenName,

        /// <summary>
        /// The headline of a member.
        /// </summary>
        [Description("headline")]
        Headline,

        /// <summary>
        /// The industry this member belongs to.
        /// </summary>
        [Description("industry")]
        Industry,

        /// <summary>
        /// The degree distance of the fetched profile from the member who fetched the profile.
        /// </summary>
        [Description("distance")]
        Distance,

        /// <summary>
        /// The current status of a member.
        /// </summary>
        [Description("current-share")]
        CurrentShare,

        /// <summary>
        /// The number of connections a member has.
        /// </summary>
        [Description("num-connections")]
        NumberOfConnections,

        /// <summary>
        /// Whether the number of connections has been capped.
        /// </summary>
        [Description("num-connections-capped")]
        NumberOfConnectionsCapped,

        /// <summary>
        /// The summary of a member.
        /// </summary>
        [Description("summary")]
        Summary,

        /// <summary>
        /// The specialties of a member.
        /// </summary>
        [Description("specialties")]
        Specialties,

        /// <summary>
        /// primary email address of user
        /// </summary>
        [Description("email-address")]
        EmailAddress,

        /// <summary>
        /// the timestamp, in milliseconds, when the member's profile was last edited
        /// </summary>
        [Description("last-modified-timestamp")]
        LastModifiedTimestamp,

        /// <summary>
        /// A short-form text area describing the member's interests
        /// </summary>
        [Description("interests")]
        Interests,

        /// <summary>
        /// A collection of education institutions a member has attended, the total indicated by a total attribute
        /// </summary>
        [Description("educations")]
        Educations,

        /// <summary>
        /// A collection of skills held by this member
        /// </summary>
        [Description("skills")]
        Skills,

        /// <summary>
        /// A collection of publications authored by this member.
        /// </summary>
        [Description("publications:(id,title,publisher:(name),authors:(id,name,person),date,url,summary)")]
        Publications,


        /// <summary>
        /// A collection of positions a member has had, the total indicated by a total attribute
        /// </summary>
        [Description("positions")]
        Positions,

        [Description("main-address")]
        MainAddress,

        [Description("phone-numbers")]
        PhoneNumbers,
    }
}
