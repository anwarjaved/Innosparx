namespace Framework.Web.Mvc.UI
{
    using System.Security;
    using System.Web.Mvc;
    using System.Web.UI;

    using Framework.Web.Mvc;

    /// <summary>
    ///  Represent a Menu Item.
    /// </summary>
    
    public abstract class MenuItem : IHtmlRenderer, IMenuSelection
    {
        private string selectedCssClass;

        private SelectionMode? selectionMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        protected MenuItem()
        {
            this.NavigateUrl = "javaScript:void(0);";
        }

        /// <summary>
        /// Gets or sets the text displayed for the menu item.
        /// </summary>
        /// <value>The text displayed for the menu item.</value>
        public string Text { get; set; }

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
            
            get
            {
                return string.IsNullOrWhiteSpace(this.selectedCssClass) ? "selected" : this.selectedCssClass;
            }
            set
            {
                this.selectedCssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>The selection mode.</value>
        public SelectionMode SelectionMode
        {
            get
            {
                return this.selectionMode.HasValue ? this.selectionMode.Value : SelectionMode.ControllerAction;
            }
            set
            {
                this.selectionMode = value;
            }
        }

        SelectionMode? IMenuSelection.SelectionMode
        {
            
            get
            {
                return this.selectionMode.HasValue ? this.selectionMode.Value : SelectionMode.ControllerAction;
            }
        }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>The name of the action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>The route values.</value>
        /// <author>Anwar</author>
        /// <datetime>1/25/2011 5:44 PM</datetime>
        public object RouteValues { get; set; }

        /// <summary>
        /// Gets or sets the navigate URL.
        /// </summary>
        /// <value>
        /// The navigate URL.
        /// </value>
        public string NavigateUrl { get; set; }


        /// <summary>
        /// Gets or sets the on client click.
        /// </summary>
        /// <value>The on client click.</value>
        public string OnClientClick { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>The tooltip.</value>
        public string Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public string Target { get; set; }

        internal int Level { get; set; }
        internal int ChildIndex { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determines whether the specified menu can be selected.
        /// </summary>
        /// <param name="htmlHelper">
        ///     The HTML helper.
        /// </param>
        /// <param name="item">
        ///     The menu item.
        /// </param>
        ///
        /// <returns>
        ///     <see langword="true"/>If the specified HTML helper is selected; otherwise,
        ///     <see langword="false"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool CanSelect(HtmlHelper htmlHelper, MenuItem item)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            switch (item.SelectionMode)
            {
                case SelectionMode.ControllerAction:
                    return urlHelper.IsCurrentControllerAction(item.Action, item.Controller);
                case SelectionMode.Controller:
                    return urlHelper.IsCurrentController(item.Controller);
                case SelectionMode.Action:
                    return urlHelper.IsCurrentAction(item.Action);
            }

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Renders html to the specified writer.
        /// </summary>
        ///
        /// <param name="htmlHelper">
        ///     The HTML helper.
        /// </param>
        /// <param name="writer">
        ///     The writer.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        
        public abstract void Render(HtmlHelper htmlHelper, HtmlTextWriter writer);
    }
}
