﻿namespace Framework.IDMembership
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Framework.Paging;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for membership provider.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    
    public interface IMembershipProvider
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets all roles.
        /// </summary>
        ///
        /// <returns>
        ///     all roles.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IRole> GetAllRoles();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Queries if a given role exists.
        /// </summary>
        ///
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool RoleExists(string roleName);

        bool RoleExists<T>(string roleName) where T : IRole, new();


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a role.
        /// </summary>
        ///
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        /// <param name="description">
        ///     The description.
        /// </param>
        ///
        /// <returns>
        ///     The new role.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IRole CreateRole(string roleName, string description);

        T CreateRole<T>(string roleName, string description) where T : IRole, new();


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a role.
        /// </summary>
        ///
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        ///
        /// <returns>
        ///     The role.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IRole GetRole(string roleName);
        T GetRole<T>(string roleName) where T : IRole, new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets all roles.
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
        ///     all roles.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IPagedList<IRole> GetAllRoles(int pageIndex, int pageSize);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets roles for user.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        ///
        /// <returns>
        ///     The roles for user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IRole> GetRolesForUser(string uniqueID);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets all users.
        /// </summary>
        ///
        /// <returns>
        ///     all users.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IUser> GetAllUsers();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets all users.
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
        IPagedList<IUser> GetAllUsers(int pageIndex, int pageSize);

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
        IReadOnlyList<IUser> FindUsers(Expression<Func<IUser, bool>> predicate);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Searches for the first users in role.
        /// </summary>
        ///
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        ///
        /// <returns>
        ///     The found users in role.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IUser> FindUsersInRole(string roleName);

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
        IReadOnlyList<IUser> FindUsersInRole(string[] roleNames);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets an user.
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
        ///     The user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IUser GetUser(Expression<Func<IUser, bool>> predicate, bool userIsOnline);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets an user.
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
        ///     The user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        T GetUser<T>(Expression<Func<T, bool>> predicate, bool userIsOnline) where T : IUser, new();


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets user by unique ID.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="userIsOnline">
        ///     true to update the last-activity date/time stamp for the user; false to return user
        ///     information without updating the last-activity date/time stamp for the user.
        /// </param>
        ///
        /// <returns>
        ///     The user by uniqueID.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IUser GetUserByUniqueID(string uniqueID, bool userIsOnline);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets user by uniqueID.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="userIsOnline">
        ///     true to update the last-activity date/time stamp for the user; false to return user
        ///     information without updating the last-activity date/time stamp for the user.
        /// </param>
        ///
        /// <returns>
        ///     The user by uniqueID.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        T GetUserByUniqueID<T>(string uniqueID, bool userIsOnline) where T : IUser, new();

        /// <summary>
        /// Gets user by uniqueID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uniqueID">The uniqueID.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user
        /// information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>The user by uniqueID.</returns>
        /// -------------------------------------------------------------------------------------------------
        /// -------------------------------------------------------------------------------------------------
        T GetUserByUniqueID<T>(string uniqueID, Expression<Func<T, bool>> predicate, bool userIsOnline) where T : IUser, new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Updates the user described by user.
        /// </summary>
        ///
        /// <param name="user">
        ///     The user.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void UpdateUser(IUser user);

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
        IUser CreateUser(string uniqueID, string password, string firstName, string lastName, bool isVerified, params IRole[] roles);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Query if 'uniqueID' is user in role.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        ///
        /// <returns>
        ///     true if user in role, false if not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool IsUserInRole(string uniqueID, string roleName);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a password.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        ///
        /// <returns>
        ///     The password.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string GetPassword(string uniqueID);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Change password.
        /// </summary>
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
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool ChangePassword(string uniqueID, string oldPassword, string newPassword);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Change password.
        /// </summary>
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
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool ChangePassword<T>(string uniqueID, string oldPassword, string newPassword, Expression<Func<T, bool>> predicate) where T : IUser, new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resets the password described by uniqueID.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string ResetPassword(string uniqueID);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resets the password described by uniqueID.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string ResetPassword<T>(string uniqueID, Expression<Func<T, bool>> predicate) where T : IUser, new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Validate user.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="password">
        ///     The password for the new user.
        /// </param>
        /// <param name="validatorCallback">
        ///     The validator Callback.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool ValidateUser(string uniqueID, string password, Func<IUser, bool> validatorCallback = null);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Validate user.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="password">
        ///     The password for the new user.
        /// </param>
        /// <param name="validatorCallback">
        ///     The validator Callback.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool ValidateUser<T>(string uniqueID, string password, Expression<Func<T, bool>> predicate = null, Func<T, bool> validatorCallback = null) where T : IUser, new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Unlocks the user.
        /// </summary>
        ///
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        bool UnlockUser(string uniqueID);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets number of users online.
        /// </summary>
        ///
        /// <returns>
        ///     The number of users online.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        int GetNumberOfUsersOnline();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds the users to roles to 'roleNames'.
        /// </summary>
        ///
        /// <param name="uniqueIDs">
        ///     The uniqueIDs.
        /// </param>
        /// <param name="roleNames">
        ///     List of names of the roles.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void AddUsersToRoles(ICollection<string> uniqueIDs, ICollection<string> roleNames);

        /// <summary>
        /// Removes the users from roles.
        /// </summary>
        /// <param name="uniqueIDs">The uniqueIDs.</param>
        /// <param name="roleNames">The role names.</param>
        void RemoveUsersFromRoles(ICollection<string> uniqueIDs, ICollection<string> roleNames);


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets users in role.
        /// </summary>
        ///
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        ///
        /// <returns>
        ///     The users in role.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        IReadOnlyList<IUser> GetUsersInRole(string roleName);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the role described by roleName.
        /// </summary>
        ///
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void DeleteRole(string roleName);
        void DeleteRole<T>(string roleName);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates an user.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="uniqueID">
        ///     The uniqueID.
        /// </param>
        /// <param name="password">
        ///     The password.
        /// </param>
        /// <param name="firstName">
        ///     The person's first name.
        /// </param>
        /// <param name="lastName">
        ///     The person's last name.
        /// </param>
        /// <param name="isVerified">
        ///     true if this object is verified.
        /// </param>
        /// <param name="action">
        ///     (optional) the action.
        /// </param>
        /// <param name="roles">
        ///     A variable-length parameters list containing roles.
        /// </param>
        ///
        /// <returns>
        ///     The new user&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        T CreateUser<T>(string uniqueID, string password, string firstName, string lastName, bool isVerified, Action<T> action = null, params IRole[] roles)
            where T : IUser, new();
    }
}
