using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    using Framework.Localization;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A language resource.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/05/2014 3:30 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class LanguageResource : ILanguageResource
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        ///
        /// <value>
        ///     The key.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Key { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the group.
        /// </summary>
        ///
        /// <value>
        ///     The group.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public LanguageCode Code { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the tooltip text.
        /// </summary>
        ///
        /// <value>
        ///     The tooltip text.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string TooltipText { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether we can show tooltip.
        /// </summary>
        ///
        /// <value>
        ///     true if we can show tooltip, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool CanShowTooltip { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the row version.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 3:30 PM.
        /// </remarks>
        ///
        /// <value>
        ///     The row version.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public byte[] RowVersion { get; private set; }
    }
}
