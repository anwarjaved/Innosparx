namespace Framework.Web.Mvc.UI
{
    /// <summary>
    /// Represents Checkbox list.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>1/22/2011 3:34 PM</datetime>
    public class CheckBoxListItem
    {
        private string value;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The checkbox label.</value>
        /// <author>Anwar</author>
        /// <datetime>1/22/2011 3:42 PM</datetime>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <author>Anwar</author>
        /// <datetime>1/22/2011 3:42 PM</datetime>
        public string Value
        {
            get
            {
                return string.IsNullOrEmpty(this.value) ? this.Text : this.value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CheckBoxListItem"/> is selected.
        /// </summary>
        /// <value>
        /// <see langword="true"/> If selected; otherwise, <see langword="false"/>.
        /// </value>
        /// <author>Anwar</author>
        /// <datetime>1/22/2011 3:42 PM</datetime>
        public bool Selected { get; set; }
    }
}
