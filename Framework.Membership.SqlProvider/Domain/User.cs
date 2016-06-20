using System;

namespace Framework.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Framework.Membership;

    /// <summary>
    /// Represent a User in the system.
    /// </summary>
    public class User : AggregateEntity, IUser
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        ///
        /// <value>
        ///     The email.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Email { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the person's first name.
        /// </summary>
        ///
        /// <value>
        ///     The name of the first.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string FirstName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the person's last name.
        /// </summary>
        ///
        /// <value>
        ///     The name of the last.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string LastName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is verified.
        /// </summary>
        ///
        /// <value>
        ///     true if this object is verified, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool IsVerified { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the password failure since last success.
        /// </summary>
        ///
        /// <value>
        ///     The password failure since last success.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int PasswordFailureSinceLastSuccess { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last password failure.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last password failure.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? LastPasswordFailureDate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last activity.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last activity.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? LastActivityDate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last lockout.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last lockout.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? LastLockoutDate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last login.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last login.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? LastLoginDate { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is locked out.
        /// </summary>
        ///
        /// <value>
        ///     true if this object is locked out, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool IsLockedOut { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is suspended.
        /// </summary>
        ///
        /// <value>
        ///     true if this object is suspended, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool IsSuspended { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date of the last password changed.
        /// </summary>
        ///
        /// <value>
        ///     The date of the last password changed.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? LastPasswordChangedDate { get; set; }

        string IUser.Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.FirstName))
                {
                    return this.Email;
                }

                return this.FirstName + " " + (this.LastName ?? "");
            }
        }

        ICollection<IRole> IUser.Roles
        {
            get
            {
                return this.GetRoles();
            }
        }

        protected virtual ICollection<IRole> GetRoles()
        {
            return this.Roles.Cast<IRole>().ToList();
        }

        private EntityCollection<Role> roles;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the roles.
        /// </summary>
        ///
        /// <value>
        ///     The roles.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public virtual EntityCollection<Role> Roles
        {
            get { return this.roles ?? (this.roles = this.CreateCollection<Role>()); }
        }

        /// <summary>
        /// Determines whether the specified account has permission.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        ///   <see langword="true"/> If the specified name has permission; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsInRole(string roleName)
        {
            return this.Roles.Any(p => string.Compare(roleName, p.Name, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private PasswordInfo password;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        ///
        /// <value>
        ///     The password.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public virtual PasswordInfo Password
        {
            get
            {
                return this.password ?? (this.password = new PasswordInfo());
            }

            set { this.password = value; }
        }
    }
}
