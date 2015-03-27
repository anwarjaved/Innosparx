namespace Framework.Domain
{
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Framework.DataAccess;

    using Newtonsoft.Json;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Base entity.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public abstract class BaseEntity : IBaseEntity
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [JsonIgnore]
        [IgnoreDataMember]
        [XmlIgnore]
        bool IBaseEntity.Deleted { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object is modified.
        /// </summary>
        ///
        /// <value>
        ///     true if modified, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [JsonIgnore]
        [IgnoreDataMember]
        [XmlIgnore]
        bool IBaseEntity.Modified { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the row version.
        /// </summary>
        ///
        /// <value>
        ///     The row version.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonIgnore]
        [IgnoreDataMember]
        [XmlIgnore]
        public byte[] RowVersion { get; protected internal set; }

        void IBaseEntity.OnLoad(IUnitOfWork unitOfWork)
        {
            this.OnLoad(unitOfWork);
        }

        void IBaseEntity.OnBeforeSave(IUnitOfWork unitOfWork, bool added)
        {
            this.OnBeforeSave(unitOfWork, added);
        }

        void IBaseEntity.OnSave(IUnitOfWork unitOfWork)
        {
            this.OnSave(unitOfWork);
        }

        void IBaseEntity.OnRemove(IUnitOfWork unitOfWork)
        {
            this.OnRemove(unitOfWork);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates the collection.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        ///
        /// <returns>
        ///     The new collection&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected EntityCollection<T> CreateCollection<T>() where T : class, IBaseEntity, new()
        {
            return new EntityCollection<T>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the load action.
        /// </summary>
        ///
        /// <param name="unitOfWork">
        ///     The unit of work.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void OnLoad(IUnitOfWork unitOfWork)
        {
        }

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
        protected virtual void OnBeforeSave(IUnitOfWork unitOfWork, bool added)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the save action.
        /// </summary>
        ///
        /// <param name="unitOfWork">
        ///     The unit of work.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void OnSave(IUnitOfWork unitOfWork)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the remove action.
        /// </summary>
        ///
        /// <param name="unitOfWork">
        ///     The unit of work.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected virtual void OnRemove(IUnitOfWork unitOfWork)
        {
        }
    }
}
