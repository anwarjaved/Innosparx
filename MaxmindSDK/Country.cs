namespace MaxmindSDK
{
    /// <summary>
    /// Represent Info about Country.
    /// </summary>
    public class Country
    {
        public Country()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Country(string code, string name)
        {
            this.Name = name;
            this.Code = code;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The country name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The country code.</value>
        public string Code { get; set; }
    }
}
