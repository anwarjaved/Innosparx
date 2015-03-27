namespace Framework.Templates
{
    using System;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for template context.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public interface ITemplateContext : IDisposable
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the template locator.
        /// </summary>
        /// <value>
        ///     The template locator.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        Func<string, string> TemplateLocator { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets new context.
        /// </summary>
        /// <param name="value">
        ///     The instance.
        /// </param>
        /// <returns>
        ///     The new context.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        ITemplateContext GetContext(object value);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a template.
        /// </summary>
        /// <param name="template">
        ///     The template.
        /// </param>
        /// <returns>
        ///     The template.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        ICompiledTemplate GetTemplate(string template);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <returns>
        ///     The value.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        object GetValue(string name);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Write Text to Current Writer.
        /// </summary>
        /// <param name="text">
        ///     The text to write.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        void Write(string text);
    }
}