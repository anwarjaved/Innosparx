using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK
{
    public static class Scopes
    {
        public const string GoogleAnalytics = "https://www.googleapis.com/auth/analytics.readonly";

        public const string GoogleContacts = "https://www.google.com/m8/feeds";

        /// <summary>
        /// For login purposes, use the profile or https://www.googleapis.com/auth/plus.login scope. The https://www.googleapis.com/auth/plus.me scope is not recommended as a login scope because, for users who have not upgraded to Google+, it does not return the user's name or email address.
        /// <br/>
        /// This scope does the following:
        /// <list type="bullet">
        /// <item><description>It lets you know who the currently authenticated user is by letting you replace a Google+ user ID with "me", which represents the authenticated user, in any call to the Google+ API.</description></item>
        /// </list>
        /// </summary>
        public const string GooglePlus = "https://www.googleapis.com/auth/plus.me";

        /// <summary>
        /// This is the recommended login scope providing access to social features. This scope implicitly includes the profile scope and also requests that your app be given access to:
        /// <list type="bullet">
        /// <item><description>the age range of the authenticated user</description></item>
        /// <item><description>the list of circled people that the user has granted your app access to know</description></item>
        /// <item><description>the methods for reading, writing and deleting app activities (moments) to Google on behalf of the user</description></item>
        /// </list>
        /// </summary>
        public const string GooglePlusLogin = "https://www.googleapis.com/auth/plus.login";

        /// <summary>
        /// This scope requests that your app be given access to:
        /// <list type="bullet">
        /// <item><description>the user's Google account email address. You access the email address by calling people.get, which returns the emails array (or by calling people.getOpenIdConnect, which returns the email property in OIDC-compliant format).</description></item>
        /// <item><description>the name of the Google Apps domain, if any, that the user belongs to. The domain name is returned as the domain property from people.get (or hd property from getOpenIdConnect).</description></item>
        /// </list>
        /// This email scope is equivalent to and replaces the <see cref="GooglePlusEmail"/> scope.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// This scope requests that your app be given access to:
        /// <list type="bullet">
        /// <item><description>the user's Google account email address, as well as any public, verified email addresses in the user's Google+ profile. You access the email addresses by calling people.get, which returns the emails array.</description></item>
        /// <item><description>the name of the Google Apps domain, if any, that the user belongs to. For more details about access to the domain name, see the email scope.</description></item>
        /// </list>
        /// </summary>
        public const string GooglePlusEmail = "https://www.googleapis.com/auth/plus.profile.emails.read";
        
        /// <summary>
        /// This scope is equivalent to and requests access to the same data as the profile scope.
        /// </summary>
        public const string GoogleProfile = "https://www.googleapis.com/auth/userinfo.profile";

        /// <summary>
        /// This scope requests access to the user's Google account email address.
        /// </summary>
        public const string GoogleUserEmail = "https://www.googleapis.com/auth/userinfo.email";

        public const string GoogleUrlShortner = "https://www.googleapis.com/auth/urlshortener";

    }
}
