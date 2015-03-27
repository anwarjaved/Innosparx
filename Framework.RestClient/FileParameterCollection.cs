using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Rest
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Collection of file parameters.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [DebuggerDisplay("Count={Count}")]
    public sealed class FileParameterCollection : Collection<FileParameter>
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the length of the content.
        /// </summary>
        ///
        /// <value>
        ///     The length of the content.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public long ContentLength
        {
            get
            {
                return this.Count > 0 ? this.Sum(file => file.ContentLength) : 0;
            }
        }
    }
}