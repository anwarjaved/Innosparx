namespace Framework.Ioc
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Used to pass static information for value type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ValueAttribute : Attribute
    {
        private readonly object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using a <see cref="T:System.Boolean"></see> value.
        /// </summary>
        /// <param name="value">A <see cref="T:System.Boolean"></see> that is the default value.</param>
        public ValueAttribute(bool value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using an 8-bit unsigned integer.
        /// </summary>
        /// <param name="value">
        /// An 8-bit unsigned integer that is the default value. 
        /// </param>
        public ValueAttribute(byte value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using a Unicode character.
        /// </summary>
        /// <param name="value">
        /// A Unicode character that is the default value. 
        /// </param>
        public ValueAttribute(char value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using a double-precision floating point number.
        /// </summary>
        /// <param name="value">
        /// A double-precision floating point number that is the default value. 
        /// </param>
        public ValueAttribute(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using a 16-bit signed integer.
        /// </summary>
        /// <param name="value">
        /// A 16-bit signed integer that is the default value. 
        /// </param>
        public ValueAttribute(short value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using a 32-bit signed integer.
        /// </summary>
        /// <param name="value">
        /// A 32-bit signed integer that is the default value. 
        /// </param>
        public ValueAttribute(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using a 64-bit signed integer.
        /// </summary>
        /// <param name="value">
        /// A 64-bit signed integer that is the default value. 
        /// </param>
        public ValueAttribute(long value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using a single-precision floating point number.
        /// </summary>
        /// <param name="value">
        /// A single-precision floating point number that is the default value. 
        /// </param>
        public ValueAttribute(float value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class using a <see cref="T:System.String"></see>.
        /// </summary>
        /// <param name="value">
        /// A <see cref="T:System.String"></see> that is the default value. 
        /// </param>
        public ValueAttribute(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAttribute"></see> class, converting the specified value to the specified type, and using an invariant culture as the translation context.
        /// </summary>
        /// <param name="type">
        /// A <see cref="T:System.Type"></see> that represents the type to convert the value to. 
        /// </param>
        /// <param name="value">
        /// A <see cref="T:System.String"></see> that can be converted to the type using the <see cref="T:System.ComponentModel.TypeConverter"></see> for the type and the U.S. English culture. 
        /// </param>
        public ValueAttribute(Type type, string value)
        {
            try
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
                this.value = typeConverter.ConvertFromInvariantString(value);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets the default value of the property this attribute is bound to.
        /// </summary>
        /// <value>
        /// An <see cref="T:System.Object"></see> that represents the default value of the property this attribute is bound to.
        /// </value>
        public object Value
        {
            get
            {
                return this.GetValue(this.value);
            }
        }

        /// <summary>
        /// Returns whether the value of the given object is equal to the current <see cref="ValueAttribute"></see>.
        /// </summary>
        /// <returns>
        /// True if the value of the given object is equal to that of the current; otherwise, false.
        /// </returns>
        /// <param name="obj">
        /// The object to test the value equality of. 
        /// </param>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            var attribute = obj as ValueAttribute;
            if (attribute == null)
            {
                return false;
            }

            if (this.Value != null)
            {
                return this.Value.Equals(attribute.Value);
            }

            return attribute.Value == null;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="objValue">The value.</param>
        /// <returns>Returns Value.</returns>
        protected virtual object GetValue(object objValue)
        {
            return objValue;
        }
    }
}
