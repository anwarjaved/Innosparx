﻿namespace Framework.Membership.Impl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Security;
    using System.Threading.Tasks;

    using Framework.DataAccess;
    using Framework.Domain;
    using Framework.Ioc;
    using Framework.Paging;

    using Container = Framework.Ioc.Container;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Membership provider.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [SecurityCritical]
    [InjectBind(typeof(IMembershipProvider), LifetimeType.Singleton)]
    [InjectBind(typeof(IMembershipProvider), "SqlMembership", LifetimeType.Singleton)]
    public class SqlMembershipProvider : IMembershipProvider
    {
        private static readonly MethodInfo CreateUserMethodDefinition = typeof(SqlMembershipProvider).GetMethod("CreateMembershipUser", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo GetUserByEmailMethodDefinition = typeof(SqlMembershipProvider).GetMethod("GetMembershipUserByEmail", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo GetUserByEmailPredicateMethodDefinition = typeof(SqlMembershipProvider).GetMethod("GetMembershipUserByEmailPredicate", BindingFlags.Public | BindingFlags.Instance);


        private static readonly MethodInfo ChangePasswordMethodDefinition = typeof(SqlMembershipProvider).GetMethod("ChangeMembershipPassword", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo GetMembershipUserMethodDefinition = typeof(SqlMembershipProvider).GetMethod("GetMembershipUser", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo ValidateMembershipUserMethodDefinition = typeof(SqlMembershipProvider).GetMethod("ValidateMembershipUser", BindingFlags.Public | BindingFlags.Instance);

        private static readonly MethodInfo ResetMembershipPasswordMethodDefinition = typeof(SqlMembershipProvider).GetMethod("ResetMembershipPassword", BindingFlags.Public | BindingFlags.Instance);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>
        /// all roles.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public IReadOnlyList<IRole> GetAllRoles()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<Role> repository = unitOfWork.Get<Role>();

            return repository.Query.OrderBy(a => a.Name).ToList();
        }

 

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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public bool RoleExists(string roleName)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<Role> repository = unitOfWork.Get<Role>();

            return repository.Exists(a => a.Name.ToLower() == roleName.ToLower());
        }

        private static readonly MethodInfo MemebershipRoleExistsDefinition =
 typeof(SqlMembershipProvider).GetMethod("MemebershipRoleExists", BindingFlags.Public | BindingFlags.Instance);


        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public bool MemebershipRoleExists<T>(string roleName) where T : Role, new()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            return repository.Exists(a => a.Name.ToLower() == roleName.ToLower());
        }

        [SecurityCritical]
        bool IMembershipProvider.RoleExists<T>(string roleName)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(Role)))
            {
                throw new InvalidCastException("T must be type of Role");
            }

            MethodInfo method = MemebershipRoleExistsDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {
                    Expression.Constant(roleName, typeof(string))
                    
                });

            Func<SqlMembershipProvider, bool> func = Expression.Lambda<Func<SqlMembershipProvider, bool>>(callExpression, instance).Compile();
            return func(this);
        }

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
        [SecurityCritical]
        public IRole CreateRole(string roleName, string description)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<Role> repository = unitOfWork.Get<Role>();

            Role role = new Role { Name = roleName, Description = description };
            repository.Save(role);

            unitOfWork.Commit();

            return role;
        }

        private static readonly MethodInfo CreateMemebershipRoleDefinition =
