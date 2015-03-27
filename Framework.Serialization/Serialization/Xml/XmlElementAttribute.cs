using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Serialization.Xml
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute to customize.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class XmlElementAttribute : Attribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the XmlElementAttribute class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public XmlElementAttribute()
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
    }
}
