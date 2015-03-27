using System;
using System.Collections.Generic;

namespace Framework.Serialization.Xml
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for XML property.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class XmlPropertyAttribute : Attribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the XmlElementAttribute class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public XmlPropertyAttribute()
        {
            this.NamespaceList = new Dictionary<string, string>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the default namespace.
        /// </summary>
        ///
        /// <value>
        ///     The default namespace.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string DefaultNamespace { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a list of namespaces.
        /// </summary>
        ///
        /// <value>
        ///     A List of namespaces.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public Dictionary<string, string> NamespaceList { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        ///
        /// <value>
        ///     The name of the item.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ItemName { get; set; }
    }
}
