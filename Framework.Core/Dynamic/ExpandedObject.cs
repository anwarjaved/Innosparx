namespace Framework.Dynamic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Class that provides extensible properties and methods to an
    /// existing object when cast to dynamic. This
    /// dynamic object stores 'extra' properties in a dictionary or
    /// checks the actual properties of the instance passed via 
    /// constructor.
    /// This class can be subclass to extend an existing type or 
    /// you can pass in an instance to extend. Properties (both
    /// dynamic and strongly typed) can be accessed through an 
    /// indexer.
    /// This type allows you three ways to access its properties:
    /// Directly: any explicitly declared properties are accessible
    /// Dynamic: dynamic cast allows access to dictionary and native properties/methods
    /// Dictionary: Any of the extended properties are accessible via IDictionary interface
    /// </summary>
    [Serializable]
    public sealed class ExpandedObject : DynamicObject, IDictionary<string, object>
    {
        private readonly IDictionary<string, object> values;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ExpandedObject class.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public ExpandedObject(IEnumerable<KeyValuePair<string, object>> values = null)
        {
            this.values = values != null
                              ? values.ToDictionary(
                                  p => p.Key,
                                  p => DynamicExtensions.WrapObject(p.Value),
                                  StringComparer.OrdinalIgnoreCase)
                              : new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The keys.</value>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<string> Keys
        {
            get
            {
                return this.values.Keys;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <value>The values.</value>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<object> Values
        {
            get
            {
                return this.values.Values;
            }
        }

        int ICollection<KeyValuePair<string, object>>.Count
        {
            get
            {
                return this.values.Count;
            }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get
            {
                return this.values.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="DynamicObject"/>.</returns>
        public dynamic this[string key]
        {
            get
            {
                return this.GetValue(key);
            }

            set
            {
                this.SetValue(key, value);
            }
        }

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                return this[key];
            }

            set
            {
                this[key] = value;
            }
        }

        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the<see cref="T:System.Dynamic.DynamicObject" />class can override this method to specify
        /// dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides
        /// the type to which the object must be converted. For example, for the statement
        /// (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where
        /// sampleObject is an instance of the class derived from the<see cref="T:System.Dynamic.DynamicObject" />class, binder.Type returns the<see cref="T:System.String" />type. The binder.Explicit property provides information
        /// about the kind of conversion that occurs. It returns true for explicit conversion and
        /// false for implicit conversion.</param>
        /// <param name="result">The result of the type conversion operation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the
        /// run-time binder of the language determines the behavior. (In most cases, a language-
        /// specific run-time exception is thrown.)</returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = null;
            if (binder.Type.IsInstanceOfType(this.values))
            {
                result = this.values;
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Unable to convert to \"{0}\"", binder.Type));
            }

            return true;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from
        /// the<see cref="T:System.Dynamic.DynamicObject"/>class can override this method to
        /// specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">
        /// Provides information about the object that called the dynamic operation. The binder.Name
        /// property provides the name of the member on which the dynamic operation is performed. For
        /// example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where
        /// sampleObject is an instance of the class derived from the<see cref="T:System.Dynamic.DynamicObject"/>class, binder.Name returns
        /// "SampleProperty". The binder.IgnoreCase property specifies whether the member name is
        /// case-sensitive.
        /// </param>
        /// <param name="result">
        /// The result of the get operation. For example, if the method is called for a property, you
        /// can assign the property value to<paramref name="result"/>.
        /// </param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the
        /// run-time binder of the language determines the behavior. (In most cases, a run-time
        /// exception is thrown.)
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.GetValue(binder.Name);
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from
        /// the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to
        /// specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name
        /// property provides the name of the member to which the value is being assigned. For
        /// example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an
        /// instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" />
        /// class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies
        /// whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test",
        /// where sampleObject is an instance of the class derived from the
        /// <see cref="T:System.Dynamic.DynamicObject" /> class, the <paramref name="value" /> is
        /// "Test".</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the
        /// run-time binder of the language determines the behavior. (In most cases, a language-
        /// specific run-time exception is thrown.)</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.SetValue(binder.Name, value);
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that set a value by index. Classes derived
        /// from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to
        /// specify dynamic behavior for operations that access objects by a specified index.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] = 10
        /// operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived
        /// from the <see cref="T:System.Dynamic.DynamicObject" /> class,
        /// first value in <paramref name="indexes" /> is equal to 3.</param>
        /// <param name="value">The value to set to the object that has the specified index. For example, for the
        /// sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where
        /// sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class,
        /// <paramref name="value" /> is equal to 10.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the
        /// run-time binder of the language determines the behavior. (In most cases, a language-
        /// specific run-time exception is thrown.</returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string key = GetKey(indexes);
            if (!string.IsNullOrEmpty(key))
            {
                this.SetValue(key, value);
            }

            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived
        /// from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to
        /// specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3]
        /// operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the
        /// DynamicObject class, first value in <paramref name="indexes" /> is equal to 3.</param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the
        /// run-time binder of the language determines the behavior. (In most cases, a run-time
        /// exception is thrown.)</returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            string key = GetKey(indexes);
            result = null;
            if (!string.IsNullOrEmpty(key))
            {
                result = this.GetValue(key);
            }

            return true;
        }

        /// <summary>
        /// Returns the enumeration of all dynamic member names.
        /// </summary>
        /// <returns>A sequence that contains dynamic member names.</returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.values.Keys;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            this.SetValue(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            this.values.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return this.values.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.values.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return this.values.Remove(item);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return this.values.ContainsKey(key);
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            this.SetValue(key, value);
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return this.values.Remove(key);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return this.values.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a key.
        /// </summary>
        /// <param name="indexes">The indexes.</param>
        /// <returns>The key.</returns>
        private static string GetKey(object[] indexes)
        {
            if (indexes.Length == 1)
            {
                return (string)indexes[0];
            }
    
            // REVIEW: Should this throw?
            return null;
        }

        private object GetValue(string name)
        {
            object result;
            if (this.values.TryGetValue(name, out result))
            {
                return result;
            }

            return null;
        }

        private void SetValue(string name, object value)
        {
            if (this.values.ContainsKey(name))
            {
                this.values[name] = DynamicExtensions.WrapObject(value);
            }

            this.values.Add(name, DynamicExtensions.WrapObject(value));
        }
    }
}