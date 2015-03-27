using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookSDK
{
    public static class PermissionTypes
    {
        /// <summary>
        ///     Provides access to the user's primary email address in the email property.
        /// </summary>
        public const string Email = "email";
        /// <summary>
        ///     Enables your app to post content, comments, and likes to a user's stream and to the streams of the user's friends.
        /// </summary>
        public const string Publish = "publish_stream";

        /// <summary>
        ///     Enables your application to create and modify events on the user's behalf.
        /// </summary>
        public const string CreateEvent = "create_event";

        /// <summary>
        /// Provides read access to the user's friend requests.
        /// </summary>
        public const string ReadRequest = "read_requests";
    }
}
