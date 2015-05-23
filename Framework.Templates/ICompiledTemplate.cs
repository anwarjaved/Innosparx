namespace Framework.Templates
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for compiled template.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public interface ICompiledTemplate
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the parts.
        /// </summary>
        /// <value>
        ///     The parts.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        IEnumerable<ITemplatePart> Parts { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Renders the given data.
        /// </summary>
        /// <param name="data">
        ///     The data.
        /// </param>
        /// <param name="templateLocator">
        ///     The template locator.
        /// </param>
        /// <returns>
        ///     Processed string.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        
        string Render(object data, Func<string, string> templateLocator = null);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Renders the given data.
        /// </summary>
        /// <param name="writer">
        ///     The writer.
        /// </param>
        /// <param name="data">
        ///     The data.
        /// </param>
        /// <param name="templateLocator">
        ///     The template locator.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        
        void Render(TextWriter writer, object data, Func<string, string> templateLocator = null);
    }
}