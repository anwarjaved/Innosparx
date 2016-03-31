namespace Framework.IDMembership
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Services;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Text.RegularExpressions;
    using System.Web;

    using Framework.Ioc;
    using Framework.Paging;

    public static class MembershipManager
    {
        private static IAccountPolicy accountPolicy;
        private static IPasswordStrategy passwordStrategy;

        /// <summary>
        /// Gets password strategy
        /// </summary>
        public static IPasswordStrategy PasswordStrategy
        {
            get
            {
                if (passwordStrategy == null)
                {
                    passwordStrategy = Container.Get<IPasswordStrategy>();
                    if (passwordStrategy == null)
                        throw new InvalidOperationException(
                            "You need to assign a locator to the ServiceLocator property and it should be able to lookup IPasswordStrategy.");
                }
                return passwordStrategy;
            }
        }

        /// <summary>
        /// Gets password policy
        /// </summary>
        public static IAccountPolicy AccountPolicy
        {
            get
            {
                if (accountPolicy == null)
                {
                    accountPolicy = Container.Get<IAccountPolicy>();
                    if (accountPolicy == null)
                        throw new InvalidOperationException(
                            "You need to add an IAccountPolicy implementation to your IoC container.");
                }

                return accountPolicy;
            }
        }

        public static string GenerateNewPassword()
        {
            return PasswordStrategy.GeneratePassword(AccountPolicy);
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <returns>
        /// true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.
        /// </returns>
        public static bool EnablePasswordRetrieval
        {
            get { return PasswordStrategy.IsPasswordsDecryptable && AccountPolicy.IsPasswordRetrievalEnabled; }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <returns>
        /// true if the membership provider supports password reset; otherwise, false. The default is true.
        /// </returns>
        public static bool EnablePasswordReset
        {
            get { return AccountPolicy.IsPasswordResetEnabled; }
        }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        /// The number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </returns>
        public static int MaxInvalidPasswordAttempts
        {
            get { return AccountPolicy.MaxInvalidPasswordAttempts; }
        }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        /// The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </returns>
        public static int PasswordAttemptWindow
        {
            get { return AccountPolicy.PasswordAttemptWindow; }
        }

        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <returns>
        /// The minimum length required for a password. 
        /// </returns>
        public static int MinRequiredPasswordLength
        {
            get { return AccountPolicy.PasswordMinimumLength; }
        }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>
        /// The minimum number of special characters that must be present in a valid password.
        /// </returns>
        public static int MinRequiredNonAlphanumericCharacters
        {
            get { return AccountPolicy.MinRequiredNonAlphanumericCharacters; }
        }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>
        /// A regular expression used to evaluate a password.
        /// </returns>
        public static string PasswordStrengthRegularExpression
        {
            get { return AccountPolicy.PasswordStrengthRegularExpression; }
        }

        public static IReadOnlyList<IRole> GetAllRoles()
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetAllRoles();
        }

        /// <summary>
        /// Creates the roles.
        /// </summary>
        /// <param name="roleNames">The role names.</param>
        public static void CreateRoles(params string[] roleNames)
        {
            foreach (var role in roleNames)
            {
                if (!MembershipManager.RoleExists(role))
                {
                    MembershipManager.CreateRole(role, "{0} Role".FormatString(role));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
        /// </summary>
        /// <returns>
        /// True if the role name already exists in the data source for the configured applicationName; otherwise, false.
        /// </returns>
        /// <param name="roleName">
        /// The name of the role to search for in the data source.
        /// </param>
        public static bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName", "Role name cannot be empty or null.");
            }

            if (roleName.IndexOf(',') > 0)
            {
                throw new ArgumentException("Role names cannot contain commas", "roleName");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.RoleExists(roleName);
        }

        /// <summary>
        /// Adds a new role to the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        /// <param name="description">The description.</param>
        /// <param name="throwIfRoleExists">if set to <c>true</c> [throw if role exists].</param>
        public static IRole CreateRole(string roleName, string description = "", bool throwIfRoleExists = false)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName", "Role name cannot be empty or null.");
            }

            if (roleName.Contains(","))
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            bool roleExists = RoleExists(roleName);
            if (throwIfRoleExists && roleExists)
            {
                throw new InvalidOperationException("Role name already exists.");
            }

            if (!roleExists)
            {
                if (roleName.Length > 64)
                {
                    throw new ArgumentOutOfRangeException("roleName", "Role name cannot exceed 64 characters.");
                }

                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.CreateRole(roleName, description);
            }

            return GetRole(roleName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a role.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        ///
        /// <param name="roleName">
        ///     The name of the role to create.
        /// </param>
        ///
        /// <returns>
        ///     The role.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IRole GetRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName", "Role name cannot be empty or null.");
            }

            if (roleName.Contains(","))
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (roleName.Length > 64)
            {
                throw new ArgumentOutOfRangeException("roleName", "Role name cannot exceed 64 characters.");
            }

            if (!RoleExists(roleName))
            {
                throw new InvalidOperationException("Role name not exists.");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetRole(roleName);
        }

        public static IPagedList<IRole> GetAllRoles(int pageIndex, int pageSize)
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetAllRoles(pageIndex, pageSize);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets roles for user.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        ///
        /// <returns>
        ///     The roles for user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IReadOnlyList<IRole> GetRolesForUser(string uniqueID)
        {
            if (string.IsNullOrEmpty(uniqueID))
            {
                throw new ArgumentNullException("uniqueID", "User ID cannot be empty or null.");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetRolesForUser(uniqueID);

        }

        /// <summary>
        /// Gets a collection of all the users in the database.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyList<IUser> GetAllUsers()
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetAllUsers();

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a collection of all the users in the database.
        /// </summary>
        ///
        /// <param name="pageIndex">
        ///     Zero-based index of the page.
        /// </param>
        /// <param name="pageSize">
        ///     Size of the page.
        /// </param>
        ///
        /// <returns>
        ///     all users.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IPagedList<IUser> GetAllUsers(int pageIndex, int pageSize)
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetAllUsers(pageIndex, pageSize);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Searches for the first users.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate.
        /// </param>
        ///
        /// <returns>
        ///     The found users.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IReadOnlyList<IUser> FindUsers(Expression<Func<IUser, bool>> predicate)
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.FindUsers(predicate);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Searches for the first users in role.
        /// </summary>
        ///
        /// <param name="roleName">
        ///     The name of the role to search for in the data source.
        /// </param>
        ///
        /// <returns>
        ///     The found users in role.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IReadOnlyList<IUser> FindUsersInRole(string roleName)
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.FindUsersInRole(roleName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Searches for the first users in role.
        /// </summary>
        ///
        /// <param name="roleNames">
        ///     List of names of the roles.
        /// </param>
        ///
        /// <returns>
        ///     The found users in role.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IReadOnlyList<IUser> FindUsersInRole(string[] roleNames)
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.FindUsersInRole(roleNames);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets user information from the data source based on the unique identifier for the
        ///     membership user. Provides an option to update the last-activity date/time stamp for the
        ///     user.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate.
        /// </param>
        /// <param name="userIsOnline">
        ///     true to update the last-activity date/time stamp for the user; false to return user
        ///     information without updating the last-activity date/time stamp for the user.
        /// </param>
        ///
        /// <returns>
        ///     A <see cref="IUser"/> object populated with the specified user's information from the
        ///     data source.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IUser GetUser(Expression<Func<IUser, bool>> predicate, bool userIsOnline = false) 
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetUser(predicate, userIsOnline);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets user information from the data source based on the unique identifier for the
        ///     membership user. Provides an option to update the last-activity date/time stamp for the
        ///     user.
        /// </summary>
        ///
        /// <param name="predicate">
        ///     The predicate.
        /// </param>
        /// <param name="userIsOnline">
        ///     true to update the last-activity date/time stamp for the user; false to return user
        ///     information without updating the last-activity date/time stamp for the user.
        /// </param>
        ///
        /// <returns>
        ///     A <see cref="IUser"/> object populated with the specified user's information from the
        ///     data source.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static T GetUser<T>(Expression<Func<T, bool>> predicate, bool userIsOnline = false) where T : IUser, new()
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetUser(predicate, userIsOnline);

        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uniqueID">The unique ID.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>A <see cref="IUser" /> object populated with the specified user's information from the data source.</returns>
        public static T GetUserByUniqueID<T>(string uniqueID, Expression<Func<T, bool>> predicate, bool userIsOnline = false) where T : IUser, new()
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetUserByUniqueID<T>(uniqueID, predicate, userIsOnline);
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="uniqueID">The uniqueID.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="IUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        public static IUser GetUserByUniqueID(string uniqueID, bool userIsOnline = false)
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetUserByUniqueID(uniqueID, userIsOnline);
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="uniqueID">The uniqueID.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="IUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        public static T GetUserByUniqueID<T>(string uniqueID, bool userIsOnline = false) where T : IUser, new()
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetUserByUniqueID<T>(uniqueID, userIsOnline);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Updates the user described by user.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="user">
        ///     The user.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void UpdateUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            provider.UpdateUser(user);
        }

        /// <summary>
        /// Gets a list of users in the specified role for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the users who are members of the specified role for the configured applicationName.
        /// </returns>
        /// <param name="roleName">
        /// The name of the role to get the list of users for.
        /// </param>
        public static IReadOnlyList<IUser> GetUsersInRole(string roleName)
        {
            if (roleName.Contains(","))
            {
                throw new ArgumentNullException("roleName", "Role name cannot be empty or null.");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new InvalidOperationException("Role name cannot be empty or null.");
            }

            if (!RoleExists(roleName))
            {
                throw new InvalidOperationException("Role does not exist.");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetUsersInRole(roleName);

        }

        /// <summary>
        /// Removes a role from the data source for the configured applicationName.
        /// </summary>
        /// <returns>
        /// True if the role was successfully deleted; otherwise, false.
        /// </returns>
        /// <param name="roleName">
        /// The name of the role to delete.
        /// </param>
        /// <param name="throwOnPopulatedRole">
        /// If true, throw an exception if <paramref name="roleName"/> has one or more members and do not delete <paramref name="roleName"/>.
        /// </param>
        public static void DeleteRole(string roleName, bool throwOnPopulatedRole = false)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName", "Role name cannot be empty or null.");
            }

            if (roleName.Contains(","))
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (!RoleExists(roleName))
            {
                throw new InvalidOperationException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(roleName).Any())
            {
                throw new InvalidOperationException("Cannot delete a populated role.");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            provider.DeleteRole(roleName);
        }

        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="uniqueID">The uniqueID for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>
        /// A <see cref="IUser"/> object populated with the information for the newly created user.
        /// </returns>
        public static IUser CreateUser(string uniqueID, string password)
        {
            return CreateUser(uniqueID, password, string.Empty, string.Empty, true);
        }

        public static T CreateAdmin<T>(string uniqueID, string password, params string[] roleNames) where T : IUser, new()
        {
            if (!string.IsNullOrWhiteSpace(uniqueID))
            {
                if (MembershipManager.GetUserByUniqueID(uniqueID) == null)
                {
                    MembershipManager.CreateUser<T>(
                        uniqueID,
                       password,
                        "Administrator",
                        "",
                        true);
                }

                foreach (var role in roleNames)
                {
                    if (!MembershipManager.IsUserInRole(uniqueID, role))
                    {
                        MembershipManager.AddUsersToRoles(uniqueID, role);
                    }
                }
            }

            return MembershipManager.GetUserByUniqueID<T>(uniqueID);
        }

        public static IUser CreateAdmin(string uniqueID, string password, params string[] roleNames)
        {
            if (!string.IsNullOrWhiteSpace(uniqueID))
            {
                if (MembershipManager.GetUserByUniqueID(uniqueID) == null)
                {
                    MembershipManager.CreateUser(
                        uniqueID,
                       password,
                        "Administrator",
                        "",
                        true);
                }

                foreach (var role in roleNames)
                {
                    if (!MembershipManager.IsUserInRole(uniqueID, role))
                    {
                        MembershipManager.AddUsersToRoles(uniqueID, role);
                    }
                }
            }

            return MembershipManager.GetUserByUniqueID(uniqueID);
        }

        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="uniqueID">The uniqueID for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="isVerified">Whether or not the new user is approved to be validated.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        /// A <see cref="IUser"/> object populated with the information for the newly created user.
        /// </returns>
        public static IUser CreateUser(string uniqueID, string password, string firstName, string lastName, bool isVerified, params IRole[] roles)
        {
            if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
            {
                throw new ArgumentException(
                    "The id provided is invalid. Please check the value and try again.", "uniqueID");
            }

            if (GetUserByUniqueID(uniqueID) != null)
            {
                throw new ArgumentException(
                    "A username for that id already exists. Please enter a different id.",
                    "uniqueID");
            }

            if (!Utility.ValidateParameter(ref password, true, true, false, MinRequiredPasswordLength, 32))
            {
                throw new ArgumentException(
                    "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters."
                        .FormatString(MinRequiredPasswordLength, 32),
                    "password");
            }

            if (!PasswordStrategy.IsValid(password, AccountPolicy))
            {
                throw new ArgumentException(
                    "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters."
                        .FormatString(MinRequiredPasswordLength, 32),
                    "password");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.CreateUser(uniqueID, password, firstName, lastName, isVerified, roles);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates an user.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="uniqueID">
        ///     The uniqueID address for the new user.
        /// </param>
        /// <param name="password">
        ///     The password for the new user.
        /// </param>
        /// <param name="action">
        ///     The action.
        /// </param>
        ///
        /// <returns>
        ///     The new user&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static T CreateUser<T>(string uniqueID, string password, Action<T> action)
            where T : IUser, new()
        {
            return CreateUser<T>(uniqueID, password, string.Empty, string.Empty, true, action);
        }

        public static T CreateUser<T>(string uniqueID, string password, string firstName, string lastName, bool isVerified)
           where T : IUser, new()
        {
            return CreateUser<T>(uniqueID, password, firstName, lastName, isVerified, null);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds a new membership user to the data source.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="uniqueID">
        ///     The uniqueID for the new user.
        /// </param>
        /// <param name="password">
        ///     The password for the new user.
        /// </param>
        /// <param name="firstName">
        ///     The first name.
        /// </param>
        /// <param name="lastName">
        ///     The last name.
        /// </param>
        /// <param name="isVerified">
        ///     Whether or not the new user is approved to be validated.
        /// </param>
        /// <param name="action">
        ///     The action.
        /// </param>
        /// <param name="roles">
        ///     The roles.
        /// </param>
        ///
        /// <returns>
        ///     A <see cref="IUser"/> object populated with the information for the newly created user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static T CreateUser<T>(string uniqueID, string password, string firstName, string lastName, bool isVerified, Action<T> action, params IRole[] roles)
            where T : IUser, new()
        {
            if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
            {
                throw new ArgumentException(
                    "The uniqueID provided is invalid. Please check the value and try again.", "uniqueID");
            }

            if (GetUserByUniqueID<T>(uniqueID) != null)
            {
                throw new ArgumentException(
                    "A username for that ID already exists. Please enter a different ID.",
                    "uniqueID");
            }

            if (!Utility.ValidateParameter(ref password, true, true, false, MinRequiredPasswordLength, 32))
            {
                throw new ArgumentException(
                    "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters."
                        .FormatString(MinRequiredPasswordLength, 32),
                    "password");
            }

            if (!PasswordStrategy.IsValid(password, AccountPolicy))
            {
                throw new ArgumentException(
                    "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters."
                        .FormatString(MinRequiredPasswordLength, 32),
                    "password");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.CreateUser<T>(uniqueID, password, firstName, lastName, isVerified, action, roles);
        }

        public static T CreateUser<T>(string uniqueID, string password, string firstName, string lastName, bool isVerified, Expression<Func<T, bool>> predicate, Action<T> action, params IRole[] roles)
     where T : IUser, new()
        {
            if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
            {
                throw new ArgumentException(
                    "The id provided is invalid. Please check the value and try again.", "uniqueID");
            }

            if (GetUserByUniqueID<T>(uniqueID, predicate) != null)
            {
                throw new ArgumentException(
                    "A username for that id already exists. Please enter a different id.",
                    "uniqueID");
            }

            if (!Utility.ValidateParameter(ref password, true, true, false, MinRequiredPasswordLength, 32))
            {
                throw new ArgumentException(
                    "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters."
                        .FormatString(MinRequiredPasswordLength, 32),
                    "password");
            }

            if (!PasswordStrategy.IsValid(password, AccountPolicy))
            {
                throw new ArgumentException(
                    "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters."
                        .FormatString(MinRequiredPasswordLength, 32),
                    "password");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.CreateUser<T>(uniqueID, password, firstName, lastName, isVerified, action, roles);
        }
        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="uniqueID">The uniqueID.</param>
        /// <param name="roleName">The role to search in.</param>
        /// <returns>
        /// True if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        public static bool IsUserInRole(string uniqueID, string roleName)
        {
            if (string.IsNullOrEmpty(uniqueID))
            {
                throw new ArgumentNullException("uniqueID", "User ID cannot be empty or null.");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName", "Role name cannot be empty or null.");
            }

            if (roleName.Contains(","))
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.IsUserInRole(uniqueID, roleName);

        }

        /// <summary>
        /// Gets the password for the specified uniqueID from the data source.
        /// </summary>
        /// <param name="uniqueID">The uniqueID.</param>
        /// <returns>
        /// The password for the specified uniqueID.
        /// </returns>
        public static string GetPassword(string uniqueID)
        {
            if (!AccountPolicy.IsPasswordRetrievalEnabled || !PasswordStrategy.IsPasswordsDecryptable)
                throw new InvalidOperationException("Password retrieval is not supported");

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetPassword(uniqueID);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Change password.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <exception cref="Exception">
        ///     Thrown when an exception error condition occurs.
        /// </exception>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="oldPassword">
        ///     The old password.
        /// </param>
        /// <param name="newPassword">
        ///     The new password.
        /// </param>
        /// <param name="throwException">
        ///     (optional) the throw exception.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool ChangePassword(string uniqueID, string oldPassword, string newPassword, bool throwException = false)
        {
            try
            {
                if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
                {
                    throw new ArgumentException("The id provided is invalid. Please check the value and try again.", "uniqueID");
                }


                if (GetUserByUniqueID(uniqueID) == null)
                {
                    throw new ArgumentException("A username with that ids not exists. Please check the value and try again.", "uniqueID");
                }

                if (!Utility.ValidateParameter(ref oldPassword, true, true, false, MinRequiredPasswordLength, 32))
                {
                    throw new ArgumentException(
                        "The old password provided is invalid. Please enter a valid password value of length {0}-{1} characters.".FormatString(MinRequiredPasswordLength, 32),
                        "oldPassword");
                }

                if (!Utility.ValidateParameter(ref newPassword, true, true, false, MinRequiredPasswordLength, 32))
                {
                    throw new ArgumentException(
                        "The new password provided is invalid. Please enter a valid password value of length {0}-{1} characters.".FormatString(MinRequiredPasswordLength, 32),
                        "newPassword");
                }

                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.ChangePassword(uniqueID, oldPassword, newPassword);
            }
            catch (Exception)
            {
                if (throwException)
                {
                    throw;
                }
                return false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Change password.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 05/25/2014 10:27 PM.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <exception cref="Exception">
        ///     Thrown when an exception error condition occurs.
        /// </exception>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="oldPassword">
        ///     The old password.
        /// </param>
        /// <param name="newPassword">
        ///     The new password.
        /// </param>
        /// <param name="predicate">
        ///     (Optional) The predicate.
        /// </param>
        /// <param name="throwException">
        ///     (optional) the throw exception.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool ChangePassword<T>(string uniqueID, string oldPassword, string newPassword, Expression<Func<T, bool>> predicate = null, bool throwException = false) where T : IUser, new()
        {
            try
            {
                if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
                {
                    throw new ArgumentException("The id provided is invalid. Please check the value and try again.", "uniqueID");
                }

         
                if (GetUserByUniqueID<T>(uniqueID, predicate) == null)
                {
                    throw new ArgumentException("A username with that id not exists. Please check the value and try again.", "uniqueID");
                }

                if (!Utility.ValidateParameter(ref oldPassword, true, true, false, MinRequiredPasswordLength, 32))
                {
                    throw new ArgumentException(
                        "The old password provided is invalid. Please enter a valid password value of length {0}-{1} characters.".FormatString(MinRequiredPasswordLength, 32),
                        "oldPassword");
                }

                if (!Utility.ValidateParameter(ref newPassword, true, true, false, MinRequiredPasswordLength, 32))
                {
                    throw new ArgumentException(
                        "The new password provided is invalid. Please enter a valid password value of length {0}-{1} characters.".FormatString(MinRequiredPasswordLength, 32),
                        "newPassword");
                }

                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.ChangePassword<T>(uniqueID, oldPassword, newPassword, predicate);
            }
            catch (Exception)
            {
                if (throwException)
                {
                    throw;
                }
                return false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resets a user's password to a new, automatically generated password.
        /// </summary>
        ///
        /// <exception cref="NotSupportedException">
        ///     Thrown when the requested operation is not supported.
        /// </exception>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        ///
        /// <returns>
        ///     The new password for the specified user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string ResetPassword(string uniqueID)
        {
            if (!AccountPolicy.IsPasswordResetEnabled)
            {
                throw new NotSupportedException("Password reset is not supported.");
            }

            if (Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
            {
                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.ResetPassword(uniqueID);

            }

            return string.Empty;
        }

        public static string ResetPassword<T>(string uniqueID, Expression<Func<T, bool>> predicate) where T : IUser, new()
        {
            if (!AccountPolicy.IsPasswordResetEnabled)
            {
                throw new NotSupportedException("Password reset is not supported.");
            }

            if (Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
            {
                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.ResetPassword<T>(uniqueID, predicate);

            }

            return string.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Verifies that the specified user name and password exist in the data source.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/11/2013 5:16 PM.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <exception cref="Exception">
        ///     Thrown when an exception error condition occurs.
        /// </exception>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="password">
        ///     The password for the specified user.
        /// </param>
        /// <param name="validatorCallback">
        ///     (optional) the validator callback.
        /// </param>
        /// <param name="throwException">
        ///     (optional) the throw exception.
        /// </param>
        ///
        /// <returns>
        ///     True if the specified user name and password are valid; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool ValidateUser(string uniqueID, string password, Func<IUser, bool> validatorCallback = null, bool throwException = false)
        {
            try
            {
                if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
                {
                    throw new ArgumentException("The uniqueID provided is invalid. Please check the value and try again.", "uniqueID");
                }

                if (GetUserByUniqueID(uniqueID) == null)
                {
                    throw new ArgumentException("A username with that id not exists. Please check the value and try again.", "uniqueID");
                }

                if (!Utility.ValidateParameter(ref password, true, true, false, MinRequiredPasswordLength, 32))
                {
                    throw new ArgumentException(
                        "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters.".FormatString(MinRequiredPasswordLength, 32),
                        "password");
                }

                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.ValidateUser(uniqueID, password, validatorCallback);

            }
            catch (Exception)
            {
                if (throwException)
                {
                    throw;
                }
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Verifies that the specified user name and password exist in the data source.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/11/2013 5:20 PM.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <exception cref="Exception">
        ///     Thrown when an exception error condition occurs.
        /// </exception>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="password">
        ///     The password for the specified user.
        /// </param>
        /// <param name="validatorCallback">
        ///     (optional) the validator callback.
        /// </param>
        /// <param name="throwException">
        ///     (optional) the throw exception.
        /// </param>
        ///
        /// <returns>
        ///     True if the specified user name and password are valid; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool ValidateUser<T>(string uniqueID, string password, Func<T, bool> validatorCallback = null, bool throwException = false) where T : IUser, new()
        {
            try
            {
                if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
                {
                    throw new ArgumentException("The uniqueID address provided is invalid. Please check the value and try again.", "uniqueID");
                }

                if (GetUserByUniqueID<T>(uniqueID) == null)
                {
                    throw new ArgumentException("A username with that unique id not exists. Please check the value and try again.", "uniqueID");
                }

                if (!Utility.ValidateParameter(ref password, true, true, false, MinRequiredPasswordLength, 32))
                {
                    throw new ArgumentException(
                        "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters.".FormatString(MinRequiredPasswordLength, 32),
                        "password");
                }

                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.ValidateUser(uniqueID, password, null, validatorCallback);

            }
            catch (Exception)
            {
                if (throwException)
                {
                    throw;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="uniqueID">The uniqueID.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="validatorCallback">(optional) the validator callback.</param>
        /// <param name="throwException">(optional) the throw exception.</param>
        /// <returns>True if the specified user name and password are valid; otherwise, false.</returns>
        /// <remarks>Anwar Javed, 09/11/2013 5:20 PM.</remarks>
        public static bool ValidateUser<T>(string uniqueID, string password, Expression<Func<T, bool>> predicate, Func<T, bool> validatorCallback = null, bool throwException = false) where T : IUser, new()
        {
            try
            {
                if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
                {
                    throw new ArgumentException("The unique ID provided is invalid. Please check the value and try again.", "uniqueID");
                }

                if (GetUserByUniqueID<T>(uniqueID, predicate) == null)
                {
                    throw new ArgumentException("A username with that uniqueID not exists. Please check the value and try again.", "uniqueID");
                }

                if (!Utility.ValidateParameter(ref password, true, true, false, MinRequiredPasswordLength, 32))
                {
                    throw new ArgumentException(
                        "The password provided is invalid. Please enter a valid password value of length {0}-{1} characters.".FormatString(MinRequiredPasswordLength, 32),
                        "password");
                }

                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.ValidateUser(uniqueID, password, predicate, validatorCallback);

            }
            catch (Exception)
            {
                if (throwException)
                {
                    throw;
                }
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Clears a lock so that the membership user can be validated.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="throwException">
        ///     (optional) the throw exception.
        /// </param>
        ///
        /// <returns>
        ///     true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool UnlockUser(string uniqueID, bool throwException = false)
        {
            try
            {
                if (!Utility.ValidateParameter(ref uniqueID, true, true, true, 0, 150))
                {
                    throw new ArgumentException("The unique ID provided is invalid. Please check the value and try again.", "uniqueID");
                }

                if (GetUserByUniqueID(uniqueID) == null)
                {
                    throw new ArgumentException("A username with that unique ID not exists. Please check the value and try again.", "uniqueID");
                }

                IMembershipProvider provider = Container.Get<IMembershipProvider>();

                return provider.UnlockUser(uniqueID);

            }
            catch (Exception)
            {
                if (throwException)
                {
                    throw;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
        public static int GetNumberOfUsersOnline()
        {
            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            return provider.GetNumberOfUsersOnline();
        }

        /// <summary>
        /// Adds the specified user names to the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="uniqueID">
        /// A user names to be added to the specified roles. 
        /// </param>
        /// <param name="roleNames">
        /// A string array of the role names to add the specified user names to.
        /// </param>
        public static void AddUsersToRoles(string uniqueID, params string[] roleNames)
        {
            AddUsersToRoles(new[] { uniqueID }, roleNames);
        }

        /// <summary>
        /// Adds the specified user names to the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="uniqueID">
        /// A string array of user names to be added to the specified roles. 
        /// </param>
        /// <param name="roleNames">
        /// A string array of the role names to add the specified user names to.
        /// </param>
        public static void AddUsersToRoles(string[] uniqueID, string[] roleNames)
        {
            List<string> rolesNamesList = new List<string>();
            foreach (string rolename in roleNames)
            {
                if (string.IsNullOrEmpty(rolename))
                {
                    throw new ArgumentNullException("roleNames", "Role name cannot be empty or null.");
                }

                if (!RoleExists(rolename))
                {
                    throw new InvalidOperationException("Role name not found.");
                }

                rolesNamesList.Add(rolename);
            }

            foreach (string id in uniqueID)
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException("uniqueID", "User ID cannot be empty or null.");
                }
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            provider.AddUsersToRoles(uniqueID, roleNames);
        }

        /// <summary>
        /// Removes the users from roles.
        /// </summary>
        /// <param name="uniqueID">The uniqueID.</param>
        /// <param name="roleNames">The role names.</param>
        public static void RemoveUsersFromRoles(string uniqueID, params string[] roleNames)
        {
            RemoveUsersFromRoles(new[] { uniqueID }, roleNames);
        }

        /// <summary>
        /// Removes the specified user names from the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="uniqueIDs">
        /// A string array of user names to be added to the specified roles. 
        /// </param>
        /// <param name="roleNames">
        /// A string array of role names to remove the specified user names from.
        /// </param>
        public static void RemoveUsersFromRoles(string[] uniqueIDs, string[] roleNames)
        {
            List<string> rolesNamesList = new List<string>();
            foreach (string rolename in roleNames)
            {
                if (string.IsNullOrEmpty(rolename))
                {
                    throw new ArgumentNullException("roleNames", "Role name cannot be empty or null.");
                }

                if (!RoleExists(rolename))
                {
                    throw new InvalidOperationException("Role name not found.");
                }

                rolesNamesList.Add(rolename);
            }

            foreach (string id in uniqueIDs)
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException("uniqueIDs", "User ID cannot be empty or null.");
                }
            }

            IMembershipProvider provider = Container.Get<IMembershipProvider>();

            provider.RemoveUsersFromRoles(uniqueIDs, roleNames);
      
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <returns>
        /// A <see cref="IUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        public static IUser GetCurrentUser()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (currentClaimsPrincipal != null && currentClaimsPrincipal.Identity.IsAuthenticated)
            {
                if (currentClaimsPrincipal.HasClaim(ClaimTypes.NameIdentifier))
                {
                    var claim = currentClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

                    if (claim != null)
                    {
                        string id = claim.Value;

                        if (!string.IsNullOrEmpty(id))
                        {
                            return GetUserByUniqueID(id);
                        }
                    }
                }
            }

            return null;
        }

        public static string GetCurrentUserName()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (currentClaimsPrincipal != null && currentClaimsPrincipal.Identity.IsAuthenticated)
            {
                if (currentClaimsPrincipal.HasClaim(ClaimTypes.NameIdentifier))
                {
                    var claim = currentClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

                    if (claim != null)
                    {
                        string id = claim.Value;

                        if (!string.IsNullOrEmpty(id))
                        {
                            var user = GetUserByUniqueID(id);

                            if (user != null)
                            {
                                return user.Name;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static ClaimsPrincipal GetPrincipal(string uniqueID, params Claim[] claims)
        {
            if (!string.IsNullOrWhiteSpace(uniqueID))
            {
                var account = GetUserByUniqueID(uniqueID);
                var list = new List<Claim>()
                             {
                                 new Claim(ClaimTypes.GivenName, account.Name),
                                 new Claim(ClaimTypes.Name, account.FirstName),
                                 new Claim(ClaimTypes.Surname, account.LastName),
                                 new Claim(ClaimTypes.NameIdentifier, account.UniqueID)
                             };

                list.AddRange(account.Roles.Select(r => r.Name).Select(role => new Claim(ClaimTypes.Role, role)));

                if (claims!= null && claims.Length > 0)
                {
                    list.AddRange(claims);
                }

                var identity = new ClaimsIdentity(list, MembershipConstants.AuthenticationType, ClaimTypes.NameIdentifier, ClaimTypes.Role);
                return new ClaimsPrincipal(identity);

            }


            return null;
        }

        public static ClaimsPrincipal GetPrincipal(IUser account, params Claim[] claims)
        {
            if (account != null)
            {
                var claimList = new List<Claim>()
                             {
                                 new Claim(ClaimTypes.GivenName, account.Name),
                                 new Claim(ClaimTypes.Name, account.FirstName ?? ""),
                                 new Claim(ClaimTypes.Surname, account.LastName ?? ""),
                                 new Claim(ClaimTypes.NameIdentifier, account.UniqueID)
                             };

                claimList.AddRange(account.Roles.Select(r => r.Name).Select(role => new Claim(ClaimTypes.Role, role)));

                if (claims != null && claims.Length > 0)
                {
                    claimList.AddRange(claims);
                }
                var identity = new ClaimsIdentity(claimList, MembershipConstants.AuthenticationType, ClaimTypes.NameIdentifier, ClaimTypes.Role);
                return new ClaimsPrincipal(identity);

            }


            return null;
        }


        /// <summary> 
        /// Login 
        /// </summary> 
        /// <param name="uniqueID">User ID</param> 
        /// <param name="password">Password</param> 
        /// <param name="rememberMe">True, if authentication should persist between browser sessions 
        /// </param> 
        /// <returns>True if login succeeds</returns> 
        public static bool Login(string uniqueID, string password, bool rememberMe, params Claim[] claims)
        {
            if (ValidateUser(uniqueID, password))
            {
                SetLoginCookie(uniqueID, rememberMe, claims);
                return true;
            }

            return false;
        }

        public static void SetLoginCookie(string uniqueID, bool rememberMe, params Claim[] claims)
        {
            var claimsPrincipal = GetPrincipal(uniqueID, claims);

            var sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;

            if (sessionAuthenticationModule != null)
            {
/*
                sessionAuthenticationModule.CookieHandler.Name = FormsAuthentication.FormsCookieName;
                sessionAuthenticationModule.CookieHandler.Domain = FormsAuthentication.CookieDomain;
                sessionAuthenticationModule.CookieHandler.RequireSsl = FormsAuthentication.RequireSSL;
*/

                var sst = sessionAuthenticationModule.CreateSessionSecurityToken(claimsPrincipal, MembershipConstants.AuthenticationType,

                    DateTime.UtcNow, rememberMe ? DateTime.UtcNow.AddDays(60) : DateTime.UtcNow.AddHours(1), rememberMe);

                sessionAuthenticationModule.AuthenticateSessionSecurityToken(sst, true);
            }
        }

        public static void SetLoginCookie(IUser account, bool rememberMe, params Claim[] claims)
        {
            var claimsPrincipal = GetPrincipal(account, claims);
           // claimsPrincipal.

            var sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;

            if (sessionAuthenticationModule != null)
            {
                var sst = sessionAuthenticationModule.CreateSessionSecurityToken(claimsPrincipal, MembershipConstants.AuthenticationType,

                    DateTime.UtcNow, rememberMe ? DateTime.UtcNow.AddDays(60) : DateTime.UtcNow.AddHours(1), rememberMe);

                sessionAuthenticationModule.AuthenticateSessionSecurityToken(sst, true);
            }
        }

        public static void ImpersonateUser(string uniqueID)
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (currentClaimsPrincipal != null && currentClaimsPrincipal.Identity.IsAuthenticated)
            {
                if (currentClaimsPrincipal.HasClaim(ClaimTypes.NameIdentifier))
                {
                    var claim = currentClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

                    if (claim != null)
                    {
                        string id = claim.Value;

                        MembershipManager.Logout();

                        MembershipManager.SetLoginCookie(uniqueID, true, new Claim(AppClaimTypes.PreviousUserID, id));
                    }
                }
            }
        }

        public static void Deimpersonate()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (currentClaimsPrincipal != null && currentClaimsPrincipal.Identity.IsAuthenticated)
            {
                MembershipManager.Logout();

                if (currentClaimsPrincipal.HasClaim(AppClaimTypes.PreviousUserID))
                {
                    var claim = currentClaimsPrincipal.FindFirst(AppClaimTypes.PreviousUserID);

                    if (claim != null)
                    {
                        string id = claim.Value;

                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            MembershipManager.SetLoginCookie(id, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the current user is being impersonated.
        /// </summary>
        public static bool IsImpersonating
        {
            get
            {
                ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
                if (currentClaimsPrincipal != null && currentClaimsPrincipal.Identity.IsAuthenticated)
                {
                    if (currentClaimsPrincipal.HasClaim(AppClaimTypes.PreviousUserID))
                    {
                        var claim = currentClaimsPrincipal.FindFirst(AppClaimTypes.PreviousUserID);

                        if (claim != null)
                        {
                            string id = claim.Value;

                            return !string.IsNullOrWhiteSpace(id);
                        }
                    }
                }

                return false;
            }
        }

        public static void SetLoginCookieForDomain(string cookieName, string domainName, IUser account, bool rememberMe, bool writeOnly)
        {
            var claimsPrincipal = GetPrincipal(account);

            var sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;

            if (sessionAuthenticationModule != null)
            {
                var currentDomain = sessionAuthenticationModule.CookieHandler.Domain;
                var currentCookie = sessionAuthenticationModule.CookieHandler.Name;
                sessionAuthenticationModule.CookieHandler.Name = cookieName;
                sessionAuthenticationModule.CookieHandler.Domain = domainName;
                var sst = sessionAuthenticationModule.CreateSessionSecurityToken(claimsPrincipal, MembershipConstants.AuthenticationType,

                    DateTime.UtcNow, rememberMe ? DateTime.UtcNow.AddDays(60) : DateTime.UtcNow.AddHours(1), rememberMe);

                if (!writeOnly)
                {
                    sessionAuthenticationModule.AuthenticateSessionSecurityToken(sst, true);
                   
                }
                sessionAuthenticationModule.WriteSessionTokenToCookie(sst);
                
                sessionAuthenticationModule.CookieHandler.Domain = currentDomain;
                sessionAuthenticationModule.CookieHandler.Name = cookieName;
            }
        }
 
        public static void Logout()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            if (currentClaimsPrincipal != null && currentClaimsPrincipal.Identity.IsAuthenticated)
            {
                if (FederatedAuthentication.SessionAuthenticationModule != null)
                {
                    FederatedAuthentication.SessionAuthenticationModule.SignOut();
                    FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
                }

                if (FederatedAuthentication.WSFederationAuthenticationModule != null)
                {
                    FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);
                }
            }

            var current = HttpContext.Current;
            if (current != null && current.Session != null)
            {
                current.Session.Clear();
                current.Session.Abandon();
            }
        }
    }
}
