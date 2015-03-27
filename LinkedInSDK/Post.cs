namespace LinkedInSDK
{
    using Framework.Rest;


    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An individual entry in a profile's feed as represented in the Graph API.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/14/2013 12:03 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class Post : BaseRequest
    {
        /// <summary>
        /// Gets or sets the Text of member's comment.
        /// </summary>
        /// <value>The comment.</value>
        /// <remarks> Max length is 700 characters.</remarks>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the Title of shared document.
        /// </summary>
        /// <value>The Title of shared document.</value>
        /// <remarks> Max length is 200 characters.</remarks>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL for shared content.
        /// </summary>
        /// <value>The URL for shared content.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Description of shared content.
        /// </summary>
        /// <value>The Description of shared content.</value>
        /// <remarks>Max length of 256 characters.</remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL for image of shared content.
        /// </summary>
        /// <value>The URL for image of shared content.</value>
        public string ImageUrl { get; set; }


        public PostVisibility Visibility { get; set; }

        protected internal override void AddFields(RestRequest request)
        {
          
        }
    }
}
