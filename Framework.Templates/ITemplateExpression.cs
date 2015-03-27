namespace Framework.Templates
{
    using System.Collections.Generic;
    using System.Security;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for template expression.
    /// </summary>
    /// <remarks>
    ///     Anwar Javed, 09/17/2013 3:02 PM.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    public interface ITemplateExpression
    {
        /// <summary>
        ///     Renders this ITemplateExpression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="inverted">true if inverted.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="parts">The parts.</param>
        /// <param name="context">The context.</param>
        /// -------------------------------------------------------------------------------------------------
        /// -------------------------------------------------------------------------------------------------
        [SecurityCritical]
        void Render(
            string expression,
            bool inverted,
            dynamic properties,
            IEnumerable<ITemplatePart> parts,
            ITemplateContext context);
    }
}