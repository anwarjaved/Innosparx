namespace Framework.Templates
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for template engine.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public interface ITemplateEngine
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Compiles the template and return as <see cref="ICompiledTemplate" />.
        /// </summary>
        /// <param name="template">
        ///     The template to compile.
        /// </param>
        /// <returns>
        ///     An Instance of <see cref="ICompiledTemplate" />.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        ICompiledTemplate Compile(string template);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Compiles the template and return as <see cref="ICompiledTemplate" />.
        /// </summary>
        /// <param name="templatePath">
        ///     The template Path to compile.
        /// </param>
        /// <returns>
        ///     An Instance of <see cref="ICompiledTemplate" />.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        ICompiledTemplate CompileFile(string templatePath);
    }
}