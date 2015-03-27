namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// Represent Address Information returned.
    /// </summary>
    public struct AddressInfo : IEquatable<AddressInfo>
    {
        /// <summary>
        /// Gets the city.
        /// </summary>
        /// <value>The city specified.</value>
        public string City { get; private set; }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <value>The country.</value>
        public CountryCode Country { get; private set; }

        /// <summary>
        /// Gets the Person’s name associated with this address.
        /// </summary>
        /// <value>Person’s name associated with this address.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public AddressStatus Status { get; private set; }

        /// <summary>
        /// Gets the street.
        /// </summary>
        /// <value>The street.</value>
        public string Street { get; private set; }

        /// <summary>
        /// Gets the ZIP code or other country-specific postal code.
        /// </summary>
        /// <value>ZIP code or other country-specific postal code.</value>
        public string Zip { get; private set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left operator.</param>
        /// <param name="right">The right operator.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(AddressInfo left, AddressInfo right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left operator.</param>
        /// <param name="right">The right operator.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(AddressInfo left, AddressInfo right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// True if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(AddressInfo other)
        {
            return string.Equals(other.City, this.City)
              && object.Equals(other.Country, this.Country)
              && string.Equals(other.Name, this.Name) &&
              string.Equals(other.State, this.State) &&
              object.Equals(other.Status, this.Status)
              && string.Equals(other.Street, this.Street) &&
              string.Equals(other.Zip, this.Zip);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// True if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj.GetType() == typeof(AddressInfo) && this.Equals((AddressInfo)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.City != null ? this.City.GetHashCode() : 0;
                result = (result * 397) ^ this.Country.GetHashCode();
                result = (result * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                result = (result * 397) ^ (this.State != null ? this.State.GetHashCode() : 0);
                result = (result * 397) ^ this.Status.GetHashCode();
                result = (result * 397) ^ (this.Street != null ? this.Street.GetHashCode() : 0);
                result = (result * 397) ^ (this.Zip != null ? this.Zip.GetHashCode() : 0);
                return result;
            }
        }

        internal void Parse(NameValueCollection values)
        {
            this.City = values["address_city"];
            ////this.Country = (CountryCode)Reflector.DescriptionToEnum(typeof(CountryCode), values["address_country_code"]);

            this.Name = values["address_name"];

            this.State = values["address_state"];

            ////this.Status = (AddressStatus)Reflector.DescriptionToEnum(typeof(AddressStatus), values["address_status"]);

            this.Street = values["address_street"];
            this.Zip = values["address_zip"];
        }
    }
}