namespace Framework
{
    using System.Security;
    using System.Web.Mvc;

    using Framework.Web.Mvc.UI;

    /// <summary>
    /// Menu Extensions.
    /// </summary>
    public static class MenuExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Drops down menu EXtension.
        /// </summary>
        ///
        /// <param name="htmlHelper">
        ///     The HTML helper.
        /// </param>
        /// <param name="orientation">
        ///     The orientation.
        /// </param>
        /// <param name="cssClass">
        ///     The CSS class.
        /// </param>
        /// <param name="selectedCssClass">
        ///     (optional) the selected CSS class.
        /// </param>
        /// <param name="selectionMode">
        ///     (optional) the selection mode.
        /// </param>
        ///
        /// <returns>
        ///     An instance of DropDownMenu.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static DropDownMenu DropDownMenu(this HtmlHelper htmlHelper, MenuOrientation orientation = MenuOrientation.Horizontal, string cssClass = null, string selectedCssClass = null, SelectionMode? selectionMode = null)
        {
            return new DropDownMenu(htmlHelper, orientation) { CssClass = cssClass, SelectedCssClass = selectedCssClass, SelectionMode = selectionMode};
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Drops down menu EXtension.
        /// </summary>
        ///
        /// <param name="htmlHelper">
        ///     The HTML helper.
        /// </param>
        /// <param name="cssClass">
        ///     The CSS class.
        /// </param>
        /// <param name="selectedCssClass">
        ///     (optional) the selected CSS class.
        /// </param>
        /// <param name="selectionMode">
        ///     (optional) the selection mode.
        /// </param>
        ///
        /// <returns>
        ///     An instance of DropDownMenu.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static DropDownMenu DropDownMenu(this HtmlHelper htmlHelper, string cssClass = null, string selectedCssClass = null, SelectionMode? selectionMode = null)
        {
            return htmlHelper.DropDownMenu(MenuOrientation.Horizontal, cssClass, selectedCssClass, selectionMode);
        }
    }
}
