namespace Framework.Membership
{
    using System;
    using System.Collections.Generic;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for user.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public interface IUser
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        ///
        /// <value>
        ///     The identifier.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        Guid ID { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the email.
        /// </summary>
        ///
        /// <value>
        ///     The email.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        string Email { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        string Name { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the date of the last activity.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last activity.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        DateTime? LastActivityDate { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the person's first name.
        /// </summary>
        ///
        /// <value>
        ///     The name of the first.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        string FirstName { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the person's last name.
        /// </summary>
        ///
        /// <value>
        ///     The name of the last.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        string LastName { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is verified.
        /// </summary>
        ///
        /// <value>
        ///     true if this object is verified, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        bool IsVerified { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the password failure since last success.
        /// </summary>
        ///
        /// <value>
        ///     The password failure since last success.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        int PasswordFailureSinceLastSuccess { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last password failure.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last password failure.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        DateTime? LastPasswordFailureDate { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last lockout.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last lockout.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        DateTime? LastLockoutDate { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last login.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last login.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        DateTime? LastLoginDate { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is locked out.
        /// </summary>
        ///
        /// <value>
        ///     true if this object is locked out, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        bool IsLockedOut { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is suspended.
        /// </summary>
        ///
        /// <value>
        ///     true if this object is suspended, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        bool IsSuspended { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last password changed.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last password changed.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        DateTime? LastPasswordChangedDate { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the phone.
        /// </summary>
        ///
        /// <value>
        ///     The phone.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        string Phone { get; set; }

        /// <summary>
        /// Determines whether the specified account has permission.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        ///   <see langword="true"/> If the specified name has permission; otherwise, <see langword="false"/>.
        /// </returns>
        bool IsInRole(string roleName);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the roles.
        /// </summary>
        ///
        /// <value>
        ///     The roles.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        ICollection<IRole> Roles { get; }
    }
}