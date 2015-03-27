namespace Framework.Web.Mvc.UI
{
    using System.Collections.ObjectModel;
    using System.Security;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Represents Collection of Menu Items.
    /// </summary>
    /// <typeparam name="T">Type of Menu.</typeparam>
    [SecurityCritical]
    public class MenuItemCollection<T> : Collection<T> where T : MenuItem, IHtmlRenderer
    {
        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>The CSS class.</value>
        public string CssClass { get; set; }

        internal int Level { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Renders html to the specified writer.
        /// </summary>
        ///
        /// <param name="menuSelection">
        ///     The menu.
        /// </param>
        /// <param name="htmlHelper">
        ///     The HTML helper.
        /// </param>
        /// <param name="writer">
        ///     The writer.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public void Render(IMenuSelection menuSelection, HtmlHelper htmlHelper, HtmlTextWriter writer)
        {
            if (this.Items.Count > 0)
            {
                if (this.Level == 0)
                {
                    this.CssClass = "level0 " + this.CssClass;
                }
                else
                {
                    this.CssClass = "level{0} ".FormatString(this.Level) + this.CssClass;
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);


                writer.RenderBeginTag("ul");
                for (int index = 0; index < this.Items.Count; index++)
                {
                    T item = this.Items[index];

                    if (!string.IsNullOrWhiteSpace(menuSelection.SelectedCssClass))
                    {
                        item.SelectedCssClass = menuSelection.SelectedCssClass;
                    }

                    if (menuSelection.SelectionMode.HasValue)
                    {
                        item.SelectionMode = menuSelection.SelectionMode.Value;
                    }

                    item.Level = this.Level + 1;
                    int childIndex = index;
                    if (childIndex == this.Items.Count - 1)
                    {
                        childIndex = -1;
                    }

                    item.ChildIndex = childIndex;
                    item.Render(htmlHelper, writer);
                }

                writer.RenderEndTag();
            }
        }
    }
}
