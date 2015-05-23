namespace Framework.Web.Mvc.UI
{
    using System.Collections.Generic;
    using System.Security;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Drop Down Menu.
    /// </summary>
    
    public class DropDownMenu : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownMenu"/> class.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="orientation">The orientation.</param>
        public DropDownMenu(HtmlHelper htmlHelper, MenuOrientation orientation)
            : base(htmlHelper)
        {
            this.Orientation = orientation;
            this.Items = new MenuItemCollection<DropDownMenuItem> { CssClass = "menu " + (this.Orientation == MenuOrientation.Horizontal ? "horizontal-menu" : "vertical-menu"), Level = 0 };
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public MenuItemCollection<DropDownMenuItem> Items { get; private set; }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public MenuOrientation Orientation { get; private set; }

        /// <summary>
        /// Adds the menu.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>DropDownMenu Instance.</returns>
        public DropDownMenu Add(DropDownMenuItem item)
        {
            this.Items.Add(item);
            return this;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>DropDownMenu Instance.</returns>
        public DropDownMenu AddRange(params DropDownMenuItem[] items)
        {
            if (items != null)
            {
                foreach (DropDownMenuItem item in items)
                {
                    this.Items.Add(item);
                }
            }

            return this;
        }

        /// <summary>
        /// Renders html to the specified writer.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="writer">The writer.</param>
        
        public override void Render(HtmlHelper htmlHelper, HtmlTextWriter writer)
        {
            if (this.Items.Count > 0)
            {
                List<string> classes = new List<string> { this.CssClass, "menu", (this.Orientation == MenuOrientation.Horizontal ? "horizontal-menu" : "vertical-menu") };

                writer.AddAttribute(HtmlTextWriterAttribute.Class, classes.ToArray().ToConcatenatedString());
                writer.RenderBeginTag("nav");
                this.Items.Render(this, htmlHelper, writer);
                writer.RenderEndTag();
            }
        }
    }
}
