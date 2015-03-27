namespace Framework.Web.Mvc.UI
{
    using System.Security;

    [SecurityCritical]
    public interface IMenuSelection
    {
        /// <summary>
        /// Gets or sets the selected CSS class.
        /// </summary>
        /// <value>The selected CSS class.</value>
        string SelectedCssClass { get;  }

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>The selection mode.</value>
        SelectionMode? SelectionMode { get; }
    }
}