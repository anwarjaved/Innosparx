using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Localization
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for language resource.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public interface ILanguageResource
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
        string Key { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the group.
        /// </summary>
        ///
        /// <value>
        ///     The group.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        string Category { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        LanguageCode Code { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the tooltip text.
        /// </summary>
        ///
        /// <value>
        ///     The tooltip text.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        string TooltipText { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether we can show tooltip.
        /// </summary>
        ///
        /// <value>
        ///     true if we can show tooltip, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        bool CanShowTooltip { get; set; }
    }
}
