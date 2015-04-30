namespace Framework.Rest
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;


    /// <summary>
    /// Provides an internal structure to sort the query parameter.
    /// </summary>
    [DebuggerDisplay("{Name}={Value}")]
    public sealed class QueryParameter : IEquatable<QueryParameter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameter"/> class.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 11:20 PM</datetime>
        public QueryParameter(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The parameter name.</value>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 11:20 PM</datetime>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 11:20 PM</datetime>
        public string Value { get; private set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of the operator.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 1:26 AM</datetime>
        public static bool operator ==(QueryParameter left, QueryParameter right)
        {
            return object.Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of the operator.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 1:26 AM</datetime>
        public static bool operator !=(QueryParameter left, QueryParameter right)
        {
            return !object.Equals(left, right);
        }

        /// <summary>
        /// Tries to the parse querystring parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="text">The text to parse.</param>
        /// <returns>
        /// The value of the parameter or an empty string.
        /// </returns>
        public static string ParseQuerystringParameter(string parameterName, string text)
        {
            Match expressionMatch = Regex.Match(text, string.Format("{0}=(?<value>[^&]+)", parameterName));
            return !expressionMatch.Success ? string.Empty : HttpUtility.UrlDecode(expressionMatch.Groups["value"].Value);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(QueryParameter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return object.Equals(other.Name, this.Name) && object.Equals(other.Value, this.Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is QueryParameter && this.Equals((QueryParameter)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^
                       (this.Value != null ? this.Value.GetHashCode() : 0);
            }
        }
    }
}
