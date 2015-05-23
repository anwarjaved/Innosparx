namespace Framework.Web.Mvc.UI
{
    using System.IO;
    using System.Security;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Represents Menu.
    /// </summary>
    
    public abstract class Menu : Control, IHtmlRenderer, IMenuSelection
    {
        private readonly HtmlHelper htmlHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        protected Menu(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>The CSS class.</value>
        public string CssClass { get; set; }


        /// <summary>
        /// Gets or sets the selected CSS class.
        /// </summary>
        /// <value>The selected CSS class.</value>
        public string SelectedCssClass
        {
            
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>The selection mode.</value>
        public SelectionMode? SelectionMode
        {
            
            get;
            set;
        }

        /// <summary>
        /// Renders this menu.
        /// </summary>
        /// <returns>Output as MvcHtmlString.</returns>
        
        public override IHtmlString Render()
        {
            StringBuilder builder = new StringBuilder();
            using (HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(builder)))
            {
                this.Render(this.htmlHelper, writer);
            }

            return HtmlStringExtensions.ToHtmlString(builder);
        }

        /// <summary>
        /// Renders html to the specified writer.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="writer">The writer.</param>
        
        public abstract void Render(HtmlHelper htmlHelper, HtmlTextWriter writer);
    }
}
