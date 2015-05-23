namespace Framework.Templates
{
    using System.Security;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for template part.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public interface ITemplatePart
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Renders the part using given context.
        /// </summary>
        /// <param name="context">
        ///     The context to use.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        
        void Render(ITemplateContext context);
    }
}