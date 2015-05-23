namespace Framework.Web.Mvc.UI
{
    using System.Collections.Generic;
    using System.Security;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Drop Down MenuItem.
    /// </summary>
    
    public class DropDownMenuItem : MenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownMenuItem"/> class.
        /// </summary>
        public DropDownMenuItem()
        {
            this.Items = new MenuItemCollection<DropDownMenuItem>();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public MenuItemCollection<DropDownMenuItem> Items { get; private set; }

        /// <summary>
        /// Adds the menu.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>DropDownMenuItem instance.</returns>
        public DropDownMenuItem Add(DropDownMenuItem item)
        {
            this.Items.Add(item);
            return this;
        }

        /// <summary>
        /// Adds the menu.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>DropDownMenuItem instance.</returns>
        public DropDownMenuItem AddRange(params DropDownMenuItem[] items)
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Renders html to the specified writer.
        /// </summary>
        /// <param name="htmlHelper">
        ///     The HTML helper.
        /// </param>
        /// <param name="writer">
        ///     The writer.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        
        public override void Render(HtmlHelper htmlHelper, HtmlTextWriter writer)
        {
            List<string> classes = new List<string> { this.CssClass };

            bool isParent = this.Items.Count > 0;
            string theClass = isParent ? "parent" : "item";

            switch (this.ChildIndex)
            {
                case 0:
                    theClass = theClass + " separator first";
                    break;
                case -1:
                    theClass = theClass + " last";
                    break;
                default:
                    theClass = theClass + " separator index" + this.ChildIndex;
                    break;
            }
            classes.Add(theClass);


            if (CanSelect(htmlHelper, this))
            {
                classes.Add(this.SelectedCssClass);
            }


            string cssClass = classes.ToConcatenatedString().TrimEnd();

            if (!cssClass.IsEmpty())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
            }

            writer.RenderBeginTag("li");

            Dictionary<string, string> htmlAttributes = new Dictionary<string, string>();

            if (!this.Target.IsEmpty())
            {
                htmlAttributes["target"] = this.Target;
            }

            if (this.Tooltip.IsEmpty())
            {
                this.Tooltip = this.Text;
            }

            htmlAttributes["title"] = this.Tooltip;

            if (this.Action.IsEmpty() && this.Controller.IsEmpty())
            {
                htmlAttributes["href"] = this.NavigateUrl;
            }
            else
            {
                UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                htmlAttributes["href"] = urlHelper.Action(this.Action, this.Controller, this.RouteValues);
            }

            foreach (var htmlAttribute in htmlAttributes)
            {
                writer.AddAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            writer.RenderBeginTag("a");
            writer.Write(this.Text);
            writer.RenderEndTag();

            this.Items.Level = this.Level;
            this.Items.Render(this, htmlHelper, writer);

            writer.RenderEndTag();
        }
    }
}
