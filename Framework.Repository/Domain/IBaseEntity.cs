namespace Framework.Domain
{
    using System.ComponentModel;

    using Framework.DataAccess;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for base entity.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public interface IBaseEntity
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether the deleted.
        /// </summary>
        ///
        /// <value>
        ///     true if deleted, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Deleted { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is modified.
        /// </summary>
        ///
        /// <value>
        ///     true if modified, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        bool Modified { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the row version.
        /// </summary>
        ///
        /// <value>
        ///     The row version.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        byte[] RowVersion { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the load action.
        /// </summary>
        ///
        /// <param name="unitOfWork">
        ///     The unit of work.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void OnLoad(IUnitOfWork unitOfWork);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the before save action.
        /// </summary>
        ///
        /// <param name="unitOfWork">
        ///     The unit of work.
        /// </param>
        /// <param name="added">
        ///     true if added.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void OnBeforeSave(IUnitOfWork unitOfWork, bool added);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the save action.
        /// </summary>
        ///
        /// <param name="unitOfWork">
        ///     The unit of work.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void OnSave(IUnitOfWork unitOfWork);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the remove action.
        /// </summary>
        ///
        /// <param name="unitOfWork">
        ///     The unit of work.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void OnRemove(IUnitOfWork unitOfWork);
    }
}