typeof(SqlMembershipProvider).GetMethod("CreateMemebershipRole", BindingFlags.Public | BindingFlags.Instance);


        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public T CreateMemebershipRole<T>(string roleName, string description) where T : Role, new()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            T role = new T { Name = roleName, Description = description };
            repository.Save(role);

            unitOfWork.Commit();

            return role;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        T IMembershipProvider.CreateRole<T>(string roleName, string description)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(Role)))
            {
                throw new InvalidCastException("T must be type of Role");
            }

            MethodInfo method = CreateMemebershipRoleDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                    { Expression.Constant(roleName, typeof(string)), Expression.Constant(description, typeof(string)) });

            Func<SqlMembershipProvider, T> func = Expression.Lambda<Func<SqlMembershipProvider, T>>(callExpression, instance).Compile();
            return func(this);
        }

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
        [SecurityCritical]
        public IRole GetRole(string roleName)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<Role> repository = unitOfWork.Get<Role>();

            return repository.One(r => string.Compare(r.Name, roleName, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private static readonly MethodInfo GetMemebershipRoleDefinition =
typeof(SqlMembershipProvider).GetMethod("GetMemebershipRole", BindingFlags.Public | BindingFlags.Instance);



        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public T GetMemebershipRole<T>(string roleName) where T : Role, new()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            return repository.One(r => string.Compare(r.Name, roleName, StringComparison.OrdinalIgnoreCase) == 0);
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        T IMembershipProvider.GetRole<T>(string roleName)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(Role)))
            {
                throw new InvalidCastException("T must be type of Role");
            }

            MethodInfo method = GetMemebershipRoleDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {

                    
                });

            Func<SqlMembershipProvider, T> func = Expression.Lambda<Func<SqlMembershipProvider, T>>(callExpression, instance).Compile();
            return func(this);
        }

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
        [SecurityCritical]
        public IPagedList<IRole> GetAllRoles(int pageIndex, int pageSize)
        {

            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<Role> repository = unitOfWork.Get<Role>();

            return repository.Query.OrderBy(a => a.Name).ToPagedList<IRole>(pageIndex, pageSize);

        }

      

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets roles for user.
        /// </summary>
        ///
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        ///
        /// <param name="email">
        ///     The email.
        /// </param>
        ///
        /// <returns>
        ///     The roles for user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public IReadOnlyList<IRole> GetRolesForUser(string email)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
            IRepository<User> repository = unitOfWork.Get<User>();

            User account = repository.One(u => u.Email == email.ToLower(), u => u.Roles);

            if (account == null)
            {
                throw new InvalidOperationException("No User Exists with specified email.");
            }

            return account.Roles.ToList();
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets all users.
        /// </summary>
        ///
        /// <returns>
        ///     all users.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public IReadOnlyList<IUser> GetAllUsers()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Query.OrderBy(a => a.Email).ToList();
        }

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
        [SecurityCritical]
        public IPagedList<IUser> GetAllUsers(int pageIndex, int pageSize)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Query.OrderBy(a => a.Email).ToPagedList<IUser>(pageIndex, pageSize);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets all users.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="pageIndex">
        ///     Zero-based index of the page.
        /// </param>
        /// <param name="pageSize">
        ///     Size of the page.
        /// </param>
        ///
        /// <returns>
        ///     all users&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public IPagedList<T> GetAllUsers<T>(int pageIndex, int pageSize) where T : IUser
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Query.Cast<T>().OrderBy(a => a.Email).ToPagedList(pageIndex, pageSize);
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
        [SecurityCritical]
        public IReadOnlyList<IUser> FindUsers(Expression<Func<IUser, bool>> predicate)
        {

            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            var expression = predicate.Cast<IUser, User>();
            return repository.Where(expression).OrderBy(a => a.Email).ToList();
        }

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
        [SecurityCritical]
        public IReadOnlyList<IUser> FindUsersInRole(string roleName)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Where(u => u.Roles.Any(r => string.Compare(r.Name, roleName, StringComparison.OrdinalIgnoreCase) == 0)).OrderBy(a => a.Email).ToList();
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
        [SecurityCritical]
        public IReadOnlyList<IUser> FindUsersInRole(string[] roleNames)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Where(u => u.Roles.Any(r => roleNames.Contains(r.Name, StringComparer.OrdinalIgnoreCase))).OrderBy(a => a.Email).ToList();

        }

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
        [SecurityCritical]
        public IUser GetUser(Expression<Func<IUser, bool>> predicate, bool userIsOnline)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            var expression = predicate.Cast<IUser, User>();
            User user = repository.One(expression, u => u.Roles);

            if (userIsOnline && user != null)
            {
                user.LastActivityDate = DateTime.UtcNow;
                repository.Save(user);
                Task.Run(() => unitOfWork.CommitAsync()).ConfigureAwait(true);
            }

            return user;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets membership user.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 12:33 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="predicate">
        ///     The predicate.
        /// </param>
        /// <param name="userIsOnline">
        ///     true to update the last-activity date/time stamp for the user; false to return user
        ///     information without updating the last-activity date/time stamp for the user.
        /// </param>
        ///
        /// <returns>
        ///     The membership user&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public T GetMembershipUser<T>(Expression<Func<T, bool>> predicate, bool userIsOnline) where T : User, new()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            T user = repository.One(predicate, u => u.Roles);

            if (userIsOnline && user != null)
            {
                user.LastActivityDate = DateTime.UtcNow;
                repository.Save(user);
                Task.Run(() => unitOfWork.CommitAsync()).ConfigureAwait(true);
            }

            return user;
        }

        [SecurityCritical]
        T IMembershipProvider.GetUser<T>(Expression<Func<T, bool>> predicate, bool userIsOnline)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }


            MethodInfo method = GetMembershipUserMethodDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {
                    Expression.Constant(predicate, typeof(Expression<Func<T, bool>>)), Expression.Constant(userIsOnline, typeof(bool))
                });

            Func<SqlMembershipProvider, T> func = Expression.Lambda<Func<SqlMembershipProvider, T>>(callExpression, instance).Compile();
            return func(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets user by email.
        /// </summary>
        ///
        /// <exception cref="NotImplementedException">
        ///     Thrown when the requested operation is unimplemented.
        /// </exception>
        ///
        /// <param name="email">
        ///     The email.
        /// </param>
        /// <param name="userIsOnline">
        ///     true to update the last-activity date/time stamp for the user; false to return user
        ///     information without updating the last-activity date/time stamp for the user.
        /// </param>
        ///
        /// <returns>
        ///     The user by email.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public IUser GetUserByEmail(string email, bool userIsOnline)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            User user = repository.One(u => u.Email == email.ToLower(), u => u.Roles);

            if (userIsOnline && user != null)
            {
                user.LastActivityDate = DateTime.UtcNow;
                repository.Save(user);
                Task.Run(() => unitOfWork.CommitAsync()).ConfigureAwait(true);
            }

            return user;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets membership user by email.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 12:33 PM.
        /// </remarks>
        ///
        /// <exception cref="InvalidCastException">
        ///     Thrown when an object cannot be cast to a required type.
        /// </exception>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="email">
        ///     The email.
        /// </param>
        /// <param name="userIsOnline">
        ///     true to update the last-activity date/time stamp for the user; false to return user
        ///     information without updating the last-activity date/time stamp for the user.
        /// </param>
        ///
        /// <returns>
        ///     The membership user by email&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public T GetMembershipUserByEmail<T>(string email, bool userIsOnline) where T : User, new()
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }

            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            T user = repository.One(u => u.Email == email.ToLower(), u => u.Roles);

            if (userIsOnline && user != null)
            {
                user.LastActivityDate = DateTime.UtcNow;
                repository.Save(user);
                Task.Run(() => unitOfWork.CommitAsync()).ConfigureAwait(true);
            }

            return user;
        }

        [SecurityCritical]
        T IMembershipProvider.GetUserByEmail<T>(string email, bool userIsOnline)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }


            MethodInfo method = GetUserByEmailMethodDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {

                    Expression.Constant(email, typeof(string)), Expression.Constant(userIsOnline, typeof(bool))
                });

            Func<SqlMembershipProvider, T> func = Expression.Lambda<Func<SqlMembershipProvider, T>>(callExpression, instance).Compile();
            return func(this);
        }

        public T GetMembershipUserByEmailPredicate<T>(string email, Expression<Func<T, bool>> predicate, bool userIsOnline) where T : User, new()
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }

            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            T user = repository.Where(u => u.Email == email.ToLower(), u => u.Roles).Where(predicate).FirstOrDefault();

            if (userIsOnline && user != null)
            {
                user.LastActivityDate = DateTime.UtcNow;
                repository.Save(user);
                Task.Run(() => unitOfWork.CommitAsync()).ConfigureAwait(true);
            }

            return user;
        }

        [SecurityCritical]
        T IMembershipProvider.GetUserByEmail<T>(string email, Expression<Func<T, bool>> predicate, bool userIsOnline)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }


            MethodInfo method = GetUserByEmailPredicateMethodDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {
                    Expression.Constant(email, typeof(string)), Expression.Constant(predicate, typeof(Expression<Func<T, bool>>)), Expression.Constant(userIsOnline, typeof(bool))
                });

            Func<SqlMembershipProvider, T> func = Expression.Lambda<Func<SqlMembershipProvider, T>>(callExpression, instance).Compile();
            return func(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Updates the user described by user.
        /// </summary>
        ///
        /// <param name="user">
        ///     The user.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public void UpdateUser(IUser user)
        {
            User castUser = user as User;
            if (castUser != null)
            {
                IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
                IRepository<User> repository = unitOfWork.Get<User>();
                castUser.LastActivityDate = DateTime.UtcNow;
                repository.Save(castUser);
                unitOfWork.Commit();

            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the user described by user.
        /// </summary>
        ///
        /// <param name="user">
        ///     The user.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public void DeleteUser(IUser user)
        {
            User castUser = user as User;
            if (castUser != null)
            {
                IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
                IRepository<User> repository = unitOfWork.Get<User>();
                castUser.LastActivityDate = DateTime.UtcNow;
                repository.Remove(castUser);
                unitOfWork.Commit();

            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds a new membership user to the data source.
        /// </summary>
        ///
        /// <exception cref="NotImplementedException">
        ///     Thrown when the requested operation is unimplemented.
        /// </exception>
        ///
        /// <param name="email">
        ///     The email.
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
        /// <param name="roles">
        ///     A variable-length parameters list containing roles.
        /// </param>
        ///
        /// <returns>
        ///     A <see cref="IUser"/> object populated with the information for the newly created user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public IUser CreateUser(string email, string password, string firstName, string lastName, bool isVerified, params IRole[] roles)
        {
            AccountPasswordInfo accountPasswordInfo = new AccountPasswordInfo(email, password);
            string encryptedPassword = MembershipManager.PasswordStrategy.Encrypt(accountPasswordInfo);
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
            IRepository<User> repository = unitOfWork.Get<User>();
            User user = new User
                        {
                            Email = email,
                            Password = new PasswordInfo() { Value = encryptedPassword, Salt = accountPasswordInfo.PasswordSalt, },
                            FirstName = firstName,
                            LastName = lastName,
                            IsVerified = isVerified
                        };

            if (roles != null)
            {
                foreach (Role role in roles)
                {
                    user.Roles.Add(role, true);
                }
            }

            repository.Save(user);
            unitOfWork.Commit();
            return user;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates an user.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="email">
        ///     The email.
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public T CreateMembershipUser<T>(string email, string password, string firstName, string lastName, bool isVerified, Action<T> action = null, params IRole[] roles)
            where T : User, new()
        {
            AccountPasswordInfo accountPasswordInfo = new AccountPasswordInfo(email, password);
            string encryptedPassword = MembershipManager.PasswordStrategy.Encrypt(accountPasswordInfo);
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
            IRepository<T> repository = unitOfWork.Get<T>();
            T user = new T
            {
                Email = email,
                Password = new PasswordInfo() { Value = encryptedPassword, Salt = accountPasswordInfo.PasswordSalt, },
                FirstName = firstName,
                LastName = lastName,
                IsVerified = isVerified
            };

            if (roles != null)
            {
                foreach (Role role in roles)
                {
                    user.Roles.Add(role, true);
                }
            }

            if (action != null)
            {
                action(user);
            }

            repository.Save(user);
            unitOfWork.Commit();
            return user;
        }


        private static readonly MethodInfo DeleteMemebershipRoleDefination =
typeof(SqlMembershipProvider).GetMethod("DeleteMemebershipRole", BindingFlags.Public | BindingFlags.Instance);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public void DeleteMemebershipRole<T>(string roleName) where T : Role, new()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
            IRepository<T> repository = unitOfWork.Get<T>();

            T role = repository.One(r => string.Compare(r.Name, roleName, StringComparison.OrdinalIgnoreCase) == 0);

            repository.Remove(role);

            unitOfWork.Commit();
        }

        [SecurityCritical]
        void IMembershipProvider.DeleteRole<T>(string roleName)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(Role)))
            {
                throw new InvalidCastException("T must be type of Role");
            }

            MethodInfo method = DeleteMemebershipRoleDefination.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {
                Expression.Constant(roleName, typeof(string))
                    
                });

            Action<SqlMembershipProvider> func = Expression.Lambda<Action<SqlMembershipProvider>>(callExpression, instance).Compile();
            func(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates an user.
        /// </summary>
        ///
        /// <exception cref="InvalidCastException">
        ///     Thrown when an object cannot be cast to a required type.
        /// </exception>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="email">
        ///     The email.
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
        [SecurityCritical]
        public T CreateUser<T>(string email, string password, string firstName, string lastName, bool isVerified, Action<T> action, params IRole[] roles)
            where T : IUser, new()
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }

            MethodInfo method = CreateUserMethodDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {
                    Expression.Constant(email, typeof(string)), Expression.Constant(password, typeof(string)), Expression.Constant(firstName, typeof(string)),
                    Expression.Constant(lastName, typeof(string)), Expression.Constant(isVerified, typeof(bool)), Expression.Constant(action, typeof(Action<T>)),
                    Expression.Constant(roles, typeof(IRole[]))
                });

            Func<SqlMembershipProvider, T> func = Expression.Lambda<Func<SqlMembershipProvider, T>>(callExpression, instance).Compile();
            return func(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets all users.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        ///
        /// <returns>
        ///     all users&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public IReadOnlyList<T> GetAllUsers<T>() where T : IUser
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Query.Cast<T>().OrderBy(a => a.Email).ToList();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Query if 'email' is user in role.
        /// </summary>
        ///
        /// <param name="email">
        ///     The email.
        /// </param>
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        ///
        /// <returns>
        ///     true if user in role, false if not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public bool IsUserInRole(string email, string roleName)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Exists(u => u.Email == email.ToLower() && u.Roles.Any(x => x.Name == roleName));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a password.
        /// </summary>
        ///
        /// <param name="email">
        ///     The email.
        /// </param>
        ///
        /// <returns>
        ///     The password.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public string GetPassword(string email)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();


            var account = repository.One(u => u.Email == email.ToLower());

            return MembershipManager.PasswordStrategy.Decrypt(account.Password.Value, account.Password.Salt);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Change password.
        /// </summary>
        ///
        /// <param name="email">
        ///     The email.
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
        [SecurityCritical]
        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();


            var account = repository.One(u => u.Email == email.ToLower());

            if (account != null && !account.IsLockedOut && account.IsVerified && !account.IsSuspended)
            {
                AccountPasswordInfo accountPasswordInfo = new AccountPasswordInfo(account.Email, account.Password.Value, account.Password.Salt);
                if (MembershipManager.PasswordStrategy.Compare(accountPasswordInfo, oldPassword))
                {
                    accountPasswordInfo = new AccountPasswordInfo(account.Email, newPassword, account.Password.Salt);
                    account.LastPasswordChangedDate = DateTime.UtcNow;
                    account.Password.Value = MembershipManager.PasswordStrategy.Encrypt(accountPasswordInfo);
                    UpdateUser(account);
                    return true;
                }
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Change membership password.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 12:32 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="email">
        ///     The email.
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public bool ChangeMembershipPassword<T>(string email, string oldPassword, string newPassword, Expression<Func<T, bool>> predicate) where T : User, new()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            IQueryable<T> queryable = repository.Where(u => u.Email == email.ToLower());
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            var account = queryable.FirstOrDefault();

            if (account != null && !account.IsLockedOut && account.IsVerified && !account.IsSuspended)
            {
                AccountPasswordInfo accountPasswordInfo = new AccountPasswordInfo(account.Email, account.Password.Value, account.Password.Salt);
                if (MembershipManager.PasswordStrategy.Compare(accountPasswordInfo, oldPassword))
                {
                    accountPasswordInfo = new AccountPasswordInfo(account.Email, newPassword, account.Password.Salt);
                    account.LastPasswordChangedDate = DateTime.UtcNow;
                    account.Password.Value = MembershipManager.PasswordStrategy.Encrypt(accountPasswordInfo);
                    UpdateUser(account);
                    return true;
                }
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Change password.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 12:32 PM.
        /// </remarks>
        ///
        /// <exception cref="InvalidCastException">
        ///     Thrown when an object cannot be cast to a required type.
        /// </exception>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="email">
        ///     The email.
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
        [SecurityCritical]
        bool IMembershipProvider.ChangePassword<T>(string email, string oldPassword, string newPassword, Expression<Func<T, bool>> predicate)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }


            MethodInfo method = ChangePasswordMethodDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {
                    Expression.Constant(email, typeof(string)), 
                    Expression.Constant(oldPassword, typeof(string)),
                     Expression.Constant(newPassword, typeof(string)),
                                      Expression.Constant(predicate, typeof(Expression<Func<T, bool>>)),
                });

            Func<SqlMembershipProvider, bool> func = Expression.Lambda<Func<SqlMembershipProvider, bool>>(callExpression, instance).Compile();
            return func(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resets the password described by email.
        /// </summary>
        ///
        /// <param name="email">
        ///     The email.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public string ResetPassword(string email)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();


            var account = repository.One(u => u.Email == email.ToLower());

            if (account != null && account.IsVerified && !account.IsSuspended)
            {
                var newPassword = MembershipManager.GenerateNewPassword();
                AccountPasswordInfo accountPasswordInfo = new AccountPasswordInfo(account.Email, newPassword, account.Password.Salt);
                account.LastPasswordChangedDate = DateTime.UtcNow;
                account.Password.Value = MembershipManager.PasswordStrategy.Encrypt(accountPasswordInfo);
                UpdateUser(account);
                return newPassword;
            }

            return string.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resets the membership password described by email.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 12:32 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="email">
        ///     The email.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public string ResetMembershipPassword<T>(string email, Expression<Func<T, bool>> predicate) where T : User, new()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            IQueryable<T> queryable = repository.Where(u => u.Email == email.ToLower());

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            var account = queryable.FirstOrDefault();

            if (account != null && account.IsVerified && !account.IsSuspended)
            {
                var newPassword = MembershipManager.GenerateNewPassword();
                AccountPasswordInfo accountPasswordInfo = new AccountPasswordInfo(account.Email, newPassword, account.Password.Salt);
                account.LastPasswordChangedDate = DateTime.UtcNow;
                account.Password.Value = MembershipManager.PasswordStrategy.Encrypt(accountPasswordInfo);
                UpdateUser(account);
                return newPassword;
            }

            return string.Empty;
        }

        [SecurityCritical]
        string IMembershipProvider.ResetPassword<T>(string email, Expression<Func<T, bool>> predicate)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }


            MethodInfo method = ResetMembershipPasswordMethodDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {
                        Expression.Constant(email, typeof (string)),
                        Expression.Constant(predicate, typeof (Expression<Func<T, bool>>)),
                });

            Func<SqlMembershipProvider, string> func = Expression.Lambda<Func<SqlMembershipProvider, string>>(callExpression, instance).Compile();
            return func(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Validate user.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/11/2013 5:00 PM.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        ///
        /// <param name="email">
        ///     The email.
        /// </param>
        /// <param name="password">
        ///     The password.
        /// </param>
        /// <param name="validatorCallback">
        ///     The validator Callback.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public bool ValidateUser(string email, string password, Func<IUser, bool> validatorCallback = null)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            var account = repository.One(u => u.Email == email.ToLower());


            if (account != null && account.IsVerified && !account.IsSuspended && !account.IsLockedOut)
            {
                if (validatorCallback != null && !validatorCallback(account))
                {
                    return false;
                }

                AccountPasswordInfo accountPasswordInfo = new AccountPasswordInfo(account.Email,
                    account.Password.Value, account.Password.Salt);
                if (MembershipManager.PasswordStrategy.Compare(accountPasswordInfo, password))
                {
                    account.LastLoginDate = DateTime.UtcNow;
                    account.LastPasswordFailureDate = null;
                    account.LastLockoutDate = null;
                    account.PasswordFailureSinceLastSuccess = 0;
                    UpdateUser(account);
                    return true;
                }

                if (DateTime.UtcNow.Subtract(account.LastPasswordFailureDate.Value).TotalMinutes > MembershipManager.AccountPolicy.PasswordAttemptWindow)
                {
                    account.IsLockedOut = false;
                    account.LastPasswordFailureDate = DateTime.UtcNow;
                    account.PasswordFailureSinceLastSuccess = 0;
                    UpdateUser(account);
                }
                else
                {
                    account.PasswordFailureSinceLastSuccess += 1;
                    account.LastPasswordFailureDate = DateTime.UtcNow;

                    if (account.PasswordFailureSinceLastSuccess > MembershipManager.MaxInvalidPasswordAttempts)
                    {

                        account.IsLockedOut = true;
                        account.LastLockoutDate = DateTime.UtcNow;
                    }

                    UpdateUser(account);
                }
            }
            else
            {
                throw new ArgumentException("your account is blocked/suspended. Please contact your administrator");
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Validate membership user.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 12:32 PM.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="email">
        ///     The email.
        /// </param>
        /// <param name="password">
        ///     The password.
        /// </param>
        /// <param name="validatorCallback">
        ///     The validator Callback.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SecurityCritical]
        public bool ValidateMembershipUser<T>(string email, string password, Expression<Func<T, bool>> predicate, Func<T, bool> validatorCallback = null) where T : User, new()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<T> repository = unitOfWork.Get<T>();

            IQueryable<T> queryable = repository.Where(u => u.Email == email.ToLower());
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            var account = queryable.FirstOrDefault();


            if (account != null && account.IsVerified && !account.IsSuspended && !account.IsLockedOut)
            {
                if (validatorCallback != null && !validatorCallback(account))
                {
                    return false;
                }

                AccountPasswordInfo accountPasswordInfo = new AccountPasswordInfo(account.Email,
                    account.Password.Value, account.Password.Salt);
                if (MembershipManager.PasswordStrategy.Compare(accountPasswordInfo, password))
                {
                    account.IsLockedOut = false;
                    account.LastLoginDate = DateTime.UtcNow;
                    account.LastPasswordFailureDate = null;
                    account.LastLockoutDate = null;
                    account.PasswordFailureSinceLastSuccess = 0;
                    UpdateUser(account);
                    return true;
                }

                if (DateTime.UtcNow.Subtract(account.LastPasswordFailureDate.Value).TotalMinutes > MembershipManager.AccountPolicy.PasswordAttemptWindow)
                {
                    account.IsLockedOut = false;
                    account.LastPasswordFailureDate = DateTime.UtcNow;
                    account.PasswordFailureSinceLastSuccess = 0;
                    UpdateUser(account);
                }
                else
                {
                    account.PasswordFailureSinceLastSuccess += 1;
                    account.LastPasswordFailureDate = DateTime.UtcNow;

                    if (account.PasswordFailureSinceLastSuccess > MembershipManager.MaxInvalidPasswordAttempts)
                    {

                        account.IsLockedOut = true;
                        account.LastLockoutDate = DateTime.UtcNow;
                    }

                    UpdateUser(account);
                }

               
            }
            else
            {
                throw new ArgumentException("your account is blocked/suspended. Please contact your administrator");
            }

            return false;
        }

        [SecurityCritical]
        bool IMembershipProvider.ValidateUser<T>(string email, string password, Expression<Func<T, bool>> predicate, Func<T, bool> validatorCallback)
        {
            Type type = typeof(T);

            if (!type.IsAssignable(typeof(User)))
            {
                throw new InvalidCastException("T must be type of User");
            }


            MethodInfo method = ValidateMembershipUserMethodDefinition.MakeGenericMethod(type);

            var instance = Expression.Parameter(typeof(SqlMembershipProvider), "instance");
            MethodCallExpression callExpression = Expression.Call(
                instance,
                method,
                new Expression[]
                {
                        Expression.Constant(email, typeof(string)), Expression.Constant(password, typeof(string)),
                        Expression.Constant(predicate, typeof(Expression<Func<T, bool>>)),
                    Expression.Constant(validatorCallback, typeof(Func<T, bool>))
                });

            Func<SqlMembershipProvider, bool> func = Expression.Lambda<Func<SqlMembershipProvider, bool>>(callExpression, instance).Compile();
            return func(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Unlocks the user.
        /// </summary>
        ///
        /// <param name="email">
        ///     The email.
        /// </param>
        ///
        /// <returns>
        ///     true if it succeeds, false if it fails.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public bool UnlockUser(string email)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();


            var account = repository.One(u => u.Email == email.ToLower());
            if (account != null)
            {
                account.IsLockedOut = false;
                account.IsSuspended = false;
                account.LastPasswordFailureDate = null;
                account.LastLockoutDate = null;
                account.PasswordFailureSinceLastSuccess = 0;
                UpdateUser(account);
                return true;
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets number of users online.
        /// </summary>
        ///
        /// <returns>
        ///     The number of users online.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public int GetNumberOfUsersOnline()
        {

            TimeSpan onlineSpan = TimeSpan.FromMinutes(15);
            DateTime compareTime = DateTime.UtcNow.Subtract(onlineSpan);

            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Count(a => a.LastActivityDate > compareTime);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds the users to roles to 'roleNames'.
        /// </summary>
        ///
        /// <param name="emails">
        ///     The emails.
        /// </param>
        /// <param name="roleNames">
        ///     List of names of the roles.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public void AddUsersToRoles(ICollection<string> emails, ICollection<string> roleNames)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
            IRepository<User> repository = unitOfWork.Get<User>();
            IRepository<Role> roleRepository = unitOfWork.Get<Role>();

            var roles = roleRepository.Where(r => roleNames.Contains(r.Name));
            foreach (string email in emails)
            {
                string userEmail = email;
                User account = repository.One(u => u.Email == userEmail.ToLower(), u => u.Roles);

                foreach (Role role in roles)
                {
                    if (!account.IsInRole(role.Name))
                    {
                        account.Roles.Add(role, true);
                    }
                }
            }

            unitOfWork.Commit();
        }

        /// <summary>
        /// Removes the users from roles.
        /// </summary>
        /// <param name="emails">The emails.</param>
        /// <param name="roleNames">The role names.</param>
        [SecurityCritical]
        public void RemoveUsersFromRoles(ICollection<string> emails, ICollection<string> roleNames)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
            IRepository<User> repository = unitOfWork.Get<User>();
            IRepository<Role> roleRepository = unitOfWork.Get<Role>();

            var roles = roleRepository.Where(r => roleNames.Contains(r.Name));
            foreach (string email in emails)
            {
                string userEmail = email;
                User account = repository.One(u => u.Email == userEmail.ToLower(), u => u.Roles);

                foreach (Role role in roles)
                {
                    if (account.IsInRole(role.Name))
                    {
                        account.Roles.Remove(role, false);
                    }
                }
            }

            unitOfWork.Commit();
        }

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
        [SecurityCritical]
        public IReadOnlyList<IUser> GetUsersInRole(string roleName)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<User> repository = unitOfWork.Get<User>();

            return repository.Where(a => a.Roles.Any(r => string.Compare(r.Name, roleName, StringComparison.OrdinalIgnoreCase) == 0)).ToList();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the role described by roleName.
        /// </summary>
        ///
        /// <param name="roleName">
        ///     Name of the role.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public void DeleteRole(string roleName)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();
            IRepository<Role> repository = unitOfWork.Get<Role>();

            Role role = repository.One(r => string.Compare(r.Name, roleName, StringComparison.OrdinalIgnoreCase) == 0);

            repository.Remove(role);

            unitOfWork.Commit();
        }
    }
}
