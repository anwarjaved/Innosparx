using System;

namespace Framework
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for context name.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ContextNameAttribute : Attribute
    {
        private readonly string name;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the ContextNameAttribute class.
        /// </summary>
        ///
        /// <param name="name">
        ///     The name.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public ContextNameAttribute(string name)
        {
            this.name = name;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}
