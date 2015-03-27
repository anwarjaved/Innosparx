namespace Framework.Dynamic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Expanded array.
    /// </summary>
    /// <remarks>
    /// Anwar Javed, 09/21/2013 3:39 PM.
    /// </remarks>
    public sealed class ExpandedArray : DynamicObject, IEnumerable<object>
    {
        private readonly object[] arrayValues;

        /// <summary>
        /// Initializes a new instance of the ExpandedArray class.
        /// </summary>
        /// <param name="arrayValues">The array values.</param>
        public ExpandedArray(IEnumerable<object> arrayValues)
        {
            this.arrayValues = arrayValues.Select(DynamicExtensions.WrapObject).ToArray();
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get
            {
                return this.arrayValues.Length;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DynamicObject"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A <see cref="DynamicObject"/> object.</returns>
        public dynamic this[int index]
        {
            get
            {
                return this.arrayValues[index];
            }

            set
            {
                this.arrayValues[index] = DynamicExtensions.WrapObject(value);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ExpandedArray"/> to <see cref="Array"/> of <see cref="System.Object"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator object[](ExpandedArray obj)
        {
            return obj.arrayValues;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ExpandedArray"/> to <see cref="Array"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Array(ExpandedArray obj)
        {
            return obj.arrayValues;
        }

        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Type returns the <see cref="T:System.String" /> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param>
        /// <param name="result">The result of the type conversion operation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (this.arrayValues.GetType().IsAssignableFrom(binder.Type))
            {
                result = this.arrayValues;
                return true;
            }

            return base.TryConvert(binder, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result" />.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Testing for members should never throw. This is important when dealing with
            // services that return different json results. Testing for a member shouldn't throw,
            // it should just return null (or undefined)
            result = null;
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator GetEnumerator()
        {
            return this.arrayValues.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return this.GetEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerable.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Object}"/>.</returns>
        private IEnumerable<object> GetEnumerable()
        {
            return this.arrayValues.AsEnumerable();
        }
    }
}
