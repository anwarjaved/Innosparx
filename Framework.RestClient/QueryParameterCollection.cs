namespace Framework.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Query Parameter Collection.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/25/2011 1:14 AM</datetime>
    [DebuggerDisplay("Count={Count}")]
    public sealed class QueryParameterCollection : List<QueryParameter>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the QueryParameterCollection class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public QueryParameterCollection()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the QueryParameterCollection class.
        /// </summary>
        ///
        /// <param name="parameters">
        ///     Options for controlling the operation.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public QueryParameterCollection(IEnumerable<QueryParameter> parameters)
        {
            foreach (QueryParameter parameter in parameters)
            {
                this.Add(new QueryParameter(parameter.Name, parameter.Value));
            }
        }

        /// <summary>
        /// Adds the specified parameter.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <author>Anwar</author>
        /// <datetime>3/25/2011 3:13 AM</datetime>
        public void Add(string name, string value)
        {
            this.Add(new QueryParameter(name, value));
        }

        /// <summary>
        /// Adds the specified parameter.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <author>Anwar</author>
        /// <datetime>3/25/2011 3:13 AM</datetime>
        public void Add(string name, int value)
        {
            this.Add(new QueryParameter(name, Convert.ToString(value)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds the specified parameter.
        /// </summary>
        ///
        /// <param name="name">
        ///     The parameter name.
        /// </param>
        /// <param name="value">
        ///     The parameter value.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void Add(string name, double value)
        {
            this.Add(new QueryParameter(name, Convert.ToString(value)));
        }
    }
}
