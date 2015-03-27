namespace Framework.Fakes
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.SessionState;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fake HTTP session state.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class FakeHttpSessionState : HttpSessionStateBase
    {
        private readonly SessionStateItemCollection sessionItems;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeHttpSessionState class.
        /// </summary>
        /// <param name="sessionItems">
        /// The session items.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public FakeHttpSessionState(SessionStateItemCollection sessionItems)
        {
            this.sessionItems = sessionItems;
        }

        /// <summary>
        /// When overridden in a derived class, gets the number of items in the session-state collection.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of items in the collection.</returns>
        public override int Count
        {
            get
            {
                return this.sessionItems.Count;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a collection of the keys for all values that are stored in the session-state collection.
        /// </summary>
        /// <value>The keys.</value>
        /// <returns>The session keys.</returns>
        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return this.sessionItems.Keys;
            }
        }
        
        /// <summary>
        /// When overridden in a derived class, gets or sets a session value by using the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>An <see cref="object"/> from session.</returns>
        public override object this[string name]
        {
            get
            {
                return this.sessionItems[name];
            }

            set
            {
                this.sessionItems[name] = value;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets a session value by using the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>An <see cref="object"/> from session.</returns>
        public override object this[int index]
        {
            get
            {
                return this.sessionItems[index];
            }

            set
            {
                this.sessionItems[index] = value;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// When overridden in a derived class, adds an item to the session-state collection.
        /// </summary>
        /// <param name="name">
        /// The name of the item to add to the session-state collection.
        /// </param>
        /// <param name="value">
        /// The value of the item to add to the session-state collection.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public override void Add(string name, object value)
        {
            this.sessionItems[name] = value;
        }

        /// <summary>
        /// Check whether the specified key exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Exists(string key)
        {
            return this.sessionItems[key] != null;
        }

        /// <summary>
        /// When overridden in a derived class, returns an enumerator that can be used to read all the session-state variable names in the current session.
        /// </summary>
        /// <returns>An enumerator that can iterate through the variable names in the session-state collection.</returns>
        public override IEnumerator GetEnumerator()
        {
            return this.sessionItems.GetEnumerator();
        }

        /// <summary>
        /// When overridden in a derived class, deletes an item from the session-state collection.
        /// </summary>
        /// <param name="name">The name of the item to delete from the session-state collection.</param>
        public override void Remove(string name)
        {
            this.sessionItems.Remove(name);
        }
    }
}